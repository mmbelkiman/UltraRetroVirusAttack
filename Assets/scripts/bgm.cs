using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm : MonoBehaviour {

    AudioSource BGMsource;

    public bool BGMenabled;

    public player PCode;

	// Use this for initialization
	void Awake ()
    {
        BGMsource = GetComponent<AudioSource>();
	}

    void Start()
    {
        BGMsource.Play();
    }

    void FixedUpdate()
    {
        if (BGMenabled == true && PCode.GamePaused == false)
        {
            BGMsource.volume = 1f;
        }
        else
        {
            BGMsource.volume = 0f;
        }
    }
}
