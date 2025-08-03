using System.Collections;
using UnityEngine;

public class TVScreen : MonoBehaviour
{
    [SerializeField] private MeshRenderer m_Renderer;
    [SerializeField] private Material switchOffMat;
    [SerializeField] private Material[] screens;
    [SerializeField] private float changeScreenTime = 5f;
    [SerializeField] private bool switchOnStart = false;
    void Start()
    {
        if (switchOnStart)
        {
            StartCoroutine(SwitchScreen());
            return;
        }
    }

    private IEnumerator SwitchScreen()
    {
        int index = 0;
        while (true)
        {
            index++;
            if (index >= screens.Length)
                index = 0;
            m_Renderer.material = screens[index];
            yield return new WaitForSeconds(changeScreenTime);
        }
    }
}
