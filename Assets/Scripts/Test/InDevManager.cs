using UnityEngine;

public class InDevManager : MonoBehaviour
{
    void Update()
    {
        if(GameParam.instance.inDemo)
            return;
        if(Input.GetKeyDown(KeyCode.End))
        {
            GameParam.instance.inDev = !GameParam.instance.inDev;
        }
    }
}
