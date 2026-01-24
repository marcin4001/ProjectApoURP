using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private string isRunParam = "isRun";
    [SerializeField] private string doorInteractParam = "DoorInteract";
    [SerializeField] private string shotParam = "Shot";
    [SerializeField] private string reloadParam = "Reload";
    [SerializeField] private string attackParam = "Attack";
    [SerializeField] private string eatingParam = "Eating";
    [SerializeField] private string drinkingParam = "Drinking";
    [SerializeField] private string takeDamageParam = "TakeDamage";
    [SerializeField] private string isDeathParam = "isDeath";
    [SerializeField] private string punchParam = "Punch";
    [SerializeField] private string handGunLayer = "HandGun";
    [SerializeField] private string rifleLayer = "Rifle";
    [SerializeField] private float doorInteractTime = 2.5f;
    [SerializeField] private float eatingTime = 2.16f;
    [SerializeField] private float drinkingTime = 2;
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSpeedLocomotion(float speed)
    {
        animator.SetFloat(speedParam, speed);
    }

    public void SetIsRun(bool isRun)
    {
        animator.SetBool(isRunParam, isRun);
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

    public void Eating()
    {
        animator.SetTrigger(eatingParam);
    }

    public void Drink()
    {
        animator.SetTrigger(drinkingParam);
    }

    public void TakeDamage()
    {
        animator.SetTrigger(takeDamageParam);
    }

    public void SetDeath()
    {
        animator.SetTrigger(isDeathParam);
    }

    public void Punch()
    {
        animator.SetTrigger(punchParam);
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

    public float GetEatingTime()
    {
        return eatingTime;
    }

    public float GetDrinkingTime()
    {
        return drinkingTime;
    }

}
