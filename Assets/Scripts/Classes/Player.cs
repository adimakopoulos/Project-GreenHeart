using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player  {
    //to do use id to check instread of names
    private  string playerName;
    private  int id;
    public  Color playerColor;


    public Player(string playerName,  Color playerColor)
    {
        this.playerName = playerName;
        this.playerColor = playerColor;
    }

    public override string  ToString() { return "Player " + this.playerName + " " + this.playerColor; }
    public string Name { get => playerName; set => playerName = value; }
    public Color PlayerColor { get => playerColor; set => playerColor = value; }
}
