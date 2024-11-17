using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
[Range(0.0f, 24f)] public float hour = 12;
public Transform sun;

public float hourSpeed = 10f;

private float sunX;

void Update()
{
    hour += Time.deltaTime * (24/(60 * hourSpeed));

    if(hour > 24)
    {
        hour = 0;
    }

    SunRotation();
}

void SunRotation()
{
    sunX = 15 * hour;
    sun.localEulerAngles = new Vector3(sunX, 0, 0); 

    Color skyColor = RenderSettings.ambientSkyColor;

    float intensity = 0.3f;   

    if(hour >= 0 && hour < 12)
    {
        RenderSettings.fogColor = Color.Lerp(Color.black, skyColor, (hour / 12));
        intensity = Mathf.Lerp(0.3f, 1f, (hour / 12));
    }
    else if(hour >= 12 && hour < 24)
    {
        RenderSettings.fogColor = Color.Lerp(skyColor, Color.black, ((hour - 12) / 12));
        intensity = Mathf.Lerp(1f, 0.3f, ((hour - 12) / 12));
    }

    RenderSettings.ambientIntensity = intensity;
    RenderSettings.reflectionIntensity = intensity;
}
}