using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMapTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName = "DemoMenu";
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
