using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Material m_material;
    private Coroutine coroutine;
    void Start()
    {
        m_material = GetComponent<Renderer>().material;
        //StartCoroutine(Flicker());
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private void OnEnable()
    {
        m_material = GetComponent<Renderer>().material;
        coroutine = StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            m_material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.6f);
            m_material.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds (0.4f);
            m_material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.5f);
            m_material.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(2f);
        }
    }
}
