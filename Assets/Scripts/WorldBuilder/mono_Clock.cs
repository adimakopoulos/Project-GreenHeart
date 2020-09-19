using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldBuilder;

public class mono_Clock : MonoBehaviour
{

    float setTime;//Mayby start using SirializedField
    float internalTime;
    bool isDone;
    Tile tile;
    public void createClock(float countDown, Tile tile) {
        this.isDone = false;
        this.setTime = countDown;
        this.tile = tile;
    }

    private void timeToSpawnUnit() {
        tile.SpawnUnit();       
    }



    public void startClock()
    {
        InvokeRepeating("timeToSpawnUnit", 3f,setTime);
    }
    public void stopClock()
    {
        CancelInvoke();
        isDone = true;
    }
    public bool IsDone() {
        return isDone;
    
    }





}
