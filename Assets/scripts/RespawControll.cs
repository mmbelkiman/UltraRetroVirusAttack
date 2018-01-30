using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawControll : MonoBehaviour
{
    public GameObject Respaw;
    public int MaxRespawSameTime = 0;
    public int respawTime;
    public bool SpawInsideScreen;

    private int currentTime = 0;
    private string Tag;
    private Vector2 BottomCorner;
    private Vector2 TopCorner;
    private float CamDistance;

    public Audio ACode;

    void Start()
    {
        Tag = Respaw.tag;

        CamDistance = Vector3.Distance(Vector3.zero, Camera.main.transform.position);
        BottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, CamDistance));
        TopCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, CamDistance));
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag(Tag).Length <= MaxRespawSameTime - 1)
        {
            currentTime++;
        }

        if (currentTime > respawTime) { CreateObject(); currentTime = 0; }
    }

    private void CreateObject()
    {
        Instantiate(Respaw, RandomPosition(), new Quaternion());
        if (Tag == "Enemy")
        {
            ACode.PlaySFX(6);
        }
    }

    private Vector3 RandomPosition()
    {
        if (SpawInsideScreen)
        {
            return new Vector3(Random.Range(BottomCorner.x, TopCorner.x), Random.Range(TopCorner.y, BottomCorner.y), 0);
        }
        else
        {
            float newX = Random.Range(BottomCorner.x * 2, TopCorner.x * 2);
            float newY = Random.Range(TopCorner.y * 2, BottomCorner.y * 2);

            while (newX >= BottomCorner.x && newX <= TopCorner.x)
            {
                newX = Random.Range(BottomCorner.x * -2, TopCorner.x * 2);
            }
            while (newY >= TopCorner.y && newX <= BottomCorner.y)
            {
                newY = Random.Range(TopCorner.y * 2, BottomCorner.y * 2);
            }

            return new Vector3(newX, newY, 0);
        }
    }
}
