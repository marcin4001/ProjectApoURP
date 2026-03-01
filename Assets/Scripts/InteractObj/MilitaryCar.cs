using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MilitaryCar : MonoBehaviour, IUsableObj
{
    [SerializeField] private SlotItem repairKit;
    [SerializeField] private SlotItem map;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private bool isRepair = false;
    [SerializeField] private AudioClip repairClip;
    [SerializeField] private AudioClip jeepClip;
    [SerializeField] private int idPickUp;
    [SerializeField] private string nextScene;
    [SerializeField] private Vector2 posOnMap;
    [SerializeField] private ActionDialogue actionDialogue;
    private AudioSource source;
    private PlayerController playerController;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        playerController = FindFirstObjectByType<PlayerController>();
        isRepair = PickUpObjList.instance.ExistOnList(idPickUp);
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
        if(!isRepair)
        {
            bool result = Inventory.instance.PlayerHaveItem(repairKit.GetItem().id);
            if (!result)
            {
                HUDController.instance.AddConsolelog("The car is broken.");
                HUDController.instance.AddConsolelog("You need a repair kit.");
                return;
            }
            Inventory.instance.RemoveItem(repairKit);
            StartCoroutine(Repair());
        }
        else
        {
            bool haveMap = Inventory.instance.PlayerHaveItem(map.GetItem().id);
            if (!haveMap)
            {
                HUDController.instance.AddConsolelog("You need a map to know");
                HUDController.instance.AddConsolelog("where to go");
                return;
            }
            StartCoroutine(StartEngine());
        }
    }


    private IEnumerator Repair()
    {
        yield return new WaitForEndOfFrame();
        playerController.SetBlock(true);
        if (source != null)
        {
            source.clip = repairClip;
            source.Play();
        }
        yield return new WaitForSeconds(1);
        isRepair = true;
        playerController.SetBlock(false);
        HUDController.instance.AddConsolelog("The car has been repaired.");
        PickUpObjList.instance.AddIdToList(idPickUp);
    }

    private IEnumerator StartEngine()
    {
        CameraMovement.instance.SetBlock(true);
        FadeController.instance.SetFadeIn(true);
        playerController.SetBlock(true);
        yield return new WaitForSeconds(2f);
        if (source != null)
        {
            source.spatialBlend = 0f;
            source.clip = jeepClip;
            source.Play();
        }
        GameParam.instance.mapPosition = posOnMap;
        actionDialogue.Execute();
        yield return new WaitForSeconds(3.4f);
        SceneManager.LoadScene(nextScene);
    }
}
