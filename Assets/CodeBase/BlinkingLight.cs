using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class BlinkingLight : MonoBehaviour
{
    [SerializeField] float maxIntensity;
    [SerializeField] float minIntensity;
    [SerializeField] float maxOuterRadius;
    [SerializeField] float minOuterRadius;
    [SerializeField, Range(2.1f, 5)] float maxDuration;
    [SerializeField, Range(0.1f, 2)] float minDuration;

    private Light2D light2D;
    private float duration;
    private float elapsedTime = 0f;
    private bool increasing = false;
    private bool canBlink = false;


    IEnumerator Start()
    {
        light2D = GetComponent<Light2D>();
        duration = UnityEngine.Random.Range(minDuration, maxDuration);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, 3.5f));
        canBlink = true;
    }


    private void Update()
    {
        if (!canBlink) return;

        elapsedTime += Time.deltaTime;

        float t = elapsedTime / maxDuration;

        if (increasing)
        {
            light2D.pointLightOuterRadius = Mathf.Lerp(minOuterRadius, maxOuterRadius, t);
            light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            if (t >= 1f)
            {
                increasing = false;
                elapsedTime = 0f;
            }
        }
        else
        {
            light2D.pointLightOuterRadius = Mathf.Lerp(maxOuterRadius, minOuterRadius, t);
            light2D.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);

            if (t >= 1f)
            {
                increasing = true;
                elapsedTime = 0f;
            }
        }
    }
}
