using UnityEngine;


public class Health: MonoBehaviour
{
    public float hp;


    public void ReduceHp(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Destroy(gameObject);
    }
}
