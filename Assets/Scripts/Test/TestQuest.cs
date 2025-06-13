using System.Collections.Generic;
using UnityEngine;

public class TestQuest : MonoBehaviour
{
    [SerializeField] private List<ActionDialogue> actions = new List<ActionDialogue>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameParam.instance.inDev)
            return;
        if (Input.GetKeyUp(KeyCode.F9))
        {
            foreach(ActionDialogue action in actions)
            {
                action.Execute();
            }
        }
    }
}
