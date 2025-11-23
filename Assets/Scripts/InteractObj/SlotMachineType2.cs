using System.Collections;
using UnityEngine;

public class SlotMachineType2 : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string actionParam = "Action";
    [SerializeField] private bool active = false;

    public void Use()
    {
        if (active)
            return;
        StartCoroutine(DrawSlot());
    }

    private IEnumerator DrawSlot()
    {
        active = true;
        anim.SetTrigger(actionParam);
        yield return new WaitForSeconds(1.5f);
        active = false;
    }
}
