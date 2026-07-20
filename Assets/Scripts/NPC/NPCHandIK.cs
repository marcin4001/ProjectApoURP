using UnityEngine;

public class NPCHandIK : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField][Range(0, 1)] private float weight = 1f;
    private void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("działa1");
        if (animator == null && target == null)
            return;
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);

        animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, target.rotation);
        Debug.Log("działa2");
    }

}
