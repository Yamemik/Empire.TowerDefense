using UnityEngine;

public enum projectileType { 
    rock,arrow,fireball
};
public class Projectile : MonoBehaviour
{
    [SerializeField]
    int attackDamage;
    [SerializeField]
    projectileType pType;

    public int AttackDamage
    {
        get
        {

            return attackDamage;
        }
    }

    public projectileType PType
    {
        get
        {

            return pType;
        }
    }

    private void Update()
    {
        Destroy(gameObject, 3);
    }
}
