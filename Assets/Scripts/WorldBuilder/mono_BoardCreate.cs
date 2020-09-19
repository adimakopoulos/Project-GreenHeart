
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldBuilder;

public class mono_BoardCreate : MonoBehaviour
{

    public  static Board map;
    string mapName;
    //Create map from Editor
    public int width, height;

    public bool _killExtraUnits = true;
    public float KillRate = 0.25f;


    // Use this for initialization
    void Start()
    {

        map = new Board(width, height);
        startGame();


    }

    void startGame()
    {

        map.enableTiles();
        map.createShadingEffect();
        if (_killExtraUnits) { InvokeRepeating("killExtraUnits", 0, KillRate); }



        //hard coded stuf for testing
        map.getTileFromMap(0, 0).Owner = mono_playerManager.p1;
        map.getTileFromMap(9, 9).Owner = mono_playerManager.p2;
        //map.getTileFromMap(3,3).Owner = mono_playerManager.p3;

        //map.getTileFromMap(width - 2, height - 2).Owner = mono_playerManager.p2;
        map.getTileFromMap(Random.Range(2,6), Random.Range(4, 9)).setTileType(Tile.TileType.Water);
        map.getTileFromMap(Random.Range(2, 6), Random.Range(4, 9)).setTileType(Tile.TileType.Water);
        map.getTileFromMap(Random.Range(2, 6), Random.Range(4, 9)).setTileType(Tile.TileType.Water);
        map.getTileFromMap(Random.Range(2, 6), Random.Range(4, 9)).setTileType(Tile.TileType.Water);
        map.getTileFromMap(Random.Range(2, 6), Random.Range(4, 9)).setTileType(Tile.TileType.Rock);
        map.getTileFromMap(Random.Range(0, 0), Random.Range(4, 9)).setTileType(Tile.TileType.Water);
        //map.getTileFromMap(width - 1, height - 1).Owner = mono_playerManager.p3;
        //map.getTileFromMap(0, height - 1).Owner = mono_playerManager.p4;

        //GameObject waterCube = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //waterCube.GetComponent<Renderer>().material = Resources.Load("Materials/Terrain/WaterMat", typeof(Material)) as Material;
    }


    //Game Rule (mechanic) A tile can only support its max Units. After that, the Current(ally) units on the tile will Start to attrition lossing 1 unit per seccond
    //this has 0 references 
    int currentUnits, maxUnits;
    public void killExtraUnits()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                Tile encumberedTile = map.getTileFromMap(x, y);
                currentUnits = encumberedTile.MoveableGameobjects.Count;
                maxUnits = encumberedTile.unitMax;
                if (currentUnits > maxUnits && encumberedTile.State != Tile.TileState.Besieged)
                {
                    int randIndex = Random.Range(0, encumberedTile.MoveableGameobjects.Count - 1);
                    ITakeDamage temp = encumberedTile.MoveableGameobjects[randIndex].GetComponent<ITakeDamage>() as ITakeDamage; //get last unit

                    Debug.Log(temp + " " + encumberedTile);
                    if (temp != null)
                    {
                        encumberedTile.MoveableGameobjects.RemoveAt(randIndex);
                        temp.iTakeDamage(666, DamageType.trueDamage);               //Destroy unit game object
                    }
                    //Debug.Log("kill unit" + map.getTileFromMap(x, y));

                }

            }
        }
    }


    public Tile getBoardTile(int x, int y)
    {
        return map.getTileFromMap(x, y);
    }

    public Board getBoard()
    {
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

}
