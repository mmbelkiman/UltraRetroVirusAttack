using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public bool BGMstatus;
    public bool SFXstatus;

    AudioSource BGMsource;

    void Awake()
    {
        BGMsource = GetComponent<AudioSource>();
    }

    void Start()
    {
        BGMsource.Play();
    }

    void FixedUpdate()
    {
        if (BGMstatus == true)
        {
            BGMsource.volume = 1f;
        }
        else
        {
            BGMsource.volume = 0f;
        }
    }

    public void BGMHandler (bool BGM)
    {
        BGMstatus = BGM;
    }
    public void SFXHandler (bool SFX)
    {
        SFXstatus = SFX;
    }
}
