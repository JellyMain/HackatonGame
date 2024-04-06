using UnityEngine;


public class Health: MonoBehaviour
{
    public float hp;

    public delegate void Died();
    public Died OnDied;


    public GameObject ReduceHp(float damage)
    {
        hp -= damage;
        //Debug.Log("Ahh! I got DAmage!");

        if (hp <= 0)
        {
            Die();
            return gameObject;
        }

        return null;
    }


    private void Die()
    {
        Destroy(gameObject);

        if(OnDied != null)
        {
            OnDied.Invoke();
        }
    }
}
