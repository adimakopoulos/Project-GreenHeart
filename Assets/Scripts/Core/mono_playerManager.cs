using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class mono_playerManager :MonoBehaviour
{
    /// <summary>
    /// Load Colors from the editor tothe static field
    /// </summary>
    private void Awake()
    {
        p0.setColor(colors[0]);
        p1.setColor(colors[1]);
        p2.setColor(colors[2]);
        p3.setColor(colors[3]);
        p4.setColor(colors[4]);

    }
    [SerializeField]
    public Color[] colors = new Color[5];
    public static Player p0 = new Player("Gaia", Color.magenta);
    public static Player p1 = new Player("Takhs", Color.white);
    public static Player p2 = new Player("Scorpino", Color.magenta);
    public static Player p3 = new Player("Pig", Color.magenta);
    public static Player p4 = new Player("Goat", Color.magenta);

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

