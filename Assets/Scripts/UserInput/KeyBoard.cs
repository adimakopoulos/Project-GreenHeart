using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoard : MonoBehaviour {

  
    public  GameObject refButtonSave; 
    public  GameObject refButtonLoad;
    public  GameObject refTextFieldSaveFile;
    public  GameObject scrllPannel;


  
    // Use this for initialization
    void Start() {

        deActivateUI();
    }


    // Update is called once per frame
    void Update () {


        //handle save ui
        if (Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.P)) { 
            activateUI();

        }

        if (Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.X))
        {
            deActivateUI();

        }


    }



    public void deActivateUI() {


    }
    public void activateUI()
    {

    }





}
