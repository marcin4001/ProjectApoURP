using System.Collections;
using UnityEngine;

public class CattleDemo : MonoBehaviour
{
    [SerializeField] private string eatingParam = "Eating";
    [SerializeField] private bool isEating = false;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        while (true)
        {
            animator.SetBool(eatingParam, isEating);
            yield return new WaitForSeconds(5f);
            isEating = !isEating;
        }
    }

}
