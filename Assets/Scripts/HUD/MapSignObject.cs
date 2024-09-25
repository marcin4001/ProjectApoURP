using UnityEngine;
using UnityEngine.UI;

public class MapSignObject : MonoBehaviour
{
    [SerializeField] private string locationName;
    [SerializeField] private MapSignState state;
    [SerializeField] private Sprite ExploredSign;
    [SerializeField] private Sprite UnexploredSign;
    private CanvasGroup group;
    private Image image;
    private RectTransform rectTransform;
    void Start()
    {
        state = GameParam.instance.GetMapSignState(locationName);
        group = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        if(state == MapSignState.Hidden)
        {
            group.alpha = 0f;
            group.blocksRaycasts = false;
        }
        else
        {
            group.alpha = 1f;
            group.blocksRaycasts = true;
        }
        image = GetComponent<Image>();
        if(state == MapSignState.Explored)
            image.overrideSprite = ExploredSign;
        if(state == MapSignState.Unexplored)
            image.overrideSprite = UnexploredSign;
            
    }

    public void SetUnexplored()
    {
        if (state == MapSignState.Explored)
            return;
        state = MapSignState.Unexplored;
        GameParam.instance.SetMapSignState(locationName, state);
        image.overrideSprite = UnexploredSign;
        group.alpha = 1f;
        group.blocksRaycasts = true;
    }

    public void SetExplored()
    {
        if (state == MapSignState.Explored)
            return;
        state = MapSignState.Explored;
        GameParam.instance.SetMapSignState(locationName, state);
        image.overrideSprite = ExploredSign;
        group.alpha = 1f;
        group.blocksRaycasts = true;
    }

    public float GetDistanceTo(RectTransform _rectTransform)
    {
        return Vector2.Distance(rectTransform.anchoredPosition, _rectTransform.localPosition);
    }
}
