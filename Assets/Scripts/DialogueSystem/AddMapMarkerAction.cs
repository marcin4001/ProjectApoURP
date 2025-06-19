using UnityEngine;

[CreateAssetMenu(fileName = "AddMapMarkerAction", menuName = "Dialogue/AddMapMarkerAction")]
public class AddMapMarkerAction : ActionDialogue
{
    public string markerName;
    public override void Execute()
    {
        if(GameParam.instance.GetMapSignState(markerName) == MapSignState.Hidden)
        {
            GameParam.instance.SetMapSignState(markerName, MapSignState.Unexplored);
        }
    }
}
