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
        //dissable UI 
        refButtonSave.SetActive(false);
        refButtonLoad.SetActive(false);
        refTextFieldSaveFile.SetActive(false);
        scrllPannel.SetActive(false);

    }
    public void activateUI()
    {
        //dissable UI 
        refButtonSave.SetActive(true);
        refButtonLoad.SetActive(true);
        refTextFieldSaveFile.SetActive(true);
        scrllPannel.SetActive(true);

    }





}
