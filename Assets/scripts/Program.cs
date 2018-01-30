using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Program : MonoBehaviour
{
    public int maxTimeAtScreen = 100;
    public GameObject AnimationDie;
    public bool DoAnimationDie = false;
    public GameObject animator;

    private int lifeTime = 0;
    private int animationDieTime = 0;
    private int maxAnimationDieTime = 80;
    private bool atDownload = false;


    void Start()
    {
  
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!atDownload) lifeTime++;

        if (DoAnimationDie) { animationDieTime++;  }

        CheckTimeToDestroy();
    }

    private void CheckTimeToDestroy()
    {
        if (lifeTime > maxTimeAtScreen
            && !atDownload
            && !DoAnimationDie)
        {
            Destroy(transform.gameObject);
        }

        if (animationDieTime > maxAnimationDieTime) { Destroy(transform.gameObject); }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") { atDownload = true; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") { atDownload = false; }

    }

    public void DoAnimationDestroy()
    {
        animator.GetComponent<Animator>().Play("End_program",0,0);
        AnimationDie.SetActive(true);
        DoAnimationDie = true;
    }

}
