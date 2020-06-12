using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween_Border : MonoBehaviour
{
    public GameObject up_border;
    public GameObject dowm_border;
    public GameObject left_border;
    public GameObject right_border;

    private LTDescr tw, tw1, tw2, tw3;


    private void Start()
    {
        tw = LeanTween.moveLocalZ(up_border, 0.95f, 1f).setEaseOutCubic().setLoopPingPong();
        tw1 = LeanTween.moveLocalZ(dowm_border, 0.05f, 1f).setEaseOutCubic().setLoopPingPong();
        tw2 = LeanTween.moveLocalX(left_border, 0.05f, 1f).setEaseOutCubic().setLoopPingPong();
        tw3 = LeanTween.moveLocalX(right_border, 0.95f, 1f).setEaseOutCubic().setLoopPingPong();

    }



    private void OnEnable()
    {

        if (tw == null || tw1 == null || tw2 == null || tw3 == null)
        {
            return;
        }
        else {
            tw.resume();
            tw1.resume();
            tw2.resume();
            tw3.resume();
        }
    }

    private void OnDisable()
    {

        tw.pause();
        tw1.pause();
        tw2.pause();
        tw3.pause();



    }


}
