using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndCutSceneManager : MonoBehaviour
{
    [SerializeField] private Sprite goodScene;
    [SerializeField] private Sprite badScene;
    [SerializeField] private Image cutSceneImage;
    [SerializeField] private TMP_Text storyText;
    private string goodText = "As life trudged on, our resilient fish persisted in its quest for sustenance, eschewing the temptation of devouring garbage in favor of seeking out organic nourishment whenever feasible. Yet, with each passing day, the hunt for food grew increasingly arduous. Now, with solemn certainty, we can declare this diminutive fish as the solitary survivor, the final scion of a once-teeming marine realm.";
    private string badText = "In its relentless ascent to power, the fish carved its path through murky waters, its rise marked by dubious means. Yet, even as it attained dominance, it showed no mercy to the delicate balance of local fauna. One could ponder whether it was the harbinger of destruction or merely a symptom of an already ailing ecosystem. Regardless, aided by human neglect, the insatiable appetite of this aquatic tyrant knew no bounds. Its voracious hunger laid waste to all marine life in its vicinity, casting a shadow over the once-thriving closed ecosystem.";

    IEnumerator Start()
    {
        ShowCutScene();
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(TurnWhiteCoroutine());

        yield return new WaitForSeconds(1f); 

        yield return StartCoroutine(TurnBlackCoroutine());

        yield return StartCoroutine(ShowStoryTextCoroutine());
    }

    private void ShowCutScene()
    {
        if (PlayerPrefs.HasKey("GoodEnding"))
        {
            cutSceneImage.sprite = goodScene;
            storyText.text = goodText;
            PlayerPrefs.DeleteAll();
        }
        else if (PlayerPrefs.HasKey("BadEnding"))
        {
            cutSceneImage.sprite = badScene;
            storyText.text = badText;
            PlayerPrefs.DeleteAll();
        }
    }

    private IEnumerator ShowStoryTextCoroutine()
    {
        float timer = 0f;
        while (timer < 1f) 
        {
            timer += Time.deltaTime;
            float alpha = timer / 1f;
            storyText.alpha = alpha;
            yield return null;
        }
    }

    private IEnumerator TurnWhiteCoroutine()
    {
        float timer = 0f;
        Color initialColor = cutSceneImage.color;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            float t = timer / 1f;
            Color lerpedColor = Color.Lerp(initialColor, Color.white, t);
            cutSceneImage.color = lerpedColor;
            yield return null;
        }
        cutSceneImage.color = Color.white;
    }

    private IEnumerator TurnBlackCoroutine()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before turning black

        float timer = 0f;
        Color initialColor = cutSceneImage.color;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            float t = timer / 1f;
            Color lerpedColor = Color.Lerp(initialColor, Color.black, t);
            cutSceneImage.color = lerpedColor;
            yield return null;
        }
        cutSceneImage.color = Color.black;
    }
}
