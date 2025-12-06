using UnityEngine;

public class MapSignController : MonoBehaviour
{
    public static MapSignController instance;
    [SerializeField] private RectTransform playerSign;
    [SerializeField] private float minDistanceToPlayer = 70f;
    [SerializeField] private MapSignObject[] mapSignObjects;
    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        mapSignObjects = FindObjectsByType<MapSignObject>(FindObjectsSortMode.None);
    }

    public void UpdateMapSigns()
    {
        foreach (MapSignObject mapSignObject in mapSignObjects)
        {
            float distance = mapSignObject.GetDistanceTo(playerSign);
            if (distance <= minDistanceToPlayer)
                mapSignObject.SetUnexplored();
        }
    }

    public RectTransform GetPlayerSign()
    {
        return playerSign;
    }
}
