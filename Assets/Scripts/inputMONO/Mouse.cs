using UnityEngine;
using WorldBuilder;
/*
 this is where we handle most mouse interaction. i decided to have a script to handle most mouse inputs so 
 it is eassier to 
*/



public class Mouse : MonoBehaviour
{

    //UI variables
    public GameObject tilePanel;
    public GameObject Tselect;

    //Only player 1 shoyuld have controll of the mouse
    private Player player1 = mono_PlayerManager.p1;

    //where the player clicks in world position (after ray hit)
    Vector3 clickPoint;        

    Tile selectedTile;
    void Update()
    {
        //TODO create Selection effect on the boarder of the tile that is clicked
        setSelectedTile();

        //Tell tile to tell units that the they need to move, set tile direction
        setTileDir();



    }//update end



    void setSelectedTile()//i think This is redudant , maybe delete it?
    {
        if (Input.GetMouseButtonUp(0))
        {
            selectedTile = mono_BoardCreate.map.getTileFromMap(doRayCast());
            updateUI();                                                         //Update Text Labales and sutff
            //Debug.Log("selectedTile: "+selectedTile.X +","+ selectedTile.Z);
            activateTselection();

        }


        //desselect tile if the player does a right click
        if (Input.GetMouseButtonDown(1)) {
            
            selectedTile = null;
            Tselect.SetActive(false);

        }

    }
    void updateUI()
    {
        //tell panel to update the texts because the player pressed a tile
        if (selectedTile != null)
        {
            tilePanel.GetComponent<TilePanelCs>().updateLabels(selectedTile);
        }
    }

    void activateTselection() {
        if (selectedTile != null)
        {
            Tselect.SetActive(true);
            Tselect.transform.position = new Vector3 (selectedTile.X ,0,selectedTile.Z);
        }
    }


    //firstTile= the first tile that is click
    //this veriable also works a bit like a boolean for running this code.If it is not set then the code doesnt run
    Tile firstTile = null, _neighborTile = null;
    void setTileDir()
    {

        //first tile clicked
        //Also we must check ownership
        if (Input.GetMouseButtonDown(0))
        {

            clickPoint= doRayCast();
            //check if player hit outside of map and if he owns the tile
            if (mono_BoardCreate.map.getTileFromMap(clickPoint) != null && mono_BoardCreate.map.getTileFromMap(clickPoint).Owner == player1 )
            {
                firstTile = mono_BoardCreate.map.getTileFromMap(clickPoint);
                //Debug.Log(firstTile.getGoTile().transform);
            }
        }


        //If i am not the owner of the tile , the firstTile Var will remain Null and the logic will not trigger
        if (Input.GetMouseButtonUp(0) && firstTile != null)
        {

            clickPoint = doRayCast();  
            //Check when mouse is out of the Tile to get the direction the player is dragging. 
            //there are 4 possible outcomes. North-up, South-down, West-left, East-right.

            bool isCooltoMove;
            int ftx = firstTile.X;
            int ftz = firstTile.Z;
            if (ftx > Mathf.FloorToInt(clickPoint.x))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(ftx - 1, ftz); //get the neighboring tile that is x-1;
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Left);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Left;  }
                //Debug.Log("Left");
            }//left
            if (ftx < Mathf.FloorToInt(clickPoint.x))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(ftx + 1, ftz);
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Right);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Right;  }
                //Debug.Log("right");

            }//right
            if (ftz > Mathf.FloorToInt(clickPoint.z))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(ftx, ftz - 1);
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Down);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Down; }
                //Debug.Log("down");
            }//down
            if (ftz < Mathf.FloorToInt(clickPoint.z))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(ftx, ftz + 1);
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Up);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Up;  }
                //Debug.Log("up");
            }//up
            firstTile = null;
        }

    }

    //player can not set 2 tiles to point at each other
    private bool checkMoveValidity(Tile _originalTile, Tile _nTile, Tile.TileMovementDirection intendedDir)
    {
        bool result = false;
        if (_originalTile == null || _nTile == null)
        {
            return false;
        }
        else
        {
            //I sould be able to invade an Enemy Tile
            if (_originalTile.Owner != _nTile.Owner)
            {
                return true;
            }

            //I sould be able to set my direction to neighboing tiles that DOES NOT have a TileMovementDirection
            if (_nTile.MoveDirection == Tile.TileMovementDirection.Center)
            {
                return true;
            }



            //this switch checkes if tiles point at each other 
            switch (intendedDir)
            {
                case Tile.TileMovementDirection.Up: if (_nTile.MoveDirection != Tile.TileMovementDirection.Down) { result = true; } break;
                case Tile.TileMovementDirection.Down: if (_nTile.MoveDirection != Tile.TileMovementDirection.Up) { result = true; } break;
                case Tile.TileMovementDirection.Left: if (_nTile.MoveDirection != Tile.TileMovementDirection.Right) { result = true; } break;
                case Tile.TileMovementDirection.Right: if (_nTile.MoveDirection != Tile.TileMovementDirection.Left) { result = true; } break;
            }



            return result;



        }

    }


    Vector3 doRayCast() {
        //a Y value of -10 means something went Wrong
        Vector3 hit= new Vector3(0f,-10f,0f) ;

        Plane plane = new Plane(Vector3.up, Vector3.zero);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            hit = ray.GetPoint(distance);
        }
        return hit;
    }
    



}
