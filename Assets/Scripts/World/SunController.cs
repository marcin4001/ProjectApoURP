using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SunController : MonoBehaviour
{
    [SerializeField] private Light sunLight;
    [SerializeField] private Light playerLight;
    [SerializeField] private float sunriseTime = 6f;
    [SerializeField] private float sunsetTime = 18f;
    private Transform sunTransform;
    private TimeGame gameTime;
    private ColorAdjustments colorAdjustments;
    private Volume volume;
    private float nightPostExposure = -1.2f;
    private float dayPostExposure = 0f;
    void Start()
    {
        gameTime = FindFirstObjectByType<TimeGame>();
        sunTransform = sunLight.transform;
        playerLight = GameObject.FindGameObjectWithTag("PlayerLight").GetComponent<Light>();
        //playerLight.intensity = 0f;
        volume = FindAnyObjectByType<Volume>();
        if(volume != null && volume.profile.TryGet(out colorAdjustments))
        {
            Debug.Log("Color Adjustments found");
        }
    }


    void LateUpdate()
    {
        float timeGame = gameTime.GetCurrentTime();
        float normalizedTime = 0f;
        float sunRotationX = 0;
        if(timeGame >= sunriseTime && timeGame <= sunsetTime)
        {
            normalizedTime = (timeGame - sunriseTime) / (sunsetTime - sunriseTime);
            sunRotationX = Mathf.Lerp(0f, 180f, normalizedTime);
        }

        if(timeGame >= sunriseTime && timeGame <= sunriseTime + 1f) 
        {
            playerLight.intensity = Mathf.Lerp(3f, 0f, (timeGame - sunriseTime) / 1f);
            sunLight.intensity = Mathf.Lerp(0f, 1f, (timeGame - sunriseTime) / 1f);
            if(colorAdjustments != null)
                colorAdjustments.postExposure.value = Mathf.Lerp(nightPostExposure, dayPostExposure, (timeGame - sunriseTime) / 1f);
        }
        else if(timeGame >= sunsetTime - 1f && timeGame <= sunsetTime)
        {
            playerLight.intensity = Mathf.Lerp(0f, 3f, (timeGame - (sunsetTime -1f)) / 1f);
            sunLight.intensity = Mathf.Lerp(1f, 0f, (timeGame - (sunsetTime - 1f)) / 1f);
            if (colorAdjustments != null)
                colorAdjustments.postExposure.value = Mathf.Lerp(dayPostExposure, nightPostExposure, (timeGame - (sunsetTime - 1f)) / 1f);
        }
        else if(timeGame >= sunsetTime || timeGame < sunriseTime)
        {
            playerLight.intensity = 3f;
            sunLight.intensity = 0f;
            if (colorAdjustments != null)
                colorAdjustments.postExposure.value = nightPostExposure;
        }
        else
        {
            playerLight.intensity = 0f;
            sunLight.intensity = 1f;
            if (colorAdjustments != null)
                colorAdjustments.postExposure.value = dayPostExposure;
        }
        

        sunTransform.rotation = Quaternion.Euler(sunRotationX % 360, 270f, 0f);
    }
}
