using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    //Data
    public enum TileType { Empty, Grass, Rock, Watter, Castle };
    public enum TileState { Neutral, Claimed, Besieged };
    public enum TileMovementDirection { Up, Down, Left, Right, Center };
    private TileType type = TileType.Empty;
    private TileState state = TileState.Neutral;
    private TileMovementDirection moveDirection = TileMovementDirection.Center;
    private int x;
    private int y;
    GameObject go_Tile;
    Player owner = mono_PlayerManager.p0; //neutral player as an owner
    float spawnRate = 2.0f;
    public int unitMax = 10;
    public ArrayList units = new ArrayList();
    Vfx dirVfx;
    public float capturePoints = 0;









    public Tile(int x, int y, TileType type, TileState state) {
        this.X = x;
        this.Y = y;
        this.State = state;
        go_Tile = new GameObject("Tile = X:" + x + " Y:" + y);
        go_Tile.transform.position = new Vector3(x, y, 0);
        go_Tile.AddComponent<SpriteRenderer>();
        setTileType(type);

        go_Tile.GetComponent<SpriteRenderer>().sortingLayerName = "Lr_Tilemap";
        go_Tile.GetComponent<SpriteRenderer>().sortingOrder = 0;
        go_Tile.AddComponent<mono_Clock>();


    }


    public void setTileType(TileType type) {
        this.Type = type;
        setSprite(type, go_Tile);
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



    //hook up the Sprites with the game objectes()
    private void setSprite(TileType type, GameObject go) {

        if (Tile.TileType.Grass == type)
        {
            go.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Tiles/spr_grass", typeof(Sprite)) as Sprite;
        }
        else if (Tile.TileType.Rock == type)
        {
            go.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Tiles/spr_rock", typeof(Sprite)) as Sprite;
        }
        else if (Tile.TileType.Watter == type)
        {
            go.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Tiles/spr_watter", typeof(Sprite)) as Sprite;
        }

        else if (Tile.TileType.Empty == type)
        {

            go.GetComponent<SpriteRenderer>().sprite = null;
        }

        else if (Tile.TileType.Castle == type)
        {

            go.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Tiles/spr_castle", typeof(Sprite)) as Sprite;
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

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
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

    public void setVfxInitialSpr() {

        if (this.dirVfx == null)
        {
            dirVfx = new Vfx(this.x, this.y, Vfx.tileVfx.Center);//When  a new owner gets ownership of tile the direction of tile should be center
        }
        else
        {
            dirVfx.setVfxSpr(Vfx.tileVfx.Center);//change the indication of the movement direction for units
        }
    }



    public TileMovementDirection MoveDirection
    { 
        get => moveDirection;

        set {
            // since TileMovementDirection is a subset of Vfx.tileVfx (enums) we can safly cast it 
            moveDirection = value;
            dirVfx.setVfxSpr((Vfx.tileVfx)moveDirection);
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

    public void clearUnits (){
        units.Clear();
    }

    public void addUnits(Unit soldier)
    {
        units.Add(soldier);

    }

}
