using UnityEngine;

public class TestConsole : MonoBehaviour
{
    [SerializeField] private string log;
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return))
        {
            HUDController.instance.AddConsolelog(log);
        }
    }
}
