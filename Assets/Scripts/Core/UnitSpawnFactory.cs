using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldBuilder;

public static class UnitSpawnFactory 
{
    public enum UnitType {Hoplite , Hyspaspist  }
    // Start is called before the first frame update

    public static GameObject SpawnHoplite(Tile tile, Player player)
    {
        GameObject GoHoplite = GameObject.Instantiate(Resources.Load("PreFabs/PreUnit", typeof(GameObject))) as GameObject;
        BaseValues Stats = Resources.Load("Stats/Hoplite", typeof(ScriptableObject)) as BaseValues;


        //GoHoplite.GetComponent<Renderer>().material.color = player.PlayerColor;
        GoHoplite.GetComponent<Renderer>().material.SetColor("_BaseColor", player.playerColor);
        GoHoplite.AddComponent<Moveable>().setLocation(tile);
        GoHoplite.GetComponent<Moveable>().setSpeed(Stats.MovementSpeed);
        GoHoplite.AddComponent<Unit>();
        GoHoplite.GetComponent<Unit>().initialize(Stats, tile.Owner);
        //Spawn unit in a random posiotion inside a tile

        GoHoplite.transform.position = new Vector3(tile.X + 0.2f, tile.TileGraphicsHeight, tile.Z + 0.2f);
        GoHoplite.transform.SetParent(tile.getGoTile().transform, true);

        GoHoplite.name = Stats.UnitName + " (" + player.Name + ")";







        return GoHoplite;
    }

    public static GameObject SpawnHypaspist(Tile tile, Player player)
    {
        GameObject GoHypaspist = GameObject.Instantiate(Resources.Load("PreFabs/PreUnit", typeof(GameObject))) as GameObject;
        BaseValues Stats = Resources.Load("Stats/Hypaspist", typeof(ScriptableObject)) as BaseValues;


        //GoHypaspist.GetComponent<Renderer>().material.color = player.PlayerColor;
        GoHypaspist.GetComponent<Renderer>().material.SetColor("_BaseColor", player.PlayerColor);
        GoHypaspist.AddComponent<Moveable>().setLocation(tile);
        GoHypaspist.GetComponent<Moveable>().setSpeed(Stats.MovementSpeed);
        GoHypaspist.AddComponent<Unit>();
        GoHypaspist.GetComponent<Unit>().initialize(Stats , tile.Owner);
        //Spawn unit in a random posiotion inside a tile


        GoHypaspist.transform.position = new Vector3(tile.X + 0.2f, tile.TileGraphicsHeight, tile.Z + 0.2f);
        GoHypaspist.transform.SetParent(tile.getGoTile().transform, true);
        GoHypaspist.name = Stats.UnitName + " (" + player.Name + ")";




        return GoHypaspist;
    }
}
