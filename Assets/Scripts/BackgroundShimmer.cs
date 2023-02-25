using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class BackgroundShimmer : MonoBehaviour
{
    public PostProcessVolume Volume;
    Bloom bloomer;
    float intensity;
    bool shouldBloom;

    public AnimationCurve BloomWave;

    // Start is called before the first frame update
    void Start()
    {
        Volume.profile.TryGetSettings(out bloomer);

        EnableBloom(true, 3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shouldBloom){
            bloomer.intensity.value = (Mathf.Sin(Time.realtimeSinceStartup) + 4) * intensity;
        }
    }

    public void EnableBloom(bool bloom, float scale){
        shouldBloom = bloom;
        intensity = scale;
    }
}
