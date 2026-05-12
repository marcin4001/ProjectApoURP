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
            case InteractActionType.ActiveStove:
                ActiveStove(); break;
            case InteractActionType.ResetTrade:
                ResetTrade(); break;
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

    public void ActiveStove()
    {
        InactiveStove inactiveStove = FindFirstObjectByType<InactiveStove>();
        if (inactiveStove != null)
        {
            inactiveStove.ActiveStove();
        }
    }

    public void ResetTrade()
    {
        if(ListOffers.instance == null)
            return;
        ListOffers.instance.ResetTrade();
    }
}

public enum InteractActionType
{
    OpenGrate, SwitchOnTV, ActiveStove, ResetTrade
}


