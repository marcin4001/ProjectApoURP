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

    void Update()
    {
        transform.position = player.GetCenterPosition() - Vector3.forward * 3f - Vector3.right * 3f;
        Vector2 cutoutPos = cam.WorldToViewportPoint(player.GetCenterPosition());
        Vector3 offsetCam =  transform.position - player.GetCenterPosition();
        RaycastHit[] hits = Physics.SphereCastAll(player.GetCenterPosition(), 0.2f, offsetCam, offsetCam.magnitude);

        if (renderers.Count > 0)
        {
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.material.SetFloat(cutoutSizeProp, 0f);
                renderer.gameObject.layer = 7;
            }
            renderers.Clear();
        }

        foreach (RaycastHit hit in hits)
        {
            MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.material.SetVector(cutoutPositionProp, cutoutPos);
                meshRenderer.material.SetFloat(cutoutSizeProp, cutoutSize);
                meshRenderer.material.SetFloat(falloffSizeProp, falloffSize);
                if(meshRenderer.tag != "Door")
                    meshRenderer.gameObject.layer = 10;
                renderers.Add(meshRenderer);
            }
        }
        if(isDebug)
            Debug.DrawLine(player.GetCenterPosition(), transform.position, Color.green, 1.0f);
    }
}
