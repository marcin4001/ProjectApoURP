using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private string doorInteractParam = "DoorInteract";
    [SerializeField] private string shotParam = "Shot";
    [SerializeField] private string reloadParam = "Reload";
    [SerializeField] private string attackParam = "Attack";
    [SerializeField] private string handGunLayer = "HandGun";
    [SerializeField] private string rifleLayer = "Rifle";
    [SerializeField] private float doorInteractTime = 2.5f;
    private Animator animator;
    void Awake()
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

    public void Shot()
    {
        animator.SetTrigger(shotParam);
    }

    public void Reload()
    {
        animator.SetTrigger(reloadParam);
    }

    public void Attack()
    {
        animator.SetTrigger(attackParam);
    }

    public void ActiveBaseLayer()
    {
        int handGunIndex = animator.GetLayerIndex(handGunLayer);
        int rifleIndex = animator.GetLayerIndex(rifleLayer);
        animator.SetLayerWeight(handGunIndex, 0f);
        animator.SetLayerWeight(rifleIndex, 0f);
    }

    public void ActiveHandGunLayer()
    {
        int handGunIndex = animator.GetLayerIndex(handGunLayer);
        int rifleIndex = animator.GetLayerIndex(rifleLayer);
        animator.SetLayerWeight(handGunIndex, 1f);
        animator.SetLayerWeight(rifleIndex, 0f);
    }

    public void ActiveRifleLayer()
    {
        int handGunIndex = animator.GetLayerIndex(handGunLayer);
        int rifleIndex = animator.GetLayerIndex(rifleLayer);
        animator.SetLayerWeight(handGunIndex, 0f);
        animator.SetLayerWeight(rifleIndex, 1f);
    }

    public float GetDoorInteractTime()
    {
        return doorInteractTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ActiveHandGunLayer();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ActiveRifleLayer();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ActiveBaseLayer();
        }


    }
}
