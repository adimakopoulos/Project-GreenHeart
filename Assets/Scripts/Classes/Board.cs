using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public  class Board  {

   
    Tile[,] tiles;
    int width=0;
    int height=0;
    string mapName="" ;//gets init from a triger in the textfield savefilename



    public Board(int width, int height) {
        this.Width = width;
        this.Height = height;


        tiles = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                tiles[x, z] = new Tile(new Vector3Int(x,0,z), Tile.TileType.Grass, Tile.TileState.Neutral) ;                
                tiles[x, z].getGoTile().transform.SetParent(GameObject.Find("go_Board").transform, true);//make all the tiles a "Parent of go_Bord in the unity Hierarchy"
                
            }
        }

        tiles[0, 0].setTileType(Tile.TileType.Castle);
        tiles[Width-1, Height-1].setTileType(Tile.TileType.Castle);
        //Debug.Log("World created with Width: " + this.width + ",Height " + this.height);
    }

    public int Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public string MapName
    {
        get
        {
            return mapName;
        }

        set
        {
            mapName = value;
        }
    }

    float worldOffset = 0.5f;// because the tiles center is offset by 0.5
    public Tile getTileFromMap(Vector3 clickHit) {


        int x = Mathf.FloorToInt(clickHit.x+ worldOffset);
        int z = Mathf.FloorToInt(clickHit.z+ worldOffset);
        //Check to see if the player clicks outside of the map.
        if ( x > Width -1 || x < 0 || z >Height -1 || z <0)
        {
            //Debug.Log("No tile here. Out of bounds: " + x + ", " + y+ "Board Width: " + this.Width + ",Height " + this.Height);
           
            return null;
        }

        //Debug.Log("you clicked" + x+","+ y);
        return tiles[x, z];


    }

    public Tile getTileFromMap(int x, int y)
    {

        //Check to see if the player clicks outside of the map.
        if (x > Width - 1 || x < 0 || y > Height - 1 || y < 0)
        {
            //Debug.Log("No tile here. Out of bounds: " + x + ", " + y+ "Board Width: " + this.Width + ",Height " + this.Height);

            return null;
        }

        //Debug.Log("you clicked" + x+","+ y);
        return tiles[x, y];


    }




    //public void loadBoard() {
    //    string content = "";
    //    string[] contentArray;
        
    //    string path = Application.dataPath + "/Maps/"+mapName.Trim()+".txt";
        
    //    if (File.Exists(path))
    //    {
    //        content = File.ReadAllText(path).Replace(" ","");
    //        contentArray = content.Split(',');
    //        Debug.Log("Loeaded DATA");
            
    //    }
    //    else {
    //        Debug.Log("map not found.");
    //        return;
    //        //If we dont have a map there is no reason to exec ther rest of the routune
    //    }

    //    //clear current map
    //    if (tiles != null)
    //    {
    //        clearBoard();
    //    }
    //    else {
    //        Debug.Log("No map found to clear");
    //    }



       
    //    //generate tiles
    //    Width = int.Parse(contentArray[0].Replace("width=", ""));
    //    Height = int.Parse(contentArray[1].Replace("height=", ""));   
    //    tiles = new Tile[Width, Height];
    //    //the first 2 positions are reserved for the Width and Height.
    //    int i = 2;  
        
    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height; y++)
    //        {

    //            tiles[x, y] = new Tile(x, y, (Tile.TileType)System.Enum.Parse(typeof(Tile.TileType), contentArray[i++]),Tile.TileState.Neutral );//Better Storage needed. Tilestate. not stored
    //            tiles[x, y].getGoTile().transform.SetParent(GameObject.Find("go_Board").transform, true);//make all the tiles a "Parent of go_Bord in the unity Hierarchy"
    //        }
    //    }





    //}//load Board End


    
    public  void saveBoard() {
        //path of the file
        //make sure its not empty or has only spaces.
        string path;
        if (!string.IsNullOrEmpty(mapName) && mapName.Trim().Length >0 )
        {
            path = Application.dataPath + "/Maps/" + mapName.Trim() + ".txt";
            
        }
        else {
            Debug.Log("No name given to SaveFile!");
            return;
            //there is no point in exec this faction without a name, so we return;
        }
        


        //content of the file
        string content = "width=" + width + ",height=" + height+ ",\n";
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                content += tiles[x, y].Type.ToString() + ",\t";
                
            }

            content +=  "\n";
        }

        File.WriteAllText(path, content);
        Debug.Log("File created/Updated");



    }

    public void clearBoard() {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {                
                Object.Destroy(tiles[x, y].getGoTile());

            }
        }
        Width = 0;
        Height = 0;

    }



}//end of Board
