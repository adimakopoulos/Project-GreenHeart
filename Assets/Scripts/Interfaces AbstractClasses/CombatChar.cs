
using UnityEngine;

public abstract class CombatChar : MonoBehaviour
{
    
    public float maxHealth;
    private float currHealth;

    [SerializeField]
    private int armorAmmount;
    [SerializeField]
    private int damage;

    private enum damageType {hack, pierce, elemental };
    private enum armorType { physical, elemental };
    private void Awake()
    {
        currHealth = maxHealth;
    }

    void takeDamage(float dmg) {

        currHealth -= dmg;
        if (currHealth <= 0) {
            Destroy(gameObject); 
        }
    }

    void healAmmount(float heal)
    {

        currHealth += currHealth;
    }




}
