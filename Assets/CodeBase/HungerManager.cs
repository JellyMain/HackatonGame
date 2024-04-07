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

    FishBase fishBase;


    private void Start()
    {
        currentHunger = maxHunger;

        fishBase = GetComponent<FishBase>();

        fishBase.OnKilledFish += OnKilledFish;
    }


    private void Update()
    {
        ReduceHungerBar();
    }

    public void OnKilledFish(FishType fishType)
    {
        IncreaseHunger(10);
    }


    private void ReduceHungerBar()
    {
        if (currentHunger > 0)
        {
            currentHunger -= Time.deltaTime;
            hungerBarFilling.fillAmount = currentHunger/maxHunger;
        }
    }

    private void IncreaseHunger(float amount)
    {
        currentHunger += amount;
    }
}
