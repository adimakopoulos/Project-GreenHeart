using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{


    //UI variables
    public GameObject txtOwner;
    public GameObject txtTerainType;
    public GameObject txtUnitsInTile;
    //Debugging Var. Just to keep track of mouse x and Y(ScreenToWorldPoint)
    [SerializeField] float x, y;
    //Only player 1 shoyuld have controll of the mouse
    private Player player1 = mono_PlayerManager.p1;


    Tile firstTile = null, _neighborTile = null;

    public Camera cam;
    Vector3 currFramepos;
    Vector3 hit;
    void Update()
    {


        //check if cursor has clicked a tile
        tileIsClicked();


        currFramepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //create Selection effect on the boarder of the tile that is clicked
        createSelectionVFX(currFramepos);
        //Update Text Labales and sutff
        updateUI(currFramepos);
        //Tell tile to tell units that the they need to move
        moveUnits(currFramepos);


        //Point (x,y) of mouse
        x = currFramepos.x;
        y = currFramepos.z;

    }//update end




    void tileIsClicked()
    {
        if (Input.GetMouseButtonUp(0))
        {



            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float point;
            if (plane.Raycast(ray, out point))
            {
                hit = ray.GetPoint(point);
            }
            

        }
        firstTile = mono_BoardCreate.map.getTileFromMap(hit);
        Debug.Log(firstTile.X + firstTile.Z);
    }

    //firstTile= the first tile that is click

    void moveUnits(Vector3 currFramepos)
    {

        //this is essentiallu a function that detects mouse draging behavior;
        if (Input.GetMouseButtonDown(0))
        {
            //first tile clicked
            //Also we must check ownership
            //if (mono_BoardCreate.map.getTileFromMap(currFramepos).Owner == player1) {
            firstTile = mono_BoardCreate.map.getTileFromMap(currFramepos);
            //this thorws null Exception.
            //}


        }

        //If i am not the owner of the tile , the firstTile Var will remain Null and the logic will not trigger
        if (Input.GetMouseButtonUp(0) && firstTile != null)
        {

            //lastTile = GameObject.FindObjectOfType<mono_BoardCreate>().getBoard().getTileFromMap(Camera.main.ScreenToWorldPoint(Input.mousePosition)); 

            //Check when mouse is out of the Tile to get the direction the player is dragging. 
            //there are 4 possible outcomes. North-up, South-down, West-left, East-right.

            bool isCooltoMove;
            int ftx = firstTile.GamePosition.x;
            int ftz = firstTile.GamePosition.z;
            if (ftx > Mathf.FloorToInt(currFramepos.x))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(ftx - 1, ftz); //get the neighboring tile that is x-1;
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Left);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Left; }

            }//left
            if (ftx < Mathf.FloorToInt(currFramepos.x))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(ftx + 1, ftz);
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Right);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Right; }

            }//right
            if (firstTile.GamePosition.y > Mathf.FloorToInt(currFramepos.y))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(ftx, ftz - 1);
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Down);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Down; }

            }//down
            if (firstTile.GamePosition.y < Mathf.FloorToInt(currFramepos.y))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(ftx, ftz + 1);
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Up);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Up; }



            }//up





            firstTile = null;

        }

    }
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



    //______________________TODO FIX VFX___________________________
    Vfx selected_last;
    void createSelectionVFX(Vector3 currFramepos)
    {
        //click on map , create effect on selected Tile(on mouseButtonUp)
        if (Input.GetMouseButtonUp(0))
        {
            //this is a bit redundant 
            //Tile tile = GameObject.FindObjectOfType<mono_BoardCreate>().getBoard().getTileFromMap(Camera.main.ScreenToWorldPoint(Input.mousePosition));         
            Tile tile = mono_BoardCreate.map.getTileFromMap(currFramepos);

            if (tile != null)
            {
                //destroy selection vfx at last location(because you can only select 1)
                if (selected_last != null)
                {
                    //Destroy(selected_last.getGoVFX());    //______________________TODO FIX VFX___________________________
                    //Debug.Log("GO destroyed");
                }
                //create new selection vfx
                //selected_last = new Vfx(tile.GamePosition.x, tile.GamePosition.y, Vfx.tileVfx.Select);            //______________________TODO FIX VFX___________________________


            }

        }

    }//end of createSelectionVFX



    void updateUI(Vector3 currFramepos)
    {
        //Update UI Camera.main.ScreenPointToRay(Input.mousePosition);
        //Tile tile = GameObject.FindObjectOfType<mono_BoardCreate>().getBoard().getTileFromMap(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Tile tile = mono_BoardCreate.map.getTileFromMap(currFramepos);//a bit cleaner 
        if (Input.GetMouseButtonUp(0))
        {
            if (tile != null)
            {
                txtOwner.GetComponent<Text>().text = tile.Owner.Name;
                txtTerainType.GetComponent<Text>().text = tile.Type.ToString();
                txtUnitsInTile.GetComponent<Text>().text = "Units in Tile:" + tile.getUnitsInTile().ToString() + " State: " + tile.State.ToString();
            }
        }
    }











}
