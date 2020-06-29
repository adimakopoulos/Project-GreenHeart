using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine;
using WorldBuilder;

public class Unit : MonoBehaviour , ITakeDamage
{

    public string UnitName ;
    public string UnitDescr;
    public float MovementSpeed;

    public float baseHealth = 1f;
    public float bonusHealth = 0f;
    [ReadOnly]
    private float _currentHealth;

    //--------
    public DamageType damageType;
    public float BaseDamage = 0f;
    public float BonusDamage = 0f;
    [ReadOnly]
    private float _currentDamage;

    //-------
    public enum ArmorType { None, Physical, Elemental };
    public ArmorType armorType = ArmorType.None;

    [ReadOnly]
    private int _baseArmor = 0;
    public int bonusArmor = 0;
    [ReadOnly]
    private int _currentArmor;

    [ReadOnly]
    private int _baseElementalArmor = 0;

    internal void initialize(BaseValues stats, Player owner)
    {
        UnitName = stats.UnitName;
        UnitDescr = stats.UnitDescr;
    }

    public int bonusElementalArmor = 0;
    [ReadOnly]
    private int _currentElementalArmor;

    public Unit(BaseValues BV) {

    }
    

    /// <summary>
    /// Armor is a persentage  that reduces damage. it is Capped to minimum = 0 and maximum =80
    /// </summary> 
    public int ArmorAmmount
    {
        get => _baseArmor;
        set
        {
            if (value <= 80 && value >= 0)
            {
                _baseArmor = value;
            }
            else
            {
                Debug.LogWarning("You are trying to set ArmorAmmount with a bad value");
            }
        }

    }
    /// <summary>
    /// ElementalArmor is a persentage that reduces damage. it is Capped to minimum = 0 and maximum =80
    /// </summary>
    public int ElementalArmor
    {
        get => _baseElementalArmor;
        set
        {
            if (value <= 80 && value >= 0)
            {
                _baseElementalArmor = value;
            }
            else
            {
                Debug.LogWarning("You are trying to set _elementalArmor with a bad value");
            }
        }
    }

    public float CurrentDamage { get => _currentDamage; set { _currentDamage = Mathf.Clamp(_currentDamage, 0, BaseDamage + BonusDamage); _currentDamage = value; } }
    public float CurrentHealth { get => _currentHealth; set { Mathf.Clamp(_currentHealth, 0, baseHealth + bonusHealth); _currentHealth = value; } }
    public int CurrentArmor
    {
        get => _currentArmor;
        set
        {
            _currentArmor = Mathf.Clamp(_currentArmor, 0, 80);
            _currentArmor = value;
        }
    }
    public int CurrentElementalArmor
    {
        get => _currentElementalArmor;
        set
        {
            _currentElementalArmor = Mathf.Clamp(_currentElementalArmor, 0, 80);
            _currentElementalArmor = value;
        }

    }


    void healAmmount(float heal)
    {
        CurrentHealth += CurrentHealth;
    }

    /// <summary>
    ///EX a CombatChare with 100 points in armor will reduce the hack attack points by 100% <br></br>
    ///witch means the out character will not take any armor<br></br>
    ///                                                                             <br></br>
    ///hack attacks = reduced buy armor 100<br></br>
    ///pierce attacks = reduced buy armor 50<br></br>
    ///elemental attacks = reduced buy elemental armor 70<br></br>
    ///TRUE damage attacks = are Not reduced by any type of armor<br></br>
    /// </summary>
    /// <param name="damageAmount"> </param>
    /// <param name="damageType"></param>
    public void iTakeDamage(float damageAmount, DamageType damageType)
    {
        /*
        there is a chance that armor might become less than 0 , if this happens then 
        the Current health of the object will increase (because we are subtracking 2 negative numbers)
        so we check if armor negative. 


         */
        switch (damageType)
        {
            case DamageType.hack:
                if (CurrentArmor == 0) { CurrentHealth -= damageAmount; break; }
                CurrentHealth -= (damageAmount * (CurrentArmor * 0.01f * 0.8f));
                //currHealth -= (_armorAmount < 0) ? _armorAmount = 0 : (damageAmount * (_armorAmount / 100));
                break;
            case DamageType.pierce:
                if (CurrentArmor == 0) { CurrentHealth -= damageAmount; break; }
                CurrentHealth -= (damageAmount * (CurrentArmor * 0.01f * 0.5f));
                break;
            case DamageType.elemental:
                if (ElementalArmor == 0) { CurrentHealth -= damageAmount; break; }
                CurrentHealth -= (damageAmount * (ElementalArmor * 0.01f * 0.7f));
                break;
            case DamageType.trueDamage:
                CurrentHealth = CurrentHealth - damageAmount;
                Debug.LogWarning(CurrentHealth);
                break;
            default:
                Debug.LogWarning("Damage type has not been identified. New type should be added at (DamageSystem.ITakeDamage) ");
                break;
        }


        //Debug.Log("Die!!!!" + damageAmount + damageType + " CurrentHealth:" + combatStats.CurrentHealth);
        if (CurrentHealth < 0)
        {
            Debug.Log("Destroy(gameObject);");
            //gameObject.GetComponent<Moveable>().CurrentTile.MoveableGameobjects.Remove(gameObject);
            gameObject.GetComponent<Moveable>().enabled = false;

            Destroy(gameObject, 1f);
        }

    }



    Player ITakeDamage.getOwner()
    {
        throw new NotImplementedException();
    }
}




