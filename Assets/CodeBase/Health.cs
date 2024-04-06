using UnityEngine;


public class Health: MonoBehaviour
{
    public float hp;


    public void ReduceHp(float damage)
    {
        hp -= damage;
        //Debug.Log("Ahh! I got DAmage!");

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
