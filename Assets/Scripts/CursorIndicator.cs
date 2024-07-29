using UnityEngine;

public class CursorIndicator : MonoBehaviour
{
    [SerializeField] private Material reachableMat;
    [SerializeField] private Material unreachableMat;
    private MeshRenderer _renderer;
    void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetReachableMaterial()
    {
        _renderer.material = reachableMat;
    }

    public void SetUnreachableMaterial()
    {
        _renderer.material = unreachableMat;
    }

    public void SetVibility(bool value)
    {
        _renderer.enabled = value;
    }
}
