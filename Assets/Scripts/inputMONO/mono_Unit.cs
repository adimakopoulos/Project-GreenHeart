using UnityEngine;
using UnityEngine.EventSystems;
using WorldBuilder;

public class mono_Unit : CombatChar, IPointerClickHandler
{

    Unit unitData;
    float idleMoveTimer = 4.0f;
    private Vector3 _randLocalPos;
    public ParticleSystem voxelParticles;


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
        else if (unitData != null && unitData.health <= 0) {

            killUnit();
        }
    }



    public void setUnitData(Unit data)
    {
        unitData = data;


        //SetUp SpriteRenderer with sprite file and Layer 
        Tile tile = data.getCurrentTile();

        gameObject.GetComponent<Renderer>().material.color = tile.Owner.PlayerColor;//color the unit
        _randLocalPos = new Vector3(Random.Range(-0.5f, 0.5f), 1, Random.Range(-0.5f, 0.5f));//give it a random position to start lerping Twards

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


    public float coolDown= 0.5f;
    public void startMoving()
    {

        if (unitData != null)
        {
            if (unitData.getCurrentTile() != unitData.getTargetTile() && coolDown < 0f)
            {

                float targetX = 0;
                float targetZ = 0;
                switch (unitData.getCurrentTile().MoveDirection)
                {
                    case Tile.TileMovementDirection.Up: targetZ += 1f; break;
                    case Tile.TileMovementDirection.Down: targetZ -= 1f; ; break;
                    case Tile.TileMovementDirection.Left: targetX -= 1f; break;
                    case Tile.TileMovementDirection.Right: targetX += 1; break;
                }

                Vector3 destination = new Vector3(targetX, 1, targetZ);
                //gameObject.transform.position = Vector3.Lerp(transform.position, destination, unitData.Speed * Time.deltaTime);
                gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, destination, 0.4f * Time.deltaTime);
                //set new tile 
                hasArrived();
                


            }
            else { coolDown -= Time.deltaTime; }
        }

    }

    private void hasArrived()
    {
        //check if local gameObject position is out of the bounds of tile. if yes set new tile
        if (gameObject.transform.localPosition.x < -0.5f || gameObject.transform.localPosition.z < -0.5f || gameObject.transform.localPosition.x > 0.5f || gameObject.transform.localPosition.z > 0.5f)
        {
            coolDown = 1f;//unit must wait half a second befor doing next move acction

            unitData.getCurrentTile().units.Remove(unitData);             //remove unit from original tiles list
            unitData.setCurrentTile(unitData.getTargetTile());  //Set the new tile that the unit is currenty in
            gameObject.transform.SetParent(unitData.getTargetTile().getGoTile().transform, true);
            //add units to new cyrrent tile
            unitData.getCurrentTile().addUnits(unitData);

            Tile tile = unitData.getCurrentTile();
            //if tile in Neutral state Start Capturing !
            if (tile.State == Tile.TileState.Neutral) {
                Debug.Log("Time for battle");
                tile.State = Tile.TileState.Besieged;
                tile.getGoTile().AddComponent<mono_Battle>();
               



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
        voxelParticles = Instantiate(Resources.Load("PreFabs/ParticlSysyemVoxels", typeof(ParticleSystem))) as ParticleSystem;
        voxelParticles.transform.SetParent(gameObject.transform, false);
        //creat particles and die LUL
        // LeanTween.moveLocalX(right_border, 0.95f, 1f).setEaseOutCubic().setLoopPingPong();
        //LeanTween.scaleX(unitData.Go_Unit, 0.15f, 1f);
        //LeanTween.scaleZ(unitData.Go_Unit, 0.15f, 1f);
        //LeanTween.moveLocalY(unitData.Go_Unit, 0, 1.4f);

        Vector3 temp = -gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, temp, 4f);

        Destroy(unitData.Go_Unit, 1.5f);
        
    }


    private void takeDamage(int baseAmount) {
        unitData.health -= baseAmount;
    }



    //reminder that this is called in Update()

    private void idleMovement()
    {
        if (unitData != null)
        {
            Tile.TileMovementDirection dir = unitData.getCurrentTile().MoveDirection;
            //if tile has not set dirction to move the units , do a simple idle movements inside tile
            if (dir == Tile.TileMovementDirection.Center || coolDown>0) {

                //every 4 seconds, move !
                if (idleMoveTimer < 0)
                {
                    
                    //Isolate between -0.5 and 0.5 witch is the center of the tile. The tile has 1 as diamiter  
                    _randLocalPos = new Vector3(Random.Range(-0.5f, 0.5f), 1 , Random.Range(-0.5f, 0.5f));
                    
                    idleMoveTimer = 4.0f;

                }
                else {
                    idleMoveTimer -= 1 * Time.deltaTime;
                    gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, _randLocalPos, 0.4f * Time.deltaTime);
                }
            }

        }

    }
}