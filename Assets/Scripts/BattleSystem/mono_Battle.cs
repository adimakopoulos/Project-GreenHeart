using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WorldBuilder;
public class mono_Battle : MonoBehaviour
{
    private Tile tile;
    GameObject go_battle;
    private Player _strongestPlayer;
    [SerializeField] private GameObject textUIPrefab;
    Army _army1, _army2, _army3, _army4;

    GameObject myScore;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        InvokeRepeating("addCapurePoints", 0, 0.25f); //every hlf a second add capture points to the Leading Army so in Conqure the Tile
        InvokeRepeating("doBattle", 0, 0.25f);




    }
    private void Initialize()
    {
        int x = Mathf.FloorToInt(gameObject.transform.position.x);
        int z = Mathf.FloorToInt(gameObject.transform.position.z);
        //Debug.Log("("+gameObject.transform.position.x +","+ gameObject.transform.position.y + ") Battle at");
        tile = mono_BoardCreate.map.getTileFromMap(x, z);


        go_battle = new GameObject("BATTLE: " + x + " " + z);
        go_battle.transform.position = new Vector3(x, 0, z);
        textUIPrefab = Resources.Load<GameObject>("prefabs/txtMeshProPre") as GameObject;
        myScore = Instantiate(textUIPrefab, this.transform.position, Quaternion.identity, go_battle.transform);
        //
        //make text appear in the middle of tile
        myScore.transform.localPosition = new Vector3(0.5f, 0.7f, 0.5f);




    }


    // Update is called once per frame
    void Update()
    {
        myScore.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.position);  
        _setProgress();

    }



    public void doDamage()
    {
        //gonna do the battle logic here so the behavior doenst change on different machines.

    }

    private void _setProgress()
    {
        //only show text when the objrext is instatiated and initialized
        if (myScore != null)
        {
            if (tile.capturePoints < 101)
            {
                myScore.GetComponent<TextMeshPro>().text = tile.capturePoints.ToString();
            }
            else {
                myScore.GetComponent<TextMeshPro>().text ="100";
            }
            
        }

    }

    //is invoked at the Start() in this script
    public void addCapurePoints()
    {
        if (_strongestPlayer != null)
        {
            if (tile.capturePoints < 101 && _strongestPlayer != tile.Owner)
            {
                tile.capturePoints += tile.getNumberOfUnitsInTile();


            }
            else if (tile.capturePoints >= 100 && _strongestPlayer != tile.Owner)
            {
               
                tile.Owner = _strongestPlayer;
                tile.getGoTile().GetComponent<mono_Battle>().enabled = false;
                Debug.Log("Destroy Disabble");
                myScore.GetComponent<TextMeshPro>().text = " ";
                CancelInvoke();//stop adding points

                //Destroy(go_battle);
            }
        }
    }


    //how many people are trying to Conguer this tile

    List<string> names = new List<string>();
    public int getNumberOfArmies()
    {

        tile.MoveableGameobjects[0].GetComponent<ITakeDamage>().getOwner().ToString();
        //tile.MoveableGameobjects[0].GetComponent<Moveable>().UnitOutOfTileBounds += addUnitInArmy;
        //ask all units their owners. Store all Players as Unigue in the list
        foreach (GameObject unit in tile.MoveableGameobjects)
        {
            
            ITakeDamage temp = unit.GetComponent<ITakeDamage>();
            //Debug.Log(unit.GetComponent<ITakeDamage>().getOwner().ToString());
            if (temp != null)
            {
                //if it doenst contain the name then add it
                if (!(names.Contains(temp.getOwner().Name)))
                {
                    names.Add(temp.getOwner().Name);
                }
            }


        }

        return names.Count;
    }

    
 


    public void doBattle()
    {
        
        int _getNumberOfArmies = getNumberOfArmies();
        if (_getNumberOfArmies == 1)
        {
            _strongestPlayer = mono_playerManager.FindPlayerByName(names[0]);
        }
        else if (_getNumberOfArmies > 1 && _getNumberOfArmies < 3)
        {
            
            List <ITakeDamage> createArmyBlue = new List<ITakeDamage>() ;
            List<ITakeDamage> createArmyRed = new List<ITakeDamage>();
            foreach (GameObject unit in tile.MoveableGameobjects)
            {
                if (unit == null) { break; }


    
                ITakeDamage temp = unit.GetComponent<ITakeDamage>();
                if ( mono_playerManager.p1.Name.Equals( temp.getOwner().ToString()))
                {
                    createArmyBlue.Add(temp);
                }
                else {
                    createArmyRed.Add(temp);
                }

                

            }
            var dmg1 = createArmyBlue.Count;
            var dmg2 = createArmyRed.Count;

            foreach (GameObject unit in tile.MoveableGameobjects)
            {
                if (unit == null) { break; }
                ITakeDamage temp = unit.GetComponent<ITakeDamage>();
                
                if (mono_playerManager.p1.Name.Equals(temp.getOwner().ToString()))
                {
                    temp.iTakeDamage(35, DamageType.trueDamage);
                }
                else
                {
                    temp.iTakeDamage(60, DamageType.trueDamage);
                }
            }
                

        }
        else
        {
            Debug.LogWarning("Something went wrong in mono_Battle. there are 5 or more people contesting from a tile");
            //realisticly we should never be in here
            return;

        }

    }

    private void addUnitInArmy() {
        Debug.Log("addUnitInArmyaddUnitInArmyaddUnitInArmy");
    }





}
