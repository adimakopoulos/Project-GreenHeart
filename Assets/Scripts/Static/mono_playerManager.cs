using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class mono_playerManager :MonoBehaviour
{
    public static Player p0 = new Player("Gaia", Color.white);
    public static Player p1 = new Player("Takhs", Color.blue);
    public static Player p2 = new Player("Scorpino", Color.red);
    public static Player p3 = new Player("Pig", Color.cyan);
    public static Player p4 = new Player("Goat", Color.yellow);

    public static Player FindPlayerByName(string name) {

        switch (name)
        {
            case "Gaia":
                return p0;
            case "Takhs":
                return p1;
            case "Scorpino":
                return p2;
            case "Pig":
                return p3;
            case "Goat":
                return p4;
            default: 
                return null;

        }


    }


}

