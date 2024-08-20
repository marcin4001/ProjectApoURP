using System.Collections;
using UnityEngine;

public class Bed : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    private PlayerController player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
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
       StartCoroutine(Sleeping());
    }

    private IEnumerator Sleeping()
    {
        CameraMovement.instance.SetBlock(true);
        FadeController.instance.SetFadeIn(true);
        player.SetBlock(true);
        yield return new WaitForSeconds(2f);
        TimeGame.instance.AddHours(6);
        FadeController.instance.SetFadeIn(false);
        yield return new WaitForSeconds(1.5f);
        CameraMovement.instance.SetBlock(false);
        player.SetBlock(false);
    }
}
