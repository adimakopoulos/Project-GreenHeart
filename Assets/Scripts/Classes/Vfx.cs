using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vfx {
    private readonly int x,y;


    ////Sprite sprLoad;
    ////GameObject go_Vfx;
    ////public enum tileVfx { Up, Down, Left, Right, Center, Empty, Select };
    //////private tileVfx vfxType = tileVfx.Empty;

    //////"UI" like Effects that appear on our Board and tules 
    ////public Vfx(int x, int y, tileVfx type)
    ////{
    ////    this.x = x;
    ////    this.y = y;


    ////    //create game object. add sprite Renedder and other components
    ////    go_Vfx = new GameObject("Vfx for Tile = X:" + x + " Y:" + y);
    ////    go_Vfx.AddComponent<SpriteRenderer>();
    ////    setVfxSpr(type);
    ////    //go_Vfx.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f); i have to think about this
    ////    go_Vfx.transform.position = new Vector3(x, y, 0);

    ////    //Shorting layer so our objectes are Drawn with the correct order
    ////    go_Vfx.GetComponent<SpriteRenderer>().sortingLayerName = "Board_UI";
    ////    go_Vfx.GetComponent<SpriteRenderer>().sortingOrder = 101;//100+ us for Game ui ,(Unity ui are loaded after so no matter how hight it will never be drawn over Unity Layoyt)
    ////    //Set parrent of all vfx to go_vfx game objext so our hierarchy remains clean.
    ////    go_Vfx.transform.SetParent(GameObject.Find("go_vfx").transform, true);


    ////}
    ////public GameObject getGoVFX() { return go_Vfx; }
    ////public void setVfxSpr(tileVfx type)
    ////{

    ////    switch (type)
    ////    {
    ////        case Vfx.tileVfx.Select: sprLoad = Resources.Load("VFX/spr_select", typeof(Sprite)) as Sprite; break;
    ////        case Vfx.tileVfx.Up: sprLoad = Resources.Load("VFX/spr_upDir", typeof(Sprite)) as Sprite; break;
    ////        case Vfx.tileVfx.Down: sprLoad = Resources.Load("VFX/spr_downDir", typeof(Sprite)) as Sprite; break;
    ////        case Vfx.tileVfx.Left: sprLoad = Resources.Load("VFX/spr_leftDir", typeof(Sprite)) as Sprite; break;
    ////        case Vfx.tileVfx.Right: sprLoad = Resources.Load("VFX/spr_rightDir", typeof(Sprite)) as Sprite; break;
    ////        case Vfx.tileVfx.Center: sprLoad = Resources.Load("VFX/spr_Center", typeof(Sprite)) as Sprite; break;
    ////        default: Debug.Log("Sprite not Detected"); break;
    ////    }

    ////    go_Vfx.GetComponent<SpriteRenderer>().sprite = sprLoad;

    ////    //change tranparensy of image using a temp color Variable
    ////    Color tmp = go_Vfx.GetComponent<SpriteRenderer>().color;
    ////    tmp.a = 0.3f;
    ////    go_Vfx.GetComponent<SpriteRenderer>().color = tmp;

    ////}



}
