using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{
    [SerializeField] private float timeLoadScene = 4f;
    [SerializeField] private bool anyKeyTrigger = false;
    void Start()
    {
        if (anyKeyTrigger)
        {
            StartCoroutine(AnyKeyTrigger());
        }
        else
        {
            StartCoroutine(LoadMenuScene());
        }
    }

    private IEnumerator LoadMenuScene()
    {
        yield return new WaitForSeconds(timeLoadScene);
        SceneManager.LoadScene(0);
    }

    private IEnumerator AnyKeyTrigger()
    {
        yield return new WaitForSeconds(1.5f);
        while(true)
        {
            if(Input.anyKey)
            {
                SceneManager.LoadScene(0);
            }
            yield return new WaitForEndOfFrame();
        }
        
    }
}
