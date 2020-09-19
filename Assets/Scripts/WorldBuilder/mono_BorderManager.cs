using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mono_BorderManager : MonoBehaviour 
{

    public GameObject borderN, borderS, borderE, borderW;
    private Renderer[] borderRenderes;

    //awake is callled First!
    private void Awake()
    {
        borderRenderes = new Renderer[4];
        borderRenderes[0] = borderN.GetComponent<Renderer>();
        borderRenderes[1] = borderS.GetComponent<Renderer>();
        borderRenderes[2] = borderE.GetComponent<Renderer>();
        borderRenderes[3] = borderW.GetComponent<Renderer>();
    }





    public void setColor(Player owner)
    {

        foreach (var r in borderRenderes)
        {
             
           // r.material.color =  pl.PlayerColor;  outdate? TODO DELETE
            r.material.SetColor("_BaseColor", owner.PlayerColor);
        }

    }

}
