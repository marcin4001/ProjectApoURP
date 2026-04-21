using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrugsFX : MonoBehaviour
{
    [SerializeField] private float duration = 20f;
    [SerializeField] private Volume volume;
    [SerializeField] private bool active = false;
    private ColorAdjustments colorAdjustments;
    void Start()
    {
        volume = FindFirstObjectByType<Volume>();
        volume.profile.TryGet(out colorAdjustments);
    }

    public void StartEffect()
    {
        if(active)
            return;
        StartCoroutine(Effect());
    }

    public IEnumerator Effect()
    {
        active = true;
        float time = 0;
        float startHue = colorAdjustments.hueShift.value;
        float startSat = colorAdjustments.saturation.value;
        colorAdjustments.saturation.value = 50f;

        while (time < duration)
        {
            time += Time.deltaTime;
            colorAdjustments.hueShift.value += 200f * Time.deltaTime;
            if (colorAdjustments.hueShift.value >= 180f)
                colorAdjustments.hueShift.value = -180f;
            Debug.Log(colorAdjustments.hueShift.value);
            yield return new WaitForEndOfFrame();
        }

        float timeReturn = 0f;
        float resetTime = 2f;
        float currentHue = colorAdjustments.hueShift.value;
        float currentSat = colorAdjustments.saturation.value;

        while (timeReturn < resetTime)
        {
            timeReturn += Time.deltaTime;
            float lerp = timeReturn / resetTime;

            colorAdjustments.hueShift.value = Mathf.Lerp(currentHue, startHue, lerp);
            colorAdjustments.saturation.value = Mathf.Lerp(currentSat, startSat, lerp);
            yield return new WaitForEndOfFrame();
        }
        colorAdjustments.hueShift.value = startHue;
        colorAdjustments.saturation.value = startSat;
        active = false;
    }
}
