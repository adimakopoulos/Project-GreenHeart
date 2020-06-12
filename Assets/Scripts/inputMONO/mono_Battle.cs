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


    //GameObject myScore = Resources.Load<GameObject>("Text") as GameObject;
    GameObject myScore;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        InvokeRepeating("addCapurePoints",0,0.5f); //every hlf a second add capture points to the Leading Army so in Conqure the Tile





    }
    private void Initialize (){
        int x = Mathf.FloorToInt(gameObject.transform.position.x);
        int z = Mathf.FloorToInt(gameObject.transform.position.z);
        //Debug.Log("("+gameObject.transform.position.x +","+ gameObject.transform.position.y + ") Battle at");
        tile = mono_BoardCreate.map.getTileFromMap(x, z);


        go_battle = new GameObject("BATTLE");
        go_battle.transform.position = new Vector3(x, z, 0f);
        textUIPrefab = Resources.Load<GameObject>("prefabs/txtMeshProPre") as GameObject;
        myScore = Instantiate(textUIPrefab,this.transform.position, Quaternion.identity, go_battle.transform);
        //make text appear in the middle of tile
        myScore.transform.localPosition = new Vector3(0.5f, 0.3f, 0.5f);



        

    }


    // Update is called once per frame
    void Update()
    {

        //in case that there is only 1 player
        setLeadingTeam();
        setProgress();
        //in case there are 2 or more players
        doDamage();


    }



    public void doDamage() { 
    //gonna do the battle logic here so the behavior doenst change on different machines.
    
    }

    private void setProgress() {
        //only show text when the objrext is instatiated and initialized
        if (myScore!= null) { 
            myScore.GetComponent<TextMeshPro>().text = tile.capturePoints.ToString();
        }

    }

    //is invoked at the Start() in this script
    public void addCapurePoints()
    {
        if (_strongestPlayer != null) 
        { 
        if (  tile.capturePoints < 101 && _strongestPlayer!=tile.Owner)
        {
            tile.capturePoints += tile.units.Count;

            
        }
        else if (tile.capturePoints >= 100  && _strongestPlayer != tile.Owner) {
            tile.capturePoints = 0;
            tile.Owner = _strongestPlayer;
            tile.getGoTile().GetComponent<mono_Battle>().enabled= false;
            Debug.Log("Destroy BATTLE");
            CancelInvoke();//stop adding points
            
            Destroy(go_battle);
        }
        }
    }


    //how many people are trying to Conguer this tile
    ArrayList names = new ArrayList();
    public int getNumberOfArmies()
    {
        //ask all units their owners. Store all Players as Unige in the list
        foreach (Unit unit in tile.units)
        {
            //if it doenst contain the name then add it
            if (!(names.Contains(unit.Owner))) {
                names.Add(unit.Owner);
            }
            
        }

        return names.Count;
    }

    //calculate capture point
    public void setLeadingTeam (){
        int _getNumberOfArmies = getNumberOfArmies();
        if (_getNumberOfArmies == 1)
        {
            _strongestPlayer = (Player)names[0];
            //Debug.Log(_strongestPlayer.Name);
        }
        else if (_getNumberOfArmies == 2)
        {
            //tile.


        }
        else {
            Debug.Log("Something went wrong in mono_Battle ");
            //realisticly we should never be in here
            return;
        
        }
    
    }

  




}
