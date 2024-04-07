using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrash : FishBase
{
    // Start is called before the first frame update

    Vector3 initialPosition;

    float lifetime = 0;

    void Start()
    {
        fishType = FishType.Mutated;     
    
        initialPosition = transform.position;
    }

    private void Update()
    {
        lifetime += Time.deltaTime;
        
        transform.position = initialPosition + Vector3.up * .3f * Mathf.Sin(lifetime);
    }
}
