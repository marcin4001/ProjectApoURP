using UnityEngine;

public class MapBlockerUI : MonoBehaviour
{
    void Start()
    {
        if(!GameParam.instance.inDemo)
            Destroy(gameObject);
        else
            Destroy(this);
    }

}
