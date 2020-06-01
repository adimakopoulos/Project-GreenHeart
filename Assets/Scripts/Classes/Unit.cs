using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit {



    
    int attack;
    public int health;
    Player owner;
    GameObject go_Unit;
    Tile CurrentTile, TargetTile;
    float speed=1f;


    public Unit() { }


    public Unit(Tile tile, Player player) {

        this.attack = 25;
        this.health = 100;
        owner = player;
        CurrentTile = tile;
        TargetTile = tile;
        go_Unit = GameObject.Instantiate(Resources.Load("PreFabs/PreUnit", typeof(GameObject))) as GameObject;
        go_Unit.name = player.Name;


        //Spawn unit in a random posiotion inside a tile
        
        go_Unit.transform.position = new Vector3( tile.X, tile.TileGraphicsHeight,  tile.Z);
        go_Unit.transform.SetParent(tile.getGoTile().transform, true);

        go_Unit.AddComponent<mono_Unit>().setUnitData(this);
       



    }


    public void moveToTile (Tile Target){
        CurrentTile = Target;
        

    }
    public Tile getTargetTile() { return TargetTile; }
    public void setTargetTile(Tile target) { this.TargetTile = target; }

    public GameObject Go_Unit { get => go_Unit; }
    public Player Owner { get => owner; set => owner = value; }
    public float Speed { get => speed; set => speed = value; }

    public Tile getCurrentTile() {
        if (CurrentTile != null)
        {
            return CurrentTile;
        }
        else
        {
            Debug.Log("Unit Doenst know in witch Tile is curently in");
            return null;
        }
    }
    public void setCurrentTile(Tile tile) {
        this.CurrentTile = tile;
    }
    public Unit getUnitData() { return this; }

}
