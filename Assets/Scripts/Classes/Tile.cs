using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WorldBuilder { 

    public class Tile
    {

        //Enums Data driven states of the tile
        public enum TileType { Empty, Grass, Rock, Water, Castle };
        public enum TileState { Neutral, Claimed, Besieged };
        public enum TileMovementDirection { Up, Down, Left, Right, Center };
        private TileType type = TileType.Empty;
        private TileState state = TileState.Neutral;
        private TileMovementDirection moveDirection = TileMovementDirection.Center;

        //Game positions
        private readonly int x, z;
        private readonly float tileGraphicsHeight = 0.2f;//make it so units spawn on top of the tile  
        private readonly Vector3Int vec3Pos;//maybe this is a bit redutant?(because we already have gameObject.transfor witcht already holds the same information)

        //Gameplay Variablas
        private Building building;
        GameObject go_Tile;
        Player owner =  mono_playerManager.p0; //neutral player as an owner
        float spawnRate = 2.0f;
        public int unitMax = 10;



        //after researching i found that Lists should be used here. doesnt matter if theoreticly this is a dyniamic collection. 
        //Also we have to take to considaration that leaving this uncaped might crash peoples hardware.
        // Creating a list with an initial size
        public List<Unit> units = new List<Unit>(500);
        //public ArrayList units = new ArrayList(); 
        public float capturePoints = 0;

        //neighboring tiles
        private Tile _tileNorth, _tileSouth, _tileEast, _tileWest;
        mono_Border mnBorder;







        public Tile(Vector3Int Location, TileType type = TileType.Grass, TileState state = TileState.Neutral)
        {
            this.x = Location.x;
            this.z = Location.z;
            this.State = state;
            this.vec3Pos = Location;
            //SET game object
            go_Tile = GameObject.Instantiate(Resources.Load("PreFabs/PreTile", typeof(GameObject))) as GameObject;
            go_Tile.name = "Tile = X,Z:  (" + x + "," + z + ")";
            go_Tile.transform.position = Location;
            setTileType(type);


            go_Tile.AddComponent<mono_Clock>();
            mnBorder = go_Tile.GetComponent<mono_Border>();

        }


        public void setTileType(TileType type)
        {
            //update visauls depenting on type 
            this.Type = type;
            setMaterial(type, go_Tile);
        }




        public GameObject getGoTile()
        {
            if (go_Tile != null)
            {
                return go_Tile;
            }
            else
            {
                Debug.Log("Null GameObjectTile.");
                return null;
            }

        }


        public int getUnitsInTile()
        {
            return units.Count;
        }



        //hook up the Materials with the game objectes()
        private void setMaterial(TileType type, GameObject go)
        {


            string matPath = "Materials/Terrain/";
            if (Tile.TileType.Grass == type)
            {
                go.GetComponent<Renderer>().material = Resources.Load(matPath + "MatGrass", typeof(Material)) as Material;
            }
            else if (Tile.TileType.Rock == type)
            {
                go.GetComponent<Renderer>().material = Resources.Load(matPath + "MatRock", typeof(Material)) as Material;
            }
            else if (Tile.TileType.Water == type)
            {
                go.GetComponent<Renderer>().material = Resources.Load(matPath + "MatWater", typeof(Material)) as Material;
            }

            else if (Tile.TileType.Empty == type)
            {

                go.GetComponent<Renderer>().material = Resources.Load(matPath + "MatEmpty", typeof(Material)) as Material;

            }

            else if (Tile.TileType.Castle == type)
            {

                //go.GetComponent<Renderer>().material = Resources.Load("Materials/MatGrass", typeof(Material)) as Material;
                //Debug.Log("Castelano must be its own Buiing on top of the tile, Good luvk... :-)");
            }
            else
            {
                Debug.Log("SPRITE WASN'T FOUND");
            }


        }


        public void spawnUnit()
        {

            //check if the tile is owned by a playable player and that it has not exceted the max number of units Allowd
            if (this.owner != mono_playerManager.p0 && units.Count < unitMax)
            {
                Unit soldier = new Unit(this, this.Owner);
                units.Add(soldier);
                //Debug.Log("Unit created and add to QUQU in tile x"+this.X+"//y"+this.Y);
            }

        }



        public Player Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
                //if the player is human or AI then start spawning units using clock to set the rate of spawn
                if (this.owner != mono_playerManager.p0)
                {

                    this.State = TileState.Claimed;
                    this.go_Tile.GetComponent<mono_Clock>().createClock(this.spawnRate, this);
                    this.go_Tile.GetComponent<mono_Clock>().startClock();
                    createBorders();
                    mnBorder.setColor(this.Owner);

                }
                

            }
        }



        void createBorders()
        {


            //-----------------------------------------------------------------------
            if (_tileNorth != null)
            {
                if (_tileNorth.Owner != this.owner)
                {
                    mnBorder.borderN.SetActive(true);
                }
                else if (_tileNorth.Owner == this.owner)
                {
                    mnBorder.borderN.SetActive(false);
                    _tileNorth.mnBorder.borderS.SetActive(false);
                }

            }
            else
            {
                mnBorder.borderN.SetActive(false);
            }
            //-----------------------------------------------------------------------
            if (_tileSouth != null)
            {
                if (_tileSouth.Owner != this.owner)
                {
                    mnBorder.borderS.SetActive(true);
                }
                else if (_tileSouth.Owner == this.owner)
                {
                    mnBorder.borderS.SetActive(false);
                    _tileSouth.mnBorder.borderN.SetActive(false);
                }

            }
            else
            {
                mnBorder.borderS.SetActive(false);

            }
            //-----------------------------------------------------------------------
            if (_tileEast != null)
            {
                if (_tileEast.Owner != this.owner)
                {
                    mnBorder.borderE.SetActive(true);
                }
                else if (_tileEast.Owner == this.owner)
                {
                    mnBorder.borderE.SetActive(false);
                    _tileEast.mnBorder.borderW.SetActive(false);
                }

            }
            else
            {
                mnBorder.borderE.SetActive(false);
            }
            //-----------------------------------------------------------------------
            if (_tileWest != null)
            {
                if (_tileWest.Owner != this.owner)
                {
                    mnBorder.borderW.SetActive(true);

                }
                else if (_tileWest.Owner == this.owner)
                {
                    mnBorder.borderW.SetActive(false);
                    _tileWest.mnBorder.borderE.SetActive(false);
                }

            }
            else
            {
                mnBorder.borderW.SetActive(false);
            }
            //-----------------------------------------------------------------------

        }



        public void setNeighbors(Tile Tnorth, Tile Tsouth, Tile Teast, Tile Twest)
        {

            this._tileNorth = Tnorth;
            this._tileSouth = Tsouth;
            this._tileEast = Teast;
            this._tileWest = Twest;

        }


        public TileMovementDirection MoveDirection
        {
            get => moveDirection;

            set
            {
                moveDirection = value;
            }
        }
        public TileState State
        {

            get => state;


            set
            {

                state = value;
                if (State == TileState.Besieged && owner != mono_playerManager.p0)
                {
                    this.MoveDirection = TileMovementDirection.Center;
                    Debug.Log("MoveDirection was set To Center");
                    
                }
            }

        }
        public TileType Type
        {
            get => type;
            set => type = value;

        }
        public Vector3Int Vec3Pos { get => vec3Pos; }

        public int X => x;

        public int Z => z;

        public float TileGraphicsHeight => tileGraphicsHeight;

        public void clearUnits()
        {
            units.Clear();
        }

        public void addUnits(Unit soldier)
        {
            units.Add(soldier);

        }

    }
}
