using UnityEngine;
using System.Collections;

public class UITransition : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup[] uiElements;

    [Header("Transition Settings")]
    public float fadeDuration = 1f;
    public float frameRate = 12f;
    public float delayBetweenElements = 0.2f;
    public float waitTransition = 0f;  // Optional delay before starting the transition

    public int hasBeenAnimated;
    private float frameTime;

    

    void Start()
    {
        frameTime = 1f / frameRate;
        for(int i = 1; i < uiElements.Length; i++)
        {
            uiElements[i].alpha = 0f;  // Initially invisible
        }
    }

    // Public method to be called by a button (with wait time)
    public void StartFadeInUIElementsWithWait()
    {
        StartCoroutine(FadeInUIElements(waitTransition));
    }

    // Public method to be called by a button (without wait time)
    public void StartFadeInUIElementsNoWait()
    {
        StartCoroutine(FadeInUIElements(0f));
    }

    // Public method to be called by a button (with wait time)
    public void StartFadeOutUIElementsWithWait()
    {
        StartCoroutine(FadeOutUIElements(waitTransition));
    }

    // Public method to be called by a button (without wait time)
    public void StartFadeOutUIElementsNoWait()
    {
        StartCoroutine(FadeOutUIElements(0f));
    }

    public IEnumerator FadeInUIElements(float waitTime)
    {
        // Wait before starting the fade-in transition if waitTime is greater than 0
        if (waitTime > 0)
        {
            yield return new WaitForSeconds(waitTime);
        }

        for (int i = hasBeenAnimated; i < uiElements.Length; i++)
        {
            yield return StartCoroutine(FadeInElement(uiElements[i]));
            yield return new WaitForSeconds(delayBetweenElements);
        }

        hasBeenAnimated = 0;
    }

    public IEnumerator FadeOutUIElements(float waitTime)
    {
        // Wait before starting the fade-out transition if waitTime is greater than 0
        if (waitTime > 0)
        {
            yield return new WaitForSeconds(waitTime);
        }

        for (int i = 0; i < uiElements.Length; i++)
        {
            yield return StartCoroutine(FadeOutElement(uiElements[i]));
            yield return new WaitForSeconds(delayBetweenElements);
        }
    }

    private IEnumerator FadeInElement(CanvasGroup element)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += frameTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            element.alpha = alpha;

            yield return new WaitForSeconds(frameTime);
        }

        element.alpha = 1f;
        element.interactable = true;
        element.blocksRaycasts = true;
    }

    private IEnumerator FadeOutElement(CanvasGroup element)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += frameTime;
            float alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));

            element.alpha = alpha;

            yield return new WaitForSeconds(frameTime);
        }

        element.alpha = 0f;
        element.interactable = false;
        element.blocksRaycasts = false;
    }

}
