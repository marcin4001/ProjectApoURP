using UnityEngine;

public class FadeController : MonoBehaviour
{
    public static FadeController instance;
    public Animator animator;
    public string fadeInParam = "FadeIn";
    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    public bool IsFadeIn()
    {
        return animator.GetBool(fadeInParam);
    }

    public void SetFadeIn(bool value)
    {
        animator.SetBool(fadeInParam, value);
    }
}
