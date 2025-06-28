using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform cameraPivotSpawn;
    [SerializeField] private int indexCabinetData = 0;
    [SerializeField] private bool importantPlace;
    [SerializeField] private string mapSignName;
    [SerializeField] private int indexTheme = 0;

    void Start()
    {
        MusicManager.instance.SetMaxVolume(GameParam.instance.maxVolumeTheme);
        MusicManager.instance.SetTheme(indexTheme);
        if(GameParam.instance.startGame)
        {
            GameParam.instance.startGame = false;
            return;
        }
        PlayerController controller = FindFirstObjectByType<PlayerController>();
        controller.GetAgent().Warp(transform.position);
        controller.transform.eulerAngles = transform.eulerAngles;
        ListCabinet.instance.indexCabinetData = indexCabinetData;

        if (importantPlace && GameParam.instance.GetMapSignState(mapSignName) != MapSignState.Explored)
        {
            GameParam.instance.SetMapSignState(mapSignName, MapSignState.Explored);
            GameParam.instance.AddExp(100);
        }
        if (cameraPivotSpawn == null)
        {
            CameraMovement.instance.transform.position = transform.position;
            return;
        }
        CameraMovement.instance.transform.position = cameraPivotSpawn.position;
        
    }
}
