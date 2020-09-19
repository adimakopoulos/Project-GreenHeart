using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CombatStatsData", menuName = "ScriptableObjects/CombatStatsData")]
[Serializable]
public class BaseValues : ScriptableObject
{

    public string UnitName;
    public string UnitDescr;
    public float MovementSpeed;

    public float baseHealth = 1f;
    public DamageType damageType;
    public float baseDamage = 0f;


    //-------
    public enum ArmorType { None, Physical, Elemental };
    public ArmorType armorType = ArmorType.None;
    [Range(0, 80)]
    [SerializeField]
    private int _baseArmor = 0;

    [Range(0, 80)]
    [SerializeField]
    private int _baseElementalArmor = 0;
    [SerializeField]



    /// <summary>
    /// Set some dependacies for our values. since armor is gonna be used as percentage , we dont want to go too high, and it should NEVER be 100.
    /// A value of 100 armor means that the Character does not take any Damage.
    /// !!!Important!!!
    /// OnValidate does not validate when other monoBehavior scripts change the value in their Ypdate method
    /// so i need to check the value on get set
    /// </summary>
    private void OnValidate()
    {
        UnitName.Trim();
        UnitDescr.Trim();
        MovementSpeed = Mathf.Clamp(MovementSpeed, 0f, 100f);
        _baseArmor= Mathf.Clamp(_baseArmor, 0, 80);
        _baseElementalArmor= Mathf.Clamp(_baseElementalArmor, 0, 80);
    }


    /// <summary>
    /// Armor is a persentage  that reduces damage. it is Capped to minimum = 0 and maximum =80
    /// </summary> 
    public int BaseArmor
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
    public int BaseElementalArmor
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

}
