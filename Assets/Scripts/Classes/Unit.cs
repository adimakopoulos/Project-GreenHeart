using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit {



    int x, y;
    int attack;
    public int health;
    Player owner;
    GameObject go_Unit;
    Tile CurrentTile, TargetTile;
    float speed=1f;


    public Unit() { }


    public Unit(Tile tile, Player player) {
        this.X = tile.X;
        this.Y = tile.Y;
        this.attack = 25;
        this.health = 100;
        owner = player;
        CurrentTile = tile;
        TargetTile = tile;
        go_Unit = new GameObject("Unit at x= " + x + " y=+" + y + "-Player:" + player.Name);
        go_Unit.AddComponent<mono_Unit>().setUnitData(this);
       



    }


    public void moveToTile (Tile Target){
        CurrentTile = Target;
        

    }
    public Tile getTargetTile() { return TargetTile; }
    public void setTargetTile(Tile target) { this.TargetTile = target; }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
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
