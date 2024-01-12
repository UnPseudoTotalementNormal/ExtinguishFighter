using System.Collections;
using UnityEngine;

public class AtomicBomb : Weapon
{
    private float _closeTimer = 0;
    private float _startFlagAt = 4;
    private float _endFlagAt = 18;
    private float _closingAt = 25;
    public override void Shoot()
    {
        PlayerHandScript.Instance.enabled = false;
        PlayerController.Instance.enabled = false;
        Destroy(PlayerHandScript.Instance);
        Destroy(PlayerController.Instance);

        if (_shootSound)
        {
            CustomAudio newAudio = new CustomAudio();
            newAudio.AudioClip = _shootSound;
            newAudio.Volume = _shootVolume;
            SoundSystem.Instance.Play(newAudio);
        }
        HUDScript.Instance.NuclearDeath.SetActive(true);
        for (int i = 0; i < HUDScript.Instance.NuclearDeath.transform.childCount; i++)
        {
            HUDScript.Instance.NuclearDeath.transform.GetChild(i).gameObject.SetActive(true);
        }
        StartCoroutine(stopGameBecauseYoureLiterallyDead());
        StartCoroutine(WaitForUsFlag());
    }

    private IEnumerator WaitForUsFlag()
    {
        bool stop = false;
        while (!stop)
        {
            if (_closeTimer >= _startFlagAt)
            {
                StartCoroutine(ShowUsFlag());
                stop = true;
            }
            yield return null;
        }
        
    }

    private IEnumerator ShowUsFlag()
    {
        float EndTimer = _endFlagAt - _closeTimer;
        float newTimer = 0;
        UnityEngine.UI.Image AmericanFlag = HUDScript.Instance.America;
        while (newTimer < _endFlagAt)
        {
            AmericanFlag.color = new Color(AmericanFlag.color.r, AmericanFlag.color.g, AmericanFlag.color.b, newTimer / EndTimer);
            newTimer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    private IEnumerator stopGameBecauseYoureLiterallyDead()
    {
        while (_closeTimer < _closingAt)
        {
            _closeTimer += Time.deltaTime;
            yield return null;
        }
        Application.Quit();
    }
}
