using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BaseDoorOutside : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearpoint;
    [SerializeField] private GameObject mainObj;
    [SerializeField] private string openParam = "Open";
    [SerializeField] private string openStartParam = "OpenStart";
    [SerializeField] private int keyID;
    [SerializeField] private string sceneName = "LabsMap";
    [SerializeField] private bool inOutside = false;
    [SerializeField] private bool saveOpened = false;
    [SerializeField] private string doorID;
    private bool isLock = true;
    private Animator anim;
    private Collider col;
    private PlayerController playerController;
    private void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        playerController = FindFirstObjectByType<PlayerController>();
        if(saveOpened)
        {
            if(GameParam.instance.DoorOnList(doorID))
            {
                col.enabled = false;
                anim.SetTrigger(openStartParam);
            }
        }
    }

    public bool CanUse()
    {
        if(!isLock)
            return false;
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return mainObj;
    }

    public Vector3 GetNearPoint()
    {
        return nearpoint.position;
    }

    public void Use()
    {
        Debug.Log("isLock = " + isLock);
        if (isLock)
            return;
        anim.SetTrigger(openParam);
        col.enabled = false;
        if (inOutside)
            StartCoroutine(LoadScene());
        if(saveOpened)
        {
            GameParam.instance.AddOpenDoor(doorID);
        }
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

    public bool CheckKey(int _keyID)
    {
        return keyID == _keyID;
    }

    public void Unlock()
    {
        isLock = false;
    }

    public bool IsLocked()
    {
        return isLock;
    }
}
