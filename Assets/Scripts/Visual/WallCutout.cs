using System.Collections.Generic;
using UnityEngine;

public class WallCutout : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Camera cam;
    [Space]
    [SerializeField] private float cutoutSize = 0.1f;
    [SerializeField] private float falloffSize = 0.05f;
    [SerializeField] private string cutoutPositionProp = "_Cutout_Position";
    [SerializeField] private string cutoutSizeProp = "_Cutout_Size";
    [SerializeField] private string falloffSizeProp = "_Falloff_Size";
    [Space]
    [SerializeField] private bool isDebug = false;

    
    private List<MeshRenderer> renderers = new List<MeshRenderer> ();

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        cutoutSize = GameParam.instance.cutoutSize;
    }

    void Update()
    {
        transform.position = player.GetCenterPosition() - Vector3.forward * 3f - Vector3.right * 3f;

        Vector2 cutoutPos = cam.WorldToViewportPoint(player.GetCenterPosition());
        Vector3 offsetCam = transform.position - player.GetCenterPosition();
        Vector3 direction = offsetCam.normalized;
        float baseDistance = offsetCam.magnitude;

        int rayCount = 15;
        float spacing = 0.1f; // Odległość między promieniami (lewo/prawo)
        Vector3 right = Vector3.Cross(Vector3.up, direction); // wektor w prawo względem kierunku patrzenia

        List<RaycastHit> allHits = new List<RaycastHit>();

        for (int i = 0; i < rayCount; i++)
        {
            float offsetAmount = (i - (rayCount - 1) / 2f) * spacing;
            Vector3 rayOrigin = player.GetCenterPosition() + right * offsetAmount;

            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, direction, baseDistance);
            allHits.AddRange(hits);

            if (isDebug)
            {
                Debug.DrawRay(rayOrigin, direction * baseDistance, i == rayCount / 2 ? Color.yellow : Color.red, 0.1f);
            }
        }

        // Reset renderów z poprzedniej klatki
        if (renderers.Count > 0)
        {
            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer == null)
                    continue;
                foreach (Material mat in renderer.materials)
                    mat.SetFloat(cutoutSizeProp, 0f);
                renderer.gameObject.layer = 7;
            }
            renderers.Clear();
        }

        // Obsługa trafień
        foreach (RaycastHit hit in allHits)
        {
            MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
                continue;

            float distanceToWall = Vector3.Distance(transform.position, hit.point);
            if (distanceToWall >= baseDistance)
                continue;

            foreach (Material mat in meshRenderer.materials)
            {
                mat.SetVector(cutoutPositionProp, cutoutPos);
                mat.SetFloat(cutoutSizeProp, cutoutSize);
                mat.SetFloat(falloffSizeProp, falloffSize);
            }

            if (meshRenderer.tag != "Door" && meshRenderer.tag != "Item" && meshRenderer.tag != "Obstacle")
                meshRenderer.gameObject.layer = 10;

            if (!renderers.Contains(meshRenderer))
                renderers.Add(meshRenderer);
        }

        if (isDebug)
            Debug.DrawLine(player.GetCenterPosition(), transform.position, Color.green, 1.0f);
    }

}
