using UnityEngine;

[CreateAssetMenu(fileName = "InteractAction", menuName = "Dialogue/InteractAction")]
public class InteractAction : ActionDialogue
{
    public InteractActionType type = InteractActionType.OpenGrate;
    public override void Execute()
    {
        switch(type)
        {
            case InteractActionType.OpenGrate:
                OpenGrate(); break;
            default:
                break;
        }
    }

    private void OpenGrate()
    {
        Grate grate = FindFirstObjectByType<Grate>();
        if (grate != null)
        {
            grate.Open();
        }
    }
}

public enum InteractActionType
{
    OpenGrate
}


