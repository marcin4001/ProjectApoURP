using UnityEngine;
using UnityEngine.Events;

public class TimeGame : MonoBehaviour
{
    public static TimeGame instance;
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private float sunriseTime = 4f;
    [SerializeField] private float sunsetTime = 20f;
    [SerializeField] private UnityEvent OnSunrise;
    [SerializeField] private UnityEvent OnSunset;
    private bool isSunRise = false;
    private bool isSunset = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(GameParam.instance != null)
            currentTime = GameParam.instance.currentTime;
    }

    void Update()
    {
        currentTime += (Time.deltaTime / 60);
        if (currentTime >= 24f)
        {
            currentTime -= 24f;
            isSunRise = false;
            isSunset = false;
            GameParam.instance.AddDay();
        }

        if(currentTime >= (sunriseTime + 0.5f) && !isSunRise) 
        {
            Debug.Log("OnSunRise");
            OnSunrise.Invoke();
            isSunRise = true;
        }

        if(currentTime >= (sunsetTime - 0.5f) && !isSunset)
        {
            Debug.Log("OnSunSet");
            OnSunset.Invoke();
            isSunset = true;
        }
    }

    public void AddHours(float hours)
    {
        currentTime += hours;
        if (currentTime >= 24f)
            currentTime -= 24f;
    }

    public string GetCurrentTimeString()
    {
        int hour = Mathf.FloorToInt(currentTime);
        float fractionalHour = currentTime - hour;
        int minutes = Mathf.FloorToInt(fractionalHour * 60);

        string period = "AM";
        if(hour >= 12)
        {
            period = "PM";
            if(hour >= 13)
                hour -= 12;
        }
        if (hour == 0)
            hour = 12;

        return $"{hour.ToString("D2")}:{minutes.ToString("D2")} {period}";
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
