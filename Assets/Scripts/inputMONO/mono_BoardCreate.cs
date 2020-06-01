using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class mono_BoardCreate : MonoBehaviour {

    public static Board map;
    string mapName;
    //Create map from Editor
    public int width, height;


    // Use this for initialization
    void Start()
    {
        
        map = new Board(width, height);
        InvokeRepeating("killExtraUnits",0,0.25f);


        //hard coded stuf for testing 
        map.getTileFromMap(0,0).Owner = mono_PlayerManager.p1;
        map.getTileFromMap(0+1, 0+1).Owner = mono_PlayerManager.p1;
        map.getTileFromMap(0 + 3, 0 + 1).Owner = mono_PlayerManager.p2;
        map.getTileFromMap(width - 2, height - 2).Owner = mono_PlayerManager.p2;
        map.getTileFromMap(0, 0).setTileType(Tile.TileType.Empty);

        map.getTileFromMap(width-1, height-1).Owner = mono_PlayerManager.p2;
        map.getTileFromMap(0, height - 1).Owner = mono_PlayerManager.p3;
    }


    //Game Rule (mechanic) A tile can only support its max Units. After that, the Current(ally) units on the tile will Start to attrition lossing 1 unit per seccond
    int currentUnits, maxUnits;
    public void killExtraUnits()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                currentUnits = map.getTileFromMap(x, y).units.Count;
                maxUnits = map.getTileFromMap(x, y).unitMax;
                if (currentUnits > maxUnits)
                {
                    Unit temp =(Unit) map.getTileFromMap(x, y).units[currentUnits - 1]; //get last unit
                    map.getTileFromMap(x, y).units.RemoveAt(currentUnits - 1);          //remove it from tiles list
                    temp.Go_Unit.GetComponent<mono_Unit>().killUnit();                  //Destroy unit game object
                   
                    //Debug.Log("kill unit" + map.getTileFromMap(x, y));

                }

            }
        }
    }


    public Tile getBoardTile(int x,int y) {
        return map.getTileFromMap(x,y);
    }

    public Board getBoard() {
        return map;
    }
    public void saveMap()
    {
        Debug.Log("Saving map...");
        map.saveBoard();
    }
    //public void loadMap()
    //{
    //    Debug.Log("Loading map...");
    //    map.loadBoard();
    //}

    public void setName(string arg)
    {       
        map.MapName = arg;
    }

    // Update is called once per frame
    void Update()
    {





    }
}
