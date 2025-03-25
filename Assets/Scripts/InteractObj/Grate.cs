using UnityEngine;

public class Grate : MonoBehaviour
{
    [SerializeField] private string openParam = "Open";
    [SerializeField] private string openOnStartParam = "OpenOnStart";
    [SerializeField] private bool openOnStart = false;
    [SerializeField] private int questID;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (openOnStart)
        {
            animator.SetTrigger(openOnStartParam);
            return;
        }
        if(QuestController.instance != null)
        {
            if(QuestController.instance.Complete(questID))
            {
                animator.SetTrigger(openOnStartParam);
            }
        }
    }

    public void Open()
    {
        animator.SetTrigger(openParam);
    }
}
