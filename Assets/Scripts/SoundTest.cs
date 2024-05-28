using UnityEngine;

public class SoundGeneration : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip originalClip;
    private float[] originalSamples;
    private float lastPitchShift = 1.0f;
    private System.Random random = new System.Random();

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
       // audioSource.clip = modifiedClip;
        //audioSource.Play();
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

        if (lastPitchShift > 1.0f)
        {
            // If the last pitch shift was greater than 1, there's a higher chance of getting a lower shift this time.
            pitchShift = Random.Range(0.8f, 1.2f);
            if (random.NextDouble() < 0.75)  // 75% chance to be less than 1.0
            {
                pitchShift = Random.Range(0.8f, 1.0f);
            }
        }
        else
        {
            // If the last pitch shift was less than 1, there's a higher chance of getting a higher shift this time.
            pitchShift = Random.Range(0.8f, 1.2f);
            if (random.NextDouble() < 0.75)  // 75% chance to be greater than 1.0
            {
                pitchShift = Random.Range(1.0f, 1.2f);
            }
        }

        lastPitchShift = pitchShift;
        return pitchShift;
    }

}
