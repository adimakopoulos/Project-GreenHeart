using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player  {
    //to do use id to check instread of names
    string name;
    int id;  
    Color playerColor;


    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public Color PlayerColor { get => playerColor; set => playerColor = value; }

    public Player(string name, Color color)
    {
        this.Name = name;
        this.playerColor = color;
    }

    


}
