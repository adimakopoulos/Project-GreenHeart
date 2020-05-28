using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monoScreenFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Camera.main.orthographicSize = Screen.height / 32f / 2f;
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void LateUpdate()
    {
        this.transform.position =
    new Vector3(
    Mathf.Round(this.transform.position.x * 100f) / 100f,
    Mathf.Round(this.transform.position.y * 100f) / 100f,
    Mathf.Round(this.transform.position.z * 100f) / 100f);
    }

}
