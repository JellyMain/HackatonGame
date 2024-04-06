using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HungerManager : MonoBehaviour
{
    [SerializeField] private Image hungerBarFilling;
    [SerializeField] private float maxHunger = 50f;
    private float currentHunger;


    private void Start()
    {
        currentHunger = maxHunger;
    }


    private void Update()
    {
        ReduceHungerBar();
    }


    
    private void ReduceHungerBar()
    {
        if (currentHunger > 0)
        {
            currentHunger -= Time.deltaTime;
            hungerBarFilling.fillAmount = currentHunger/maxHunger;
        }
    }
}
