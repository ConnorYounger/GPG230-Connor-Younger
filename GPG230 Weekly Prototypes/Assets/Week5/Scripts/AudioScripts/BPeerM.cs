using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPeerM : MonoBehaviour
{
    public float bpm;
    private float beatInterval, beatTimer, beatIntervalD8, beatTimerD8;
    public static bool beatFull, beatD8;
    public static int beatCountFull, beatCountD8;

    void Update()
    {
        BeetDetection();
    }

    void BeetDetection()
    {
        beatFull = false;
        beatInterval = 60 / bpm;
        beatTimer += Time.deltaTime;

        if(beatTimer >= beatInterval)
        {
            beatTimer -= beatInterval;
            beatFull = true;
            beatCountFull++;
        }

        beatD8 = false;
        beatIntervalD8 = beatInterval / 8;
        beatTimerD8 += Time.deltaTime;

        if(beatTimer >= beatTimerD8)
        {
            beatTimerD8 -= beatIntervalD8;
            beatD8 = true;
            beatCountD8++;
        }
    }
}
