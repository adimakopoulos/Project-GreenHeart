using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(gameObject, new Vector3(0,0,0) , 1.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
