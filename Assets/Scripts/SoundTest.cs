using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGeneration : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip originalClip;
    private float[] originalSamples;
    private float lastPitchShift = 1.0f;
    private System.Random random = new System.Random();
    private List<float> pitchHistory = new List<float>();
    private float minPitchShift = 0.7f;
    private float maxPitchShift = 1.3f;

    void Start()
    {
        originalSamples = new float[originalClip.samples * originalClip.channels];
        originalClip.GetData(originalSamples, 0);
        //audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }
    
    public AudioClip GenerateAudio()
    {
        //create a new array to hold modified samples
        float[] modifiedSamples = new float[originalSamples.Length];
        System.Array.Copy(originalSamples, modifiedSamples, originalSamples.Length);

        //apply change rules
        float pitchShift = GetAdjustedPitchShift();
        float timeStretch = Random.Range(0.9f, 1.1f);
        ApplyTimeStretch(ref modifiedSamples, timeStretch);  
        ApplyPitchShift(ref modifiedSamples, pitchShift); 
 
        AudioClip modifiedClip = AudioClip.Create("ModifiedClip", modifiedSamples.Length / originalClip.channels, originalClip.channels, originalClip.frequency, false);
        modifiedClip.SetData(modifiedSamples, 0);

        return modifiedClip;
    }

    private void ApplyTimeStretch(ref float[] samples, float stretchFactor)
    {
        int newLength = (int)(samples.Length / stretchFactor);
        float[] stretchedSamples = new float[newLength];

        for (int i = 0; i < newLength; i++)
        {
            float srcIndex = i * stretchFactor;
            int intIndex = (int)srcIndex;
            float fracIndex = srcIndex - intIndex;

            if (intIndex + 1 < samples.Length)
            {
                stretchedSamples[i] = Mathf.Lerp(samples[intIndex], samples[intIndex + 1], fracIndex);
            }
            else
            {
                stretchedSamples[i] = samples[intIndex];
            }
        }

        samples = stretchedSamples;
    }

    private void ApplyPitchShift(ref float[] samples, float pitchFactor)
    {
        int newLength = (int)(samples.Length / pitchFactor);
        float[] shiftedSamples = new float[newLength];

        for (int i = 0; i < newLength; i++)
        {
            float srcIndex = i * pitchFactor;
            int intIndex = (int)srcIndex;
            float fracIndex = srcIndex - intIndex;

            if (intIndex + 1 < samples.Length)
            {
                shiftedSamples[i] = Mathf.Lerp(samples[intIndex], samples[intIndex + 1], fracIndex);
            }
            else
            {
                shiftedSamples[i] = samples[intIndex];
            }
        }

        samples = shiftedSamples;
    }

    private float GetAdjustedPitchShift()
    {
        float pitchShift;
        float oppositeShiftProbability = 0.5f; //base probability for an opposite pitch shift
        int trendCount = 0;
        bool isIncreasing = true;

        //determine the current trend and count consecutive increases or decreases
        for (int i = pitchHistory.Count - 1; i > 0; i--)
        {
            if (pitchHistory[i] > pitchHistory[i - 1])
            {
                if (!isIncreasing)
                {
                    pitchHistory = new List<float>();
                    break;
                }
                trendCount++;
                isIncreasing = true;
            }
            else if (pitchHistory[i] < pitchHistory[i - 1])
            {
                if (isIncreasing)
                {
                    pitchHistory = new List<float>();
                    break;
                }
                trendCount++;
                isIncreasing = false;
            }
            else
            {
                pitchHistory = new List<float>();
                break;
            }
        }

        //increase the probability of an opposite pitch shift based on the trend count
        oppositeShiftProbability += trendCount * 0.1f;
        oppositeShiftProbability = Mathf.Min(oppositeShiftProbability, 1.0f);

        float lowerBound = Mathf.Max(minPitchShift, lastPitchShift - 0.1f); 
        float upperBound = Mathf.Min(maxPitchShift, lastPitchShift + 0.1f); 

        if (random.NextDouble() < oppositeShiftProbability)
        {
            if (isIncreasing)
            {
                //shift towards lower range if the trend was increasing
                pitchShift = Random.Range(minPitchShift, lowerBound);
                Debug.Log(3);
                Debug.Log(pitchShift);
            }
            else
            {
                //shift towards upper range if the trend was decreasing
                pitchShift = Random.Range(upperBound, maxPitchShift);
                Debug.Log(2);
                Debug.Log(pitchShift);
            }
        }
        else
        {
            //totally random in the the interval if odds didnt hit
            Debug.Log(1);
            pitchShift = GetRandomFromIntervals(minPitchShift, lowerBound, upperBound, maxPitchShift);
            Debug.Log(pitchShift);
        }

        pitchHistory.Add(pitchShift);
        
        lastPitchShift = pitchShift;
        return pitchShift;
    }

    private float GetRandomFromIntervals(float min1, float max1, float min2, float max2)
    {
        
        float length1 = max1 - min1;
        float length2 = max2 - min2;
        float totalLength = length1 + length2;

        float interval1Probability = length1 / totalLength;

        if (Random.value < interval1Probability)
        {
            return Random.Range(min1, max1);
        }
        else
        {
            return Random.Range(min2, max2);
        }
    }

}
