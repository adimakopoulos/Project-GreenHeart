using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mono_Unit : MonoBehaviour
{    
    public Sprite unitSpr;
    Unit unitData;

 
    public virtual void OnPointerClick(PointerEventData eventData)
    {

        Debug.Log("assss asss" + eventData.button);
        if (eventData.clickCount == 2)
        {
            Debug.Log("double click");
        }
    }


    // Update is called once per frame
    void Update()
    {
        //if the unit is dead Disable movement
        if (unitData != null && unitData.health > 0) { 
        //check if the tile has been ordered to move units
        moveOrder();
        startMoving();
        idleMovement();
        }
    }
     


    public void setUnitData(Unit data)
    {
        unitData = data;
        unitSpr = Resources.Load("SPRITES/spr_unit", typeof(Sprite)) as Sprite;

        //SetUp SpriteRenderer with sprite file and Layer 
        Tile tile = data.getCurrentTile();
        gameObject.AddComponent<SpriteRenderer>();
        gameObject.GetComponent<SpriteRenderer>().sprite = unitSpr;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "UnitsGraphics";
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        gameObject.GetComponent<Renderer>().material.color = tile.Owner.PlayerColor;//color the unit

        //Spawn unit in a random posiotion inside a tile
        gameObject.transform.SetParent(tile.getGoTile().transform, false);
        gameObject.transform.position = new Vector3(Random.Range(0f, 0.9f) + tile.X, Random.Range(0f, 0.9f) + tile.Y, 0);
  
        //

    }



    public void moveOrder()
    {

        if (unitData != null)
        {
            Tile.TileMovementDirection dir = unitData.getCurrentTile().MoveDirection;
            if (dir != Tile.TileMovementDirection.Center)
            {
                //set Target tile that unit must start moving 
                int x = unitData.getCurrentTile().X;
                int y = unitData.getCurrentTile().Y;
                //get the neighboring tiles 
                switch (dir)
                {
                    case Tile.TileMovementDirection.Up: unitData.setTargetTile(mono_BoardCreate.map.getTileFromMap(x, y + 1)); break;
                    case Tile.TileMovementDirection.Down: unitData.setTargetTile(mono_BoardCreate.map.getTileFromMap(x, y - 1)); break;
                    case Tile.TileMovementDirection.Left: unitData.setTargetTile(mono_BoardCreate.map.getTileFromMap(x - 1, y)); break;
                    case Tile.TileMovementDirection.Right: unitData.setTargetTile(mono_BoardCreate.map.getTileFromMap(x + 1, y)); break;
                }



            }
            else
            {
                return;
            }


        }

    }

    public void startMoving()
    {

        if (unitData != null)
        {
            if (unitData.getCurrentTile() != unitData.getTargetTile())
            {
                //+16pixe to move in the center of the target , not the 0,0 corner
                float targetX = unitData.getTargetTile().X+0.5f;
                float targetY = unitData.getTargetTile().Y+0.5f;
                Vector3 destination = new Vector3(targetX, targetY, 0);
                gameObject.transform.position = Vector3.Lerp(transform.position, destination, unitData.Speed * Time.deltaTime);

                //set new tile 
                hasArrived();



            }
        }

    }

    private void hasArrived()
    {
        //check if local gameObject position is out of the bounds of tile. if yes set new tile
        if (gameObject.transform.localPosition.x < 0f || gameObject.transform.localPosition.y < 0f || gameObject.transform.localPosition.x > 1f || gameObject.transform.localPosition.y > 1f)
        {
           
            unitData.getCurrentTile().clearUnits();             //remove units from original tiles list
            unitData.setCurrentTile(unitData.getTargetTile());  //Set the new tile that the unit is currenty in
            gameObject.transform.SetParent(unitData.getTargetTile().getGoTile().transform, true);
            //add units to new cyrrent tile
            unitData.getCurrentTile().addUnits(unitData);

            Tile tile = unitData.getCurrentTile();
            //if tile in Neutral state Start Capturing !
            if (tile.State == Tile.TileState.Neutral) {
                Debug.Log("Time for battle");
                tile.State = Tile.TileState.Besieged;
                //GameObject battle = new GameObject("Battle at = X:" + tile.X + " Y:" + tile.Y);
                tile.getGoTile().AddComponent<mono_Battle>();
                //new GameObject("BATTLE").AddComponent<mono_Battle>();



            }

        }
    }

    private void OnDestroy()
    {
        //create death animation
    }

    public void killUnit()
    {
        unitData.health = 0;
        unitSpr = Resources.Load("SPRITES/spr_Death", typeof(Sprite)) as Sprite;
       
        unitData.Go_Unit.AddComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/anim_Death_0");
        Destroy(unitData.Go_Unit,0.5f);
    }


    private void takeDamage(int baseAmount) {
        unitData.health -= baseAmount;
    }



    //reminder that this is called in Update()
    float idleMoveTimer = 4.0f;
    Vector3 randLocalPos;
    private void idleMovement()
    {
        if (unitData != null)
        {
            Tile.TileMovementDirection dir = unitData.getCurrentTile().MoveDirection;
            //if tile has not set dirction to move the units , do a simple idle movements inside tile
            if (dir == Tile.TileMovementDirection.Center) {
                //Vector3 randLocalPos=    new Vector3(Random.Range(0.1f, 0.9f)+unitData.getCurrentTile().X, Random.Range(0.1f, 0.9f)+unitData.getCurrentTile().Y , 0);
                //gameObject.transform.position =  Vector3.Lerp(transform.position, randLocalPos, 0.4f*Time.deltaTime);

                //every 4 seconds, move !
                if (idleMoveTimer < 0)
                {
                    //randLocalPos.Y should be lower than 1 so unit does not apear outside of the tiles boarders
                    randLocalPos = new Vector3(Random.Range(0f, 0.9f), Random.Range(0f, 0.9f), 0);
                    
                    idleMoveTimer = 4.0f;

                }
                else {
                    idleMoveTimer -= 1 * Time.deltaTime;
                    gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, randLocalPos, 0.4f * Time.deltaTime);
                }
            }

        }

    }
}