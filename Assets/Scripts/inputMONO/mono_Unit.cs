using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WorldBuilder;

public class mono_Unit : CombatChar , IPointerClickHandler
{    
 
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
        else if (unitData != null && unitData.health <= 0){
        
            killUnit();
        }
    }
     


    public void setUnitData(Unit data)
    {
        unitData = data;
        

        //SetUp SpriteRenderer with sprite file and Layer 
        Tile tile = data.getCurrentTile();

        gameObject.GetComponent<Renderer>().material.color = tile.Owner.PlayerColor;//color the unit
    

    }



    public void moveOrder()
    {

        if (unitData != null)
        {
            Tile.TileMovementDirection dir = unitData.getCurrentTile().MoveDirection;
            if (dir != Tile.TileMovementDirection.Center)
            {
                //set Target tile that unit must start moving 
                int x = unitData.getCurrentTile().Vec3Pos.x;
                int z = unitData.getCurrentTile().Vec3Pos.z;
                //get the neighboring tiles 
                switch (dir)
                {
                    case Tile.TileMovementDirection.Up: unitData.setTargetTile(mono_BoardCreate.map.getTileFromMap(x, z + 1)); break;
                    case Tile.TileMovementDirection.Down: unitData.setTargetTile(mono_BoardCreate.map.getTileFromMap(x, z - 1)); break;
                    case Tile.TileMovementDirection.Left: unitData.setTargetTile(mono_BoardCreate.map.getTileFromMap(x - 1, z)); break;
                    case Tile.TileMovementDirection.Right: unitData.setTargetTile(mono_BoardCreate.map.getTileFromMap(x + 1, z)); break;
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
                float targetX = unitData.getTargetTile().Vec3Pos.x +0.5f;
                float targetZ = unitData.getTargetTile().Vec3Pos.z +0.5f;
                Vector3 destination = new Vector3(targetX, 0, targetZ);
                //gameObject.transform.position = Vector3.Lerp(transform.position, destination, unitData.Speed * Time.deltaTime);
                gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, destination, 0.4f * Time.deltaTime);
                //set new tile 
                hasArrived();



            }
        }

    }

    private void hasArrived()
    {
        //check if local gameObject position is out of the bounds of tile. if yes set new tile
        if (gameObject.transform.localPosition.x < -0.5f || gameObject.transform.localPosition.z < -0.0f || gameObject.transform.localPosition.x > 0.5f || gameObject.transform.localPosition.z > 0.5f)
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
        //create death animation or particles
    }

    public void killUnit()
    {
        unitData.health = 0;
        //creat particles and die LUL
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
                    
                    //Isolate between -0.5 and 0.5 witch is the center of the tile. The tile has 1 as diamiter  
                    randLocalPos = new Vector3(Random.Range(-0.5f, 0.5f), 1 , Random.Range(-0.5f, 0.5f));
                    
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