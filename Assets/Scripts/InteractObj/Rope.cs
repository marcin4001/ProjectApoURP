using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rope : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private string sceneName;
    private PlayerController playerController;

    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }

    public Vector3 GetNearPoint()
    {
        return nearPoint.position;
    }

    public void Use()
    {
        StartCoroutine(LoadScene());
    }

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForEndOfFrame();
        if (CombatController.instance != null)
        {
            CombatController.instance.StopCombat();
        }
        playerController.SetBlock(true);
        yield return new WaitForSeconds(0.3f);
        if (GameParam.instance != null)
        {
            GameParam.instance.UpdateParam();
            GameParam.instance.prevScene = SceneManager.GetActiveScene().name;
        }
        if (ListCabinet.instance != null)
            ListCabinet.instance.SaveCabinets();
        if (ListOffers.instance != null)
            ListOffers.instance.SaveOffers();
        GameParam.instance.exitInside = true;
        SceneManager.LoadScene(sceneName);
    }
}
