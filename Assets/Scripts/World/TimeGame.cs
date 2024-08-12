using UnityEngine;

public class TimeGame : MonoBehaviour
{
    [SerializeField] private float startTime = 6f;
    [SerializeField] private float currentTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += (Time.deltaTime / 60);
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
