using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mouse : MonoBehaviour , IPointerClickHandler{
    
    public GameObject txtOwner;
    public GameObject txtTerainType;
    public GameObject txtUnitsInTile;
    //Debugging Var. Just to keep track of mouse x and Y(ScreenToWorldPoint)
    [SerializeField] float x, y;
    //Only player 1 shoyuld have controll of the mouse
    private Player player1 = mono_PlayerManager.p1;




    public virtual void OnPointerClick(PointerEventData eventData)
    {

        Debug.Log("assss asss" + eventData.button);
        if (eventData.clickCount >= 2)
        {
            Debug.Log("double click");
        }
    }


	
	// Update is called once per frame
	void Update () {



        Vector3 currFramepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //camera mouvment by dragging
        dragCamera(currFramepos);
        //create Selection effect on the boarder of the tile that is clicked
        createSelectionVFX(currFramepos);
        //Update Text Labales and sutff
        updateUI(currFramepos);
        //Tell tile to tell units that the they need to move
        moveUnits(currFramepos);


        //Point (x,y) of mouse
        x = currFramepos.x;
        y = currFramepos.y;

    }//update end

    //firstTile= the first tile that is click
    Tile firstTile = null, _neighborTile = null;
    void moveUnits(Vector3 currFramepos) {
        
        //this is essentiallu a function that detects mouse draging behavior;
        if (Input.GetMouseButtonDown(0)) {
            //first tile clicked
            //Also we must check ownership
            //if (mono_BoardCreate.map.getTileFromMap(currFramepos).Owner == player1) {
                firstTile = mono_BoardCreate.map.getTileFromMap(currFramepos);
                //this thorws null Exception.
            //}
            

        }

        //If i am not the owner of the tile , the firstTile Var will remain Null and the logic will not trigger
        if (Input.GetMouseButtonUp(0) && firstTile != null) {

            //lastTile = GameObject.FindObjectOfType<mono_BoardCreate>().getBoard().getTileFromMap(Camera.main.ScreenToWorldPoint(Input.mousePosition)); 

            //Check when mouse is out of the Tile to get the direction the player is dragging. 
            //there are 4 possible outcomes. North-up, South-down, West-left, East-right.

            bool isCooltoMove;
            if (firstTile.X > Mathf.FloorToInt(currFramepos.x))
            {
                _neighborTile= mono_BoardCreate.map.getTileFromMap(firstTile.X-1, firstTile.Y); //get the neighboring tile that is x-1;
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Left);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Left; }

            }//left
            if (firstTile.X < Mathf.FloorToInt(currFramepos.x))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(firstTile.X+1, firstTile.Y);
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Right);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Right;}

            }//right
            if (firstTile.Y > Mathf.FloorToInt(currFramepos.y))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(firstTile.X, firstTile.Y-1);
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Down);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Down;}

            }//down
            if (firstTile.Y < Mathf.FloorToInt(currFramepos.y))
            {
                _neighborTile = mono_BoardCreate.map.getTileFromMap(firstTile.X, firstTile.Y+1);
                isCooltoMove = checkMoveValidity(firstTile, _neighborTile, Tile.TileMovementDirection.Up);
                if (isCooltoMove) { firstTile.MoveDirection = Tile.TileMovementDirection.Up; }
               


            }//up





            firstTile = null;  
        
        }

    }
    private bool checkMoveValidity(Tile _originalTile, Tile _nTile, Tile.TileMovementDirection intendedDir)
    {
        bool result= false;
        if (_originalTile == null || _nTile == null)
        {
            return false;
        }
        else
        {


            //I sould be able to invade an Enemy Tile
            if (_originalTile.Owner != _nTile.Owner)
            {
                return  true;
            }

            //I sould be able to set my direction to neighboing tiles that DOES NOT have a TileMovementDirection
            if (_nTile.MoveDirection == Tile.TileMovementDirection.Center)
            {
                return true;
            }



            //this switch checkes if tiles point at each other 
            switch (intendedDir)
            {
                case Tile.TileMovementDirection.Up: if (_nTile.MoveDirection != Tile.TileMovementDirection.Down) { result = true; } break;//i dont think i need to break here cause i am already returning
                case Tile.TileMovementDirection.Down: if (_nTile.MoveDirection != Tile.TileMovementDirection.Up) { result = true; } break;
                case Tile.TileMovementDirection.Left: if (_nTile.MoveDirection != Tile.TileMovementDirection.Right) { result = true; } break;
                case Tile.TileMovementDirection.Right: if (_nTile.MoveDirection != Tile.TileMovementDirection.Left) { result = true; } break;
            }



            return result;



        }

    }


    Vfx selected_last;
    void createSelectionVFX(Vector3 currFramepos) {
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
                    Destroy(selected_last.getGoVFX());
                    //Debug.Log("GO destroyed");
                }
                //create new selection vfx
                selected_last = new Vfx(tile.X, tile.Y, Vfx.tileVfx.Select);


            }

        }

    }//end of createSelectionVFX



    void updateUI(Vector3 currFramepos)
    {
        //Update UI 
        //Tile tile = GameObject.FindObjectOfType<mono_BoardCreate>().getBoard().getTileFromMap(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Tile tile = mono_BoardCreate.map.getTileFromMap(currFramepos);//a bit cleaner 
        if (Input.GetMouseButtonUp(0))                                                        
        {
            if (tile != null)
            {
                txtOwner.GetComponent<Text>().text = tile.Owner.Name;
                txtTerainType.GetComponent<Text>().text = tile.Type.ToString();
                txtUnitsInTile.GetComponent<Text>().text = "Units in Tile:"+tile.getUnitsInTile().ToString()+" State: "+tile.State.ToString();
            }
        }
    }



    Vector3 lastFramePos;
    Vector3 diff;
    void dragCamera(Vector3 currFramepos) {

        if (Input.GetMouseButton(1))
        {
            diff = lastFramePos - currFramepos;
            Camera.main.transform.Translate(diff);
        }
        lastFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }//end dragCamera();







}
