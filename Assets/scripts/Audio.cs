using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {

    public AudioClip ActivateShield;
    public AudioClip ActivateBomb;
    public AudioClip DownloadAlmost;
    public AudioClip DownloadProgram;
    public AudioClip GameOver;
    public AudioClip SpawnEnemy;
    public AudioClip SpawnPlayer;

    public AudioClip MainTheme;
    
    AudioSource AMsource;
    
    public bool SFXenabled;

    int Count = 0;

    public player PCode;

	// Use this for initialization
	void Awake ()
    {
        AMsource = GetComponent<AudioSource>();
	}
	
    void FixedUpdate ()
    {
        if (PCode.GamePaused == true)
        {
            Count++;
        }
        else
        {
            Count = 0;
        }
    }

    public void PlaySFX (int SFXindex)
    {
        if (SFXenabled == true && Count < 5)
        {
            switch (SFXindex)
            {
                case 1:
                    AMsource.PlayOneShot(ActivateBomb);
                    break;
                case 2:
                    AMsource.PlayOneShot(ActivateShield);
                    break;
                case 3:
                    AMsource.PlayOneShot(DownloadAlmost);
                    break;
                case 4:
                    AMsource.PlayOneShot(DownloadProgram);
                    break;
                case 5:
                    AMsource.PlayOneShot(GameOver);
                    break;
                case 6:
                    AMsource.PlayOneShot(SpawnEnemy);
                    break;
                case 7:
                    AMsource.PlayOneShot(SpawnPlayer);
                    break;
                default:
                    break;
            }
        }
    }
}
