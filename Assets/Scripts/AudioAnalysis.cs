using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioAnalysis : MonoBehaviour
{
    // 8 cubes display
    const int numObjects = 8; // number of blocks/cubes for display. MakeFrequencyBands() breaks if you change this as it goes out of audioSamples's array bounds
    // public GameObject[] displayCubes = new GameObject[numObjects]; // array that stores the cubes, making it public allows you to put an already existing object in it

    // 512 cubes display
    // public GameObject sampleCubePrefab; // for creating cubes, put a 1 by 1 normal cube in here
    // GameObject[] smallCubesArray = new GameObject[512]; // for storing the cubes to be created, as copied from tutorial
    // public float smallCubesScale = 50; // for adjusting the scale of the cubes when they expand, default 50

    // audio processing
    public static AudioSource audioSource; // put your audio in here for processing
    public static float[] audioSamples = new float[512]; // array used to hold the audio samples
    public static float[] freqBand = new float[numObjects]; // array to hold the 8 values for display
    public static float[] bandBuffer = new float[numObjects]; // array for values after using buffer
    float[] bufferDecrease = new float[numObjects]; // array for storing the decay rate for each frequency band

    public static float blockScale = 0.8f; // for adjusting the scale of the 8 cubes when they expand, default 0.8
    public bool fastDecay = true; // for adjusting fast or slow decay, i like fast. Can also be unticked in unity

    // colour processing
    // Material[] emissionMaterials = new Material[numObjects]; // for storing the materials of the objects
    public static float[] maxValue = new float [numObjects]; // for storing the maximum value of each frequency band


    // copy this to allow more objects ################################################################################################################################################
    // public GameObject[] display_1 = new GameObject[numObjects]; // stores the objects (cubes)
    // Material[] emissionMaterials_1 = new Material[numObjects]; // stores the objects' materials
    // public float blockScale_1 = 0.8f; // for adjusting cube heights


    // for pausing
    public string hideKey = "m";
    private bool soundOn = true;

    // for volume change
    public float audioVolume = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        // retrieves the materials for each cube (cubes are children of this object)
        // for(int i=0; i<numObjects; i++){
        //     emissionMaterials[i] = displayCubes[i].GetComponent<MeshRenderer>().materials[0];

        //     // for adding more cubes/objects ################################################################################################################################################
        //     // emissionMaterials_1[i] = display_1[i].GetComponent<MeshRenderer>().materials[0];
        // }

        // create 512 display
        // for(int i=0; i<512; i++){
        //     GameObject smallCubes = (GameObject)Instantiate(sampleCubePrefab);
        //     smallCubes.transform.position = this.transform.position; // to be honest I don't know what these transforms do and not sure if they all matter but whatever
        //     smallCubes.transform.parent = this.transform;
        //     smallCubes.name = "cube " + i; // give them a name in case debugging is needed
        //     smallCubes.transform.position = new Vector3((float)(0-5.0f + 10.0f/512*i), 0, -2); // span them between x=-5 and x=5
        //     smallCubesArray[i] = smallCubes; // store the object in the array
        // }
    }

    // Update is called once per frame
    void Update()
    {   
        GetSpectrumAudioSource(); // does the fourier transform
        MakeFrequencyBands(); // sorts the raw data into frequency bands
        BandBuffer(); // handles the decay so the display looks nicer

        // changes emission or colour of objects. Uncomment as necessary
        // for (int i=0; i<numObjects; i++){
        //     displayCubes[i].transform.localScale = new Vector3(1, (float)(bandBuffer[i] * blockScale + 0.04), 1);
        //     float tempValue = bandBuffer[i] / maxValue[i];
        //     Color _color = new Color(tempValue, tempValue, tempValue);
        //     emissionMaterials[i].SetColor("_EmissionColor", _color);
        //     // Color _color2 = new Color(0, (CameraScript.yAngle+180)/360, (CameraScript.yAngle+180)/360);
        //     // emissionMaterials[i].SetColor("_Color", _color2);

        //     // for adding more cubes/objects ################################################################################################################################################
        //     // display_1[i].transform.localScale = new Vector3(1, (float)(bandBuffer[i] * blockScale_1 + 0.04), 1);
        //     // emissionMaterials_1[i].SetColor("_EmissionColor", _color);
        // }

        // expands the 512 display
        // for(int i=0; i<512; i++){
        //     smallCubesArray[i].transform.localScale = new Vector3(0.04f, (float)(audioSamples[i] * smallCubesScale + 0.04), 0.04f);
        // }

        
        if(Input.GetKeyDown(hideKey)){
            if(soundOn){
                audioSource.Pause();
                soundOn = false;
            }
            else{
                audioSource.Play();
                soundOn = true;
            }
        }

        // set music volume
        audioSource.volume = audioVolume;
    }


    public void SetVolume(float vol){
        audioVolume = vol;
    }


    void GetSpectrumAudioSource(){
        audioSource.GetSpectrumData(audioSamples, 0, FFTWindow.Blackman);// fourier transform
    }

    void BandBuffer(){
        for(int g=0; g<numObjects; g++){ // not sure why I used g but here it is
            if(freqBand[g] >= bandBuffer[g]){ // if raw data is more than data in decay buffer
                bandBuffer[g] = freqBand[g]; 
                bufferDecrease[g] = 0.005f; // resets decay rate
            }
            else{
                if(fastDecay) bufferDecrease[g] *= 1.2f; // fast drop
                else bufferDecrease[g] = (bandBuffer[g] - freqBand[g])/8; // smooth decay
                bandBuffer[g] -= bufferDecrease[g]; // decay
            }
            if(bandBuffer[g] < 0) bandBuffer[g] = 0; // prevent negative values
        }
    }

    int Rounding(double number){ // rounding because apparently (int) does flooring, and I can't find out how to use Math.Round()
        if((int)(number+0.5) > (int)number){
            return (int)(number+0.5);
        }
        else{
            return (int)number;
        }
    }

    void MakeFrequencyBands(){
        // 44100 hz / 512 minibands = 86.133 hz per sample
        /* Spectrum Categories
         * 20-60hz
         * 60-250hz
         * 250-500hz
         * 500-2000hz
         * 2000-4000hz
         * 4000-6000hz
         * 6000-20000hz
         * 
         * 0 2      
         * 1 4
         * 2 8
         * 3 16
         * 4 32
         * 5 64
         * 6 128
         * 7 256  
         * 
         * 0 2+4 = 6 - 1 = 5
         * 1 8+16+32+64+128 = 248 + 1 +24 =  273
         * 2 256 = 256 -24 = 232
         */

        int count = 0; // count the current sample

        for(int i=0; i<numObjects; i++){
            float average = 0; // average of the values in a frequency band
            int sampleCount = Rounding(Mathf.Pow(2, i+1)); // find the number of samples in the frequency band. Adjust power to affect scaling, only do it if you have different number of freq bands
            // I had an excel sheet to help calculate this ages ago, ask if u need help with this

            for(int j=0; j<sampleCount; j++){
                average += audioSamples[count] * (count + 1); // count+1 is used to increase the value of high frequency samples. count+1 instead of count otherwise first sample is 0
                count++;
            }
            average /= sampleCount; // used count in fyp, think that's a mistake. I'm pretty sure sampleCount is correct

            if(audioVolume > 0.03){
                freqBand[i] = average / audioVolume; // make independent of audio volume
            }
            else{
                freqBand[i] = average; // avoid dividing by 0
            }
            if(freqBand[i] > maxValue[i]) maxValue[i] = freqBand[i]; // keep track of the max value
        }
    }
}
