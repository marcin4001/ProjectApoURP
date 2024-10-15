using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform cameraPivotSpawn;
    void Start()
    {
        if(GameParam.instance.startGame)
        {
            GameParam.instance.startGame = false;
            return;
        }
        PlayerController controller = FindFirstObjectByType<PlayerController>();
        controller.GetAgent().Warp(transform.position);
        controller.transform.eulerAngles = transform.eulerAngles;
        if(cameraPivotSpawn == null)
        {
            CameraMovement.instance.transform.position = transform.position;
            return;
        }
        CameraMovement.instance.transform.position = cameraPivotSpawn.position;
    }
}
