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
            case InteractActionType.SwitchOnTV:
                SwitchOnTV(); break;
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

    public void SwitchOnTV()
    {
        TVScreen screen = FindFirstObjectByType<TVScreen>();
        if (screen != null)
        {
            screen.SwitchOnTV();
        }
    }
}

public enum InteractActionType
{
    OpenGrate, SwitchOnTV
}


