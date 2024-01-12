using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusicScript : MonoBehaviour
{
    private AudioSource _musicSource;

    private void Awake()
    {
        _musicSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (PlayerHandScript.Instance.GetCurrentWeapon())
        {
            AudioSource holdingSound = PlayerHandScript.Instance.GetCurrentWeapon().HoldingSound;
            if (holdingSound)
            {
                if (_musicSource.isPlaying)
                {
                    _musicSource.Pause();
                }
            }
            else if (!_musicSource.isPlaying)
            {
                _musicSource.UnPause();
            }
        }
    }
}
