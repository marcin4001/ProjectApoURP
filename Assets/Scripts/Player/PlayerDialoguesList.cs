using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialoguesList : MonoBehaviour
{
    [SerializeField] private List<string> texts = new List<string>();
    [SerializeField] private float time = 3f;
    public void SpawnText()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        foreach(string text in texts)
        {
            PlayerDialogues.instance.SetText(text);
            yield return new WaitForSeconds(time);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
