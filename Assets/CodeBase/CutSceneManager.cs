using System.Collections;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup cutSceneGroup;
    [SerializeField] private float fadeDuration = 1f;

   

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(FadeInCoroutine());
    }
    

    private IEnumerator FadeInCoroutine()
    {
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f));

        yield return new WaitForSeconds(1f);
        cutSceneGroup.gameObject.SetActive(true);

        yield return StartCoroutine(FadeCanvasGroup(cutSceneGroup, 0f, 1f));
        
        yield return StartCoroutine(FadeCanvasGroup(cutSceneGroup, 1f, 0f));
        
        AllServices.Container.Single<ISceneLoaderService>().LoadScene(3);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            group.alpha = alpha;
            yield return null;
        }
        group.alpha = endAlpha;
    }
}
