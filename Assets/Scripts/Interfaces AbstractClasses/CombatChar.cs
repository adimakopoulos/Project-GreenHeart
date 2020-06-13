
using UnityEngine;

public abstract class CombatChar : MonoBehaviour
{

    public float MaxHealth = 10;
    private float currHealth;

    [SerializeField]
    private int _armorAmmount = 0;
    [SerializeField]
    private int _damage = 0;
    [SerializeField]
    private int _elementalArmor = 0;

    private enum DamageType { hack, pierce, elemental };
    private enum ArmorType { physical, elemental };
    [SerializeField]
    private DamageType _damageType;
    [SerializeField]
    private ArmorType _armor;

    //armor is a persentage damage reduction. it is Capped to minimum = 0 and maximum =80
    public int ArmorAmmount
    {

        set
        {
            if (value <= 80 && value >= 0)
            {
                _armorAmmount = value;
            }
            else
            {
                Debug.LogWarning("You are trying to set ArmorAmmount with a bad value");
            }
        }

    }


    private void Awake()
    {
        currHealth = MaxHealth;
    }

    void takeDamage(float dmg, DamageType dmgType)
    {
        switch (dmgType)
        {
            case DamageType.hack:
                currHealth -= (dmg * (_armorAmmount / 100));
                break;
            case DamageType.pierce:

                currHealth -= (dmg * (_armorAmmount / 50));
                break;
            case DamageType.elemental:
                currHealth -= (dmg * (_elementalArmor / 100));
                break;
            default:
                Debug.LogWarning("Damage type has not been identified. New type should be added here");
                break;
        }


        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void healAmmount(float heal)
    {
        currHealth += currHealth;
    }





}
