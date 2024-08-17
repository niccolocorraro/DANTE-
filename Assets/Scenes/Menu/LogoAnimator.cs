using UnityEngine;

public class LogoAnimator : MonoBehaviour
{
    [Header("Logo Animation Settings")]
    public GameObject logo;  // The logo GameObject

    private Animator logoAnimator;

    void Start()
    {
        logoAnimator = logo.GetComponent<Animator>();

        // Start the logo animation immediately
        PlayLogoAnimation();
    }

    public void PlayLogoAnimation()
    {
        if (logoAnimator != null)
        {
            logoAnimator.Play("LogoAnimation"); // The name should match your animation
        }

        // Notify that the logo animation is complete
        SceneAnimator.Instance.OnLogoAnimationComplete();
    }
}
