
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
        private const float  tileGraphicsHeight = 0.2f;//make it so units spawn on top of the tile  

        //Gameplay Variablas
        private Building building;
        GameObject go_Tile;
        Player owner =  mono_playerManager.p0; //neutral player as an owner
        private float _spawnRate = 5.0f;
        public int unitMax = 1;



        //after researching i found that Lists should be used here. doesnt matter if theoreticly this is a dyniamic collection. 
        //Also we have to take to considaration that leaving this uncaped might crash peoples hardware.
        // Creating a list with an initial size
        public List<GameObject> MoveableGameobjects = new List<GameObject>(50);
        //public ArrayList units = new ArrayList(); 
        public float capturePoints = 0;

        //neighboring tiles
        public Tile _tileNorth, _tileSouth, _tileEast, _tileWest;
        mono_BorderManager mnBorder;







        public Tile(Vector3Int Location, TileType type = TileType.Grass, TileState state = TileState.Neutral)
        {
            this.x = Location.x;
            this.z = Location.z;
            this.State = state;
            //SET game object
            go_Tile = GameObject.Instantiate(Resources.Load("PreFabs/PreTile", typeof(GameObject))) as GameObject;
            go_Tile.name = "Tile = X,Z:  (" + x + "," + z + ")";
            go_Tile.transform.position = Location;
            go_Tile.AddComponent<mono_Clock>();

            setTileType(type);

            

            mnBorder = go_Tile.GetComponent<mono_BorderManager>();

        }


        public void setTileType(TileType type)
        {
            //update visauls depenting on type 
            this.Type = type;
            setMaterial(type, go_Tile);
        }



        /// <summary>
        /// Returns the game object tile
        /// </summary>
        /// <returns>GameObject</returns>
        public GameObject getGoTile()
        {
            if (go_Tile != null)
            {
                return go_Tile;
            }
            else
            {
                Debug.Log("Null GameObject Tile.");
                return null;
            }

        }


        public int getUnitsInTile()
        {
            return MoveableGameobjects.Count;
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
            if (MoveableGameobjects.Count < unitMax)
            {


                
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHoplite(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHoplite(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHoplite(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHoplite(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHoplite(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHoplite(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHypaspist(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHypaspist(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHypaspist(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHypaspist(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHypaspist(this, this.Owner));
                MoveableGameobjects.Add(UnitSpawnFactory.SpawnHypaspist(this, this.Owner));


                //MoveableGameobjects.TrimExcess();
                //MoveableGameobjects.Add(UnitSpawnFactory.SpawnHypaspist(this, this.Owner));

                //Debug.Log(this.MoveableGameobjects[0].GetComponent<ITakeDamage>().getOwner().ToString() + " " + this.MoveableGameobjects[0].name);
                //ITakeDamage tem = this.MoveableGameobjects[1].GetComponent(typeof(ITakeDamage)) as ITakeDamage;
                //Debug.Log(tem.getOwner().ToString() + " -----" + this.MoveableGameobjects[1].name + "           " + this.MoveableGameobjects[1].name);
                //Debug.Log(this.MoveableGameobjects[0].GetComponent<ITakeDamage>().getOwner().ToString());
                //Debug.Log(this.MoveableGameobjects[1].GetComponent<ITakeDamage>().getOwner().ToString()); //gameObject.GetComponent(typeof(HingeJoint)) as HingeJoint;
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
                    this.capturePoints = 0;
                    this.go_Tile.GetComponent<mono_Clock>().createClock(this._spawnRate, this);
                    this.go_Tile.GetComponent<mono_Clock>().startClock();
                    createBorders();
                    mnBorder.setColor(this.Owner);

                }
                

            }
        }


        /// <summary>
        /// this logic must be run when the tilemap has finished spawning
        /// </summary>
        void createBorders()
        {

            //dissable the borders when 2 tiles that "touch" each other have the same Owner
            //Else the tile will activate its borders. This check happens 4 times for north, south, east and west of the tile
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
                if (State == TileState.Besieged) {
                    

                    //gameObject.GetComponent(typeof(HingeJoint)) as HingeJoint;
                    
                }
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

        public int X => x;

        public int Z => z;

        public float TileGraphicsHeight => tileGraphicsHeight;

        public void clearUnits()
        {
            MoveableGameobjects.Clear();
        }

        public void addUnits(GameObject soldier)
        {
            MoveableGameobjects.Add(soldier);

        }

    }
}
