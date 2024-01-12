using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Play(CustomAudio newCustomAudio)
    {
        GameObject newObject = new GameObject();
        AudioSource audioSource = newObject.AddComponent<AudioSource>();
        audioSource.clip = newCustomAudio.AudioClip;
        audioSource.loop = newCustomAudio.b_IsLoop;
        audioSource.volume = newCustomAudio.Volume;
        if (newCustomAudio.b_IsLocated)
        {
            audioSource.maxDistance = newCustomAudio.MaxDistance;
        }
        else
        {
            audioSource.maxDistance = 9999999999;
        }
        if (newCustomAudio.b_RandomPitch)
        {
            audioSource.pitch = 1 + Random.Range(-newCustomAudio.MaxPitchRandomness, newCustomAudio.MaxPitchRandomness);
        }
        audioSource.playOnAwake = true;
        return Instantiate(newObject, newCustomAudio.AudioPosition, Quaternion.identity).gameObject;
    }
}
