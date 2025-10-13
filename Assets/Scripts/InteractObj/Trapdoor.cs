using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Trapdoor : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private string openParam = "Open";
    [SerializeField] private Animator anim;
    [SerializeField] private bool withOutLoadScene = false;
    [SerializeField] private string sceneName;
    [SerializeField] private int ropeID;
    [SerializeField] private string doorID;
    [SerializeField] private bool needKey = false;
    private bool isLock = true;
    private PlayerController playerController;
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (GameParam.instance.DoorOnList(doorID))
        {
            isLock = false;
        }
    }
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
        if(isLock)
            return;
        anim.SetTrigger(openParam);
        if(!withOutLoadScene)
            StartCoroutine(LoadScene());
        GameParam.instance.AddOpenDoor(doorID);
    }

    public bool CheckRope(int _ropeID)
    {
        return ropeID == _ropeID;
    }

    public void ShowConsoleLog()
    {
        if(needKey)
        {
            HUDController.instance.AddConsolelog("You need a key.");
            return;
        }
        HUDController.instance.AddConsolelog("You need a rope.");
    }

    public void Unlock()
    {
        isLock = false;
        if(needKey)
            return;
        HUDController.instance.RemoveCurrentItem();
    }

    public bool IsLock()
    {
        return isLock;
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForEndOfFrame();
        if (CombatController.instance != null)
        {
            CombatController.instance.StopCombat();
        }
        playerController.SetBlock(true);
        yield return new WaitForSeconds(2);
        if (GameParam.instance != null)
        {
            GameParam.instance.UpdateParam();
            GameParam.instance.prevScene = SceneManager.GetActiveScene().name;
        }
        if (ListCabinet.instance != null)
            ListCabinet.instance.SaveCabinets();
        if (ListOffers.instance != null)
            ListOffers.instance.SaveOffers();
        SceneManager.LoadScene(sceneName);
    }
}
