using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private string doorInteractParam = "DoorInteract";
    [SerializeField] private float doorInteractTime = 2.5f;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSpeedLocomotion(float speed)
    {
        animator.SetFloat(speedParam, speed);
    }

    public void DoorInteract()
    {
        animator.SetTrigger(doorInteractParam);
    }

    public float GetDoorInteractTime()
    {
        return doorInteractTime;
    }
}
