using System.Collections;
using UnityEngine;

public class CellGridMapCombat : MonoBehaviour
{
    [SerializeField] private RectTransform center;
    [SerializeField] private bool isCombatTrigger = false;
    [SerializeField] private CellGridMapButton mapButton;
    private float minDistance = 3f;

    private void Start()
    {
        GameObject obj = new GameObject("Center", typeof(RectTransform));
        center = obj.GetComponent<RectTransform>();
        center.SetParent(transform, false);
        mapButton = GetComponent<CellGridMapButton>();
        StartCoroutine(PlayerIsNearonStart());
    }

    private IEnumerator PlayerIsNearonStart()
    {
        yield return new WaitForEndOfFrame();
        if(isCombatTrigger)
        {
            float distance = Vector3.Distance(center.transform.position, MapSignController.instance.GetPlayerSign().position);
            if (distance < minDistance)
            {
                isCombatTrigger = false;
            }
        }
    }


    public bool TriggerCombat()
    {
        if(!isCombatTrigger)
            return false;
        float distance = Vector3.Distance(center.transform.position, MapSignController.instance.GetPlayerSign().position);
        Debug.Log(distance);
        if(distance < minDistance)
        {
            isCombatTrigger = false;
            mapButton.SetNextScene();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetIsCombat(bool value)
    {
        isCombatTrigger = value;
    }
}
