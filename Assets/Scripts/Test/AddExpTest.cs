using UnityEngine;

public class AddExpTest : MonoBehaviour
{
    [SerializeField] private int exp;

    void Update()
    {
        if(!GameParam.instance.inDev)
            return;
        if(Input.GetKeyDown(KeyCode.E))
        {
            GameParam.instance.AddExp(exp);
        }
    }
}
