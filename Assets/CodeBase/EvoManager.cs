using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EvoManager : MonoBehaviour
{
    [SerializeField] private Image evoBarFilling;
    private SpriteRenderer fishSpriteRenderer;
    private FishAnimator fishAnimator;
    private FishBase fishBase;
    private int currentEvoLevel = 0;
    private int mutatedFishNumber = 0;
    private int healthyFishNumber = 0;
    private int totalFishNumber = 0;

    private readonly Dictionary<int, int> evoLevelsDictionary = new Dictionary<int, int>()
    {
        [0] = 5,
        [1] = 7,
        [2] = 9,
        [3] = 11
    };
    
    


    private void Start()
    {
        fishAnimator = GetComponentInChildren<FishAnimator>();
        fishSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        fishBase = GetComponent<FishBase>();

        fishBase.OnKilledFish += OnKilledFish;

        UpdateEvoBar();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            IncreaseEvoBar(FishType.Mutated);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            IncreaseEvoBar(FishType.Healthy);
        }
    }

    public void OnKilledFish(FishType fishType)
    {
        IncreaseEvoBar(fishType);
    }



    private void UpdateEvoBar()
    {
        float fillAmount = (float)totalFishNumber / evoLevelsDictionary[currentEvoLevel];
        evoBarFilling.fillAmount = fillAmount;
        
        Color barColor = Color.Lerp(Color.red, Color.green, (float)healthyFishNumber / totalFishNumber);
        evoBarFilling.color = barColor;
    }

    

    private void IncreaseEvoBar(FishType fishType)
    {
        if (fishType == FishType.Mutated)
        {
            mutatedFishNumber++;
        }
        else if(fishType == FishType.Healthy)
        {
            healthyFishNumber++;
        }

        totalFishNumber = healthyFishNumber + mutatedFishNumber;
        

        if (totalFishNumber == evoLevelsDictionary[currentEvoLevel])
        {
            Evolution();
            currentEvoLevel++;
            ResetFishNumber();
        }

        UpdateEvoBar();
    }


    private void Evolution()
    {
        if (mutatedFishNumber > healthyFishNumber)
        {
            fishAnimator.EvolveMutated();
        }
        else
        {
            fishAnimator.EvolveHealthy();
        }
    }


    private void ResetFishNumber()
    {
        totalFishNumber = 0;
        mutatedFishNumber = 0;
        healthyFishNumber = 0;
    }
    
}