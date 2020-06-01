using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    //Enums Data driven states of the tile
    public enum TileType { Empty, Grass, Rock, Water, Castle };
    public enum TileState { Neutral, Claimed, Besieged };
    public enum TileMovementDirection { Up, Down, Left, Right, Center };
    private TileType type = TileType.Empty;
    private TileState state = TileState.Neutral;
    private TileMovementDirection moveDirection = TileMovementDirection.Center;

    //Game positions
    private readonly int x,z;
    private readonly float tileGraphicsHeight = 0.1f;//make it so units spawn on top of the tile  
    private Vector3Int gamePosition;//maybe this is a bit redutant?(because we already have gameObject.transfor witcht already holds the same information)

    //Gameplay Variablas
    private Building building;
    GameObject go_Tile;
    Player owner = mono_PlayerManager.p0; //neutral player as an owner
    float spawnRate = 2.0f;
    public int unitMax = 10;



    //after researching i found that Lists should be used here. doesnt matter if theoreticly this is a dyniamic collection. 
    //Also we have to take to considaration that leaving this uncaped might crash peoples hardware.
    // Creating a list with an initial size
    public List<Unit> units = new List<Unit>(1000);
    //public ArrayList units = new ArrayList(); 
    Vfx dirVfx;//depricated     //______________________TODO FIX VFX___________________________

    public float capturePoints = 0;









    public Tile(Vector3Int Location, TileType type = TileType.Grass, TileState state= TileState.Neutral) {
        this.x = Location.x;
        this.z = Location.z;
        this.State = state;
        
        //SET game object
        go_Tile = GameObject.Instantiate(Resources.Load("PreFabs/PreTile", typeof(GameObject))) as GameObject;
        go_Tile.name = "Tile = X,Z:  (" + x + "," + z+")";
        go_Tile.transform.position = Location;
        setTileType(type);


        go_Tile.AddComponent<mono_Clock>();


    }


    public void setTileType(TileType type) {
        //update data and visauls
        this.Type = type;
        setMaterial(type, go_Tile);
    }




    public GameObject getGoTile() {
        if (go_Tile != null) { return go_Tile;
        } else {
            Debug.Log("Null GameObjectTile.");
            return null;
        }

    }


    public int getUnitsInTile() {
        return units.Count;
    }



    //hook up the Materials with the game objectes()
    private void setMaterial(TileType type, GameObject go) {
        string matPath = "Materials/Terrain/";
        if (Tile.TileType.Grass == type)
        {            
            go.GetComponent<Renderer>().material = Resources.Load(matPath+"MatGrass", typeof(Material)) as Material;
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

            go.GetComponent<Renderer>().material  = Resources.Load(matPath + "MatEmpty", typeof(Material)) as Material;

        }

        else if (Tile.TileType.Castle == type)
        {

            //go.GetComponent<Renderer>().material = Resources.Load("Materials/MatGrass", typeof(Material)) as Material;
            Debug.Log("Castelano must be its own Buiing on top of the tile, Good luvk... :-)");
        }
        else {
            Debug.Log("SPRITE WASN'T FOUND");
        }





    }


    public void spawnUnit() {

        //check if the tile is owned by a playable player and that it has not exceted the max number of units Allowd
        if (this.owner != mono_PlayerManager.p0 && units.Count < unitMax)
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
            if (this.owner != mono_PlayerManager.p0) {

                this.State = TileState.Claimed;
                this.go_Tile.GetComponent<mono_Clock>().createClock(this.spawnRate, this);
                this.go_Tile.GetComponent<mono_Clock>().startClock();
                setVfxInitialSpr();
                
            }
        }
    }


    //______________________TODO FIX VFX___________________________
    public void setVfxInitialSpr() {

        if (this.dirVfx == null)
        {
            //dirVfx = new Vfx(this.GamePosition.x, this.GamePosition.y, Vfx.tileVfx.Center);//When  a new owner gets ownership of tile the direction of tile should be center
        }
        else
        {
            //dirVfx.setVfxSpr(Vfx.tileVfx.Center);//change the indication of the movement direction for units
        }
    }


    //______________________TODO FIX VFX___________________________
    public TileMovementDirection MoveDirection
    { 
        get => moveDirection;

        set {
            // since TileMovementDirection is a subset of Vfx.tileVfx (enums) we can safly cast it 
            moveDirection = value;
            //dirVfx.setVfxSpr((Vfx.tileVfx)moveDirection);
        } 
    }
    public TileState State {
        
        get => state;


         set {
            
            state = value;
            if (State == TileState.Besieged && owner != mono_PlayerManager.p0)
            {
                this.MoveDirection = TileMovementDirection.Center;
                Debug.Log("Huh");
            }
        }
    
    }
    public TileType Type {
        get => type;
        set => type = value;

    }
    public Vector3Int GamePosition { get => gamePosition; set => gamePosition = value; }

    public int X => x;

    public int Z => z;

    public float TileGraphicsHeight => tileGraphicsHeight;

    public void clearUnits (){
        units.Clear();
    }

    public void addUnits(Unit soldier)
    {
        units.Add(soldier);

    }

}
