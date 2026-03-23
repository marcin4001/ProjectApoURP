using UnityEngine;

public class InactiveStove : MonoBehaviour
{
    [SerializeField] private Stove stove;
    
    public void ActiveStove()
    {
        if (stove != null)
            stove.InsertBattery();
    }
}
