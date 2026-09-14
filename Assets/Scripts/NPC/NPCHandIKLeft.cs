using UnityEngine;

public class NPCHandIKLeft : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField][Range(0, 1)] private float weight = 1f;
    private void OnAnimatorIK(int layerIndex)
    {
        if (animator == null && target == null)
            return;
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, target.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, target.rotation);
    }
}
