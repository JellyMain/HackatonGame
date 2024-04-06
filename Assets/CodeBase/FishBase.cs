using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class FishBase : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackCooldown = 5;
    public float movementSpeed = 5f;
    public float rotationSpeed = 3f;
    public float damage = 10;
    public int size = 0;
    private Health health;
    private FishAnimator fishAnimator;
    private float cooldownTimer = 0;
    private bool isInCooldown;
    
    FishType fishType = FishType.Mutated;

    public delegate void KilledFish(FishType fishType);
    public KilledFish OnKilledFish;


    private void Awake()
    {
        fishAnimator = GetComponentInChildren<FishAnimator>();
        health = GetComponent<Health>();
    }


    private void Update()
    {
        if (isInCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isInCooldown = false;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        CheckForAttack();
    }



    public bool IsBigger(int enemySize)
    {
        if (enemySize > size)
        {
            return false;
        }

        return true;
    }
    
    private void CheckForAttack()
    {
        Collider2D collider = Physics2D.OverlapCircle(attackPoint.position, attackRadius);

        if (collider == null) return;

        FishBase targetFish = collider.GetComponent<FishBase>();

        if(targetFish == this) return;        

        AiFishController aiFish = targetFish.GetComponent<AiFishController>();
        if(aiFish != null)
        {
            if (aiFish.IsSeeingCollider(collider))
                return;
        }

        if (targetFish.size > size) return;

        if (!isInCooldown && targetFish != null)
        {
            Attack(targetFish);
        }
    }


    private void Attack(FishBase targetFish)
    {
        GameObject target = targetFish.health.ReduceHp(damage);
        //fishAnimator.PlayBite();
        
        isInCooldown = true;
        cooldownTimer = attackCooldown;
    
        if(target != null && OnKilledFish != null)
        {
            OnKilledFish(target.GetComponent<FishBase>().fishType);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    
}
