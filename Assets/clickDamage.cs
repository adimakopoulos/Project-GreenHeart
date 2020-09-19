
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Input.GetKeyDown("0"))
        {

            //float start;

            //start = Time.realtimeSinceStartup;
            //HitManager hitM = null;
            //for (int i = 0; i < 10000; i++) { hitM = Main.actorController.GetComponent<HitManager>(); }
            //print("GetComponent took:\t" + (Time.realtimeSinceStartup - start));

            //start = Time.realtimeSinceStartup;
            //IDamageable<Attack, Vector3> HitInterface = null;
            //for (int i = 0; i < 10000; i++) { HitInterface = Main.actorController.GetComponent<IDamageable<Attack, Vector3>>(); }
            //print("GetInterface took:\t" + (Time.realtimeSinceStartup - start));

            //hitM.Hit(AttackManager.playerThrust, Vector3.forward, Faction.Player);
            //HitInterface.Famage(AttackManager.playerThrust, Vector3.forward);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray rey = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            if (Physics.Raycast(rey, out hitinfo)) {
                ITakeDamage tem = hitinfo.collider.GetComponent<ITakeDamage>();
                hitinfo.collider.GetComponentInParent<Renderer>().material.color = Color.white;
                Debug.Log(tem.getOwner().ToString() + " -----" + this.gameObject.name + "           "  );
                tem.iTakeDamage(999f, DamageType.trueDamage);
            }
        }
    }

}
