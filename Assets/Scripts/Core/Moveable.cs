using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public delegate void EnteringNewTile();
    public class Moveable : MonoBehaviour
    {


        public Tile CurrentTile, TargetTile;

        /// <summary>
        /// How fast the units moves
        /// </summary>
        [SerializeField]
        float moveSpeed = 10f;


        public event EnteringNewTile UnitOutOfTileBounds;
        public void setLocation(Tile arg) { TargetTile = arg; CurrentTile = arg; }
        public void setSpeed(float vroumVroum) { moveSpeed = vroumVroum; }
        // Update is called once per frame
        void Update()
        {
            SetTargetTile();
            startMoving();
            idleMovement();

        }


        private float _idleMoveTimer = 4.0f;
        private Vector3 _randLocalPos = new Vector3(0f, 1f, 0f);
        /// <summary>
        /// Simulate random movement.
        /// </summary>
        private void idleMovement()
        {

            Tile.TileMovementDirection dir = CurrentTile.MoveDirection;
            //if tile has not set dirction to move the units , do a simple idle movements inside tile
            if (dir == Tile.TileMovementDirection.Center || coolDown > 0)
            {

                //every 4 seconds, move !
                if (_idleMoveTimer < 0)
                {

                    //Isolate between -0.5 and 0.5 witch is the center of the tile. The tile has 1 as diamiter  
                    _randLocalPos = new Vector3(Random.Range(-0.5f, 0.5f), 1, Random.Range(-0.5f, 0.5f));
                    _idleMoveTimer = Random.Range(1.0f, 5.0f);
                    gameObject.transform.LookAt(_randLocalPos, Vector3.up );
                }
                else
                {
                    _idleMoveTimer -= Time.deltaTime;
                    gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, _randLocalPos, 0.4f * Time.deltaTime);
                }
            }
        }



        public void SetTargetTile()
        {

            Tile.TileMovementDirection dir = CurrentTile.MoveDirection;
            if (dir != Tile.TileMovementDirection.Center)
            {
                //get the neighboring tiles 
                switch (dir)
                {
                    case Tile.TileMovementDirection.Up: TargetTile = CurrentTile._tileNorth; break;
                    case Tile.TileMovementDirection.Down: TargetTile = CurrentTile._tileSouth; break;
                    case Tile.TileMovementDirection.Left: TargetTile = CurrentTile._tileWest; break;
                    case Tile.TileMovementDirection.Right: TargetTile = CurrentTile._tileEast; break;
                }
            }
        }

        /// <summary>
        /// time before the unit decides the next move, this exists for exploit protections
        /// </summary>
        public float coolDown = 0.5f;
        public void startMoving()
        {


            if (CurrentTile != TargetTile && coolDown < 0f )
            {

                float targetX = 0;
                float targetZ = 0;
                switch (CurrentTile.MoveDirection)
                {
                    case Tile.TileMovementDirection.Up: targetZ += 1f; break;
                    case Tile.TileMovementDirection.Down: targetZ -= 1f; ; break;
                    case Tile.TileMovementDirection.Left: targetX -= 1f; break;
                    case Tile.TileMovementDirection.Right: targetX += 1; break;
                }

                Vector3 destination = new Vector3(targetX, 1, targetZ);
                //gameObject.transform.position = Vector3.Lerp(transform.position, destination, unitData.Speed * Time.deltaTime);
                gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, destination, 0.1f * moveSpeed * Time.deltaTime);
                //set new tile 
                hasArrived();



            }
            else
            {
                coolDown -= Time.deltaTime;
            }


        }

        private void hasArrived()
        {
            //check if local gameObject position is out of the bounds of tile. if yes set new tile
            if (gameObject.transform.localPosition.x < -0.5f || gameObject.transform.localPosition.z < -0.5f || gameObject.transform.localPosition.x > 0.5f || gameObject.transform.localPosition.z > 0.5f)
            {

                coolDown = 1f;//unit must wait half a second befor doing next move acction

 


                CurrentTile.MoveableGameobjects.Remove(this.gameObject);//remove unit from original tiles list
                CurrentTile.MoveableGameobjects.RepopulateEXT(); 
                CurrentTile = TargetTile;  //Set the new tile that the unit is currenty in
                gameObject.transform.SetParent(TargetTile.getGoTile().transform, true);
                //add units to new cyrrent tile
                CurrentTile.addUnits(this.gameObject);
                
                Tile tile = CurrentTile;
                //if tile in Neutral state Start Capturing !
                Player temp = this.gameObject.GetComponent<ITakeDamage>().getOwner();
                UnitOutOfTileBounds?.Invoke();
                if ( CurrentTile.Owner != temp && tile.getGoTile().GetComponent<mono_Battle>().enabled == false)//tile.State == Tile.TileState.Neutral ||
                {
                    Debug.Log("Time for battle");
                    tile.State = Tile.TileState.Besieged;
                    tile.getGoTile().GetComponent<mono_Battle>().enabled = true;




                }
            }
        }
    }
}