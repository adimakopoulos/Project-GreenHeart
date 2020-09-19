using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum DamageType { hack, pierce, elemental, trueDamage };

    public interface ITakeDamage
    {

        void iTakeDamage(float damageAmount, DamageType damageType);
        Player getOwner();


    }

