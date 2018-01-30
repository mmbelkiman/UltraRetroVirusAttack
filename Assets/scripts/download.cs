using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class download : MonoBehaviour
{

    public GameObject DLline;
    public GameObject Player;

    bool DownloadOngoing;
    float DLLspawn;
    int count = 0;
    
    player PCode;

    void Start()
    {
        PCode = Player.GetComponent<player>();

        DLLspawn = -1.89f;

        DownloadOngoing = false;

        while (DLLspawn < 1.90f)
        {
            GameObject newLine = Instantiate(DLline, new Vector3(DLLspawn, -4.25f, -0.3f), new Quaternion());
            newLine.GetComponent<SpriteRenderer>().enabled = false;
            DLLspawn += 0.08f;
        }
    }

    void FixedUpdate()
    {
        if (DownloadOngoing == false)
        {
            if (PCode.DownloadProgress > 0.01f)
            {
                DownloadOngoing = true;
            }
        }
        if (DownloadOngoing == true)
        {
            if (PCode.DownloadProgress >= 0.01f)
            {
                while (count < Mathf.Floor(PCode.DownloadProgress / 2.04f))
                {
                    GameObject.FindGameObjectsWithTag("DLline")[count].GetComponent<SpriteRenderer>().enabled = true;
                    count++;
                }
            }
            if (GameObject.FindGameObjectsWithTag("Downloading").Length == 0)
            {
                count = 0;
                while (count < 48)
                {
                    GameObject.FindGameObjectsWithTag("DLline")[count].GetComponent<SpriteRenderer>().enabled = false;
                    count++;
                }
                count = 0;
                DownloadOngoing = false;
            }
        }
    }
}