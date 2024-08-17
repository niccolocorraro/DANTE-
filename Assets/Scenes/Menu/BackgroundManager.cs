using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    [Header("Backgrounds")]
    public CanvasGroup background1;
    public CanvasGroup background2;

    [Header("Transition Settings")]
    public float fadeDuration = 1.5f;  // Duration of the background crossfade
    public float frameRate = 12f;      // Desired frame rate for retro effect

    private float frameTime;

    void Start()
    {
        frameTime = 1f / frameRate;
        background2.alpha = 0f;  // Ensure the second background is initially invisible
    }

    public IEnumerator CrossfadeBackgrounds()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += frameTime;  // Increment time by the frame time for retro effect
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            background2.alpha = alpha;
            background1.alpha = 1f - alpha;

            yield return new WaitForSeconds(frameTime);  // Wait for the next frame
        }

        background2.alpha = 1f;
        background1.alpha = 0f;

        // Notify that background transition is complete
        SceneAnimator.Instance.OnBackgroundTransitionComplete();
    }
}
