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
        Debug.Log(GetCurrentTimeString());
    }

    public string GetCurrentTimeString()
    {
        int hour = Mathf.FloorToInt(currentTime);
        float fractionalHour = currentTime - hour;
        int minutes = Mathf.FloorToInt(fractionalHour * 60);
        return $"{hour.ToString("D2")}:{minutes.ToString("D2")}";
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
