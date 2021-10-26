using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    private AudioSource audioSource;

    private float[] samples = new float[512];
    private float[] freqBand = new float[8];
    private float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];

    private float[] freqBandHeightest = new float[8];
    public float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if(freqBand[i] > freqBandHeightest[i])
            {
                freqBandHeightest[i] = freqBand[i];
            }

            audioBand[i] = (freqBand[i] / freqBandHeightest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHeightest[i]);
        }
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            freqBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if(freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrease[i] = 0.005f;
            }

            if (freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }
}
