using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDestroy : MonoBehaviour {

    public int maxTimeAtScreen = 100;
    private int lifeTime = 0;
    private bool atDownload = false;

    // Use this for initialization
    void Start () {
		
	}

    private void FixedUpdate()
    {
        if (!atDownload) lifeTime++;

        CheckTimeToDestroy();
    }

    private void CheckTimeToDestroy()
    {
        if (lifeTime > maxTimeAtScreen
            && !atDownload)
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") { atDownload = true; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") { atDownload = false; }

    }

}
