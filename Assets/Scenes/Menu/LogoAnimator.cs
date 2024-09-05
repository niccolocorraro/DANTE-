using UnityEngine;

public class LogoAnimator : MonoBehaviour
{
    [Header("Logo Animation Settings")]
    public GameObject logo;  // The logo GameObject
    private Animator logoAnimator;

    public bool isAnimated;
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
            logoAnimator.SetBool("Animate", true);
            isAnimated = true;

        }

        // Notify that the logo animation is complete
        SceneAnimator.Instance.OnLogoAnimationComplete();
    }
}
