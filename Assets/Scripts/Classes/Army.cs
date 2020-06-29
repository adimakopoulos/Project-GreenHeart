using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WorldBuilder;

public class Army 
{

    public List <BaseValues> FightingUnits;

    float _totalDamage;

    public Army(GameObject[] units) {
        foreach (var unit in units)
        {
            //FightingUnits.Add( unit.GetComponent<ITakeDamage>().giveMeyourStats());
        }


    }


}
