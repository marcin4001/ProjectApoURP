using UnityEngine;

public class InDevManager : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.End))
        {
            GameParam.instance.inDev = !GameParam.instance.inDev;
        }
    }
}
