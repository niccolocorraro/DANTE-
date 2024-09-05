using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SceneAnimator : MonoBehaviour
{
    public static SceneAnimator Instance { get; private set; }

    [Header("Background Transition")]
    public BackgroundManager backgroundManager;

    [Header("Logo Animation")]
    public LogoAnimator logoAnimator;

    [Header("UI Transition")]
    public UITransition uiTransition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(AnimateScene());
    }

    private IEnumerator AnimateScene()
    {
        // Step 1: Animate the background transition
        yield return StartCoroutine(backgroundManager.CrossfadeBackgrounds());

        // Step 2: Animate the logo
        if(logoAnimator != null){
            logoAnimator.PlayLogoAnimation();
            uiTransition.hasBeenAnimated = 1;


        // Step 3: Fade in the UI elements
        yield return StartCoroutine(uiTransition.FadeInUIElements(1f));
        } else { yield return StartCoroutine(uiTransition.FadeInUIElements(0f));}

        
    }

    public void OnBackgroundTransitionComplete()
    {
        // Callback for when background transition is complete, if needed
    }

    public void OnLogoAnimationComplete()
    {
        // Callback for when logo animation is complete, if needed
    }

     public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

     public void LoadCanti()
    {
        SceneManager.LoadScene(5);
    }
}
