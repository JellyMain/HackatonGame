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
    private float timer = 0;
    private bool isInCooldown;


    private void Awake()
    {
        health = GetComponent<Health>();
    }


    private void Update()
    {
        CountdownTimer();
        Debug.Log(timer);
    }


    private void FixedUpdate()
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


    private void CountdownTimer()
    {
        if (isInCooldown)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            isInCooldown = false;
            timer = 0;
        }
    }



    private void CheckForAttack()
    {
        Collider2D collider = Physics2D.OverlapCircle(attackPoint.position, attackRadius);

        if (collider == null) return;

        FishBase targetFish = collider.GetComponent<FishBase>();

        if (!isInCooldown && targetFish != null)
        {
            Attack(targetFish);
        }
    }


    private void Attack(FishBase targetFish)
    {
        targetFish.health.ReduceHp(damage);
        Debug.Log(targetFish.health.hp);
        ResetTimer();
    }


    private void ResetTimer()
    {
        isInCooldown = true;
        timer = attackCooldown;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
