using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antivirus : MonoBehaviour
{
    public float MaxSpeed;

    private Rigidbody Body;
    private Collider Collider;
    private float VelocityX = 0;
    private float VelocityY = 0;
    private float RotAngle;
    private int countTime = 100;
    private int maxCountTime = 100;

    private bool respawNow;

    Vector2 BottomCorner;
    Vector2 TopCorner;
    float CamDistance;

    void Start()
    {
        Body = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();

        CamDistance = Vector3.Distance(Vector3.zero, Camera.main.transform.position);
        BottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, CamDistance));
        TopCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, CamDistance));
        respawNow = true;
    }

    void FixedUpdate()
    {
        countTime++;
        Move();
        OutOfBounds();
        Rotate();
        ControlMaxVelocity();

        if (respawNow)
        {
            MoveToCenter();
        }
        else if (countTime > maxCountTime)
        {
            countTime = 0;

            VelocityX = Random.Range(-1, 2);
            VelocityY = Random.Range(-1, 2);

            Body.AddForce(new Vector3(-Body.velocity.x * 10, -Body.velocity.y * 10, 0));

            //Body.angularVelocity = Vector3.zero;
            //Body.velocity = Vector3.zero;
        }

    }

    private void ControlMaxVelocity()
    {
        if (respawNow) return;
        Vector3 newVelocity = Body.velocity;

        if (Mathf.Abs(Body.velocity.x) > MaxSpeed) { newVelocity.x = MaxSpeed; }
        if (Mathf.Abs(Body.velocity.y) > MaxSpeed) { newVelocity.y = MaxSpeed; }

        Body.velocity = newVelocity;
    }

    private void Move()
    {
        Body.AddForce(new Vector3(VelocityX, VelocityY, 0));
    }

    public bool CanHit()
    {
        return AtScreen();
    }

    public bool AtScreen()
    {
        return false;
    }

    void Rotate()
    {
        if (Body.velocity.magnitude > 0.1f)
        {
            RotAngle = Mathf.Atan2(Body.velocity.y, Body.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.forward);
        }
    }

    private void MoveToCenter()
    {
        bool done = true;

        if (transform.position.x > TopCorner.x)
        {
            VelocityX = -1;
            done = false;
        }
        if (transform.position.x < BottomCorner.x)
        {
            VelocityX = 1;
            done = false;
        }
        if (transform.position.y > TopCorner.y)
        {
            // VelocityY = -1;
            // done = false;
        }
        if (transform.position.y < (-1f * (Collider.bounds.size.y) / 2f) + BottomCorner.y + 0.1f)
        {
            //    VelocityY = 1;
            /// done = false;
        }

        if (done) respawNow = false;
    }

    void OutOfBounds()
    {
        if (respawNow) return;

        if (transform.position.x > ((Collider.bounds.size.x) / 2f) + TopCorner.x - 0.1f)
        {
            transform.position += new Vector3((BottomCorner.x * 2f - Collider.bounds.size.x + 0.3f), 0f, 0f);
        }
        if (transform.position.x < (-1f * (Collider.bounds.size.x) / 2f) + BottomCorner.x + 0.1f)
        {
            transform.position += new Vector3((TopCorner.x * 2f + Collider.bounds.size.x - 0.3f), 0f, 0f);
        }
        if (transform.position.y > ((Collider.bounds.size.y) / 2f) + TopCorner.y - 0.1f)
        {
            transform.position += new Vector3(0f, (BottomCorner.y * 2f - Collider.bounds.size.y + 0.3f), 0f);
        }
        if (transform.position.y < (-1f * (Collider.bounds.size.y) / 2f) + BottomCorner.y + 0.1f)
        {
            transform.position += new Vector3(0f, (TopCorner.y * 2f + Collider.bounds.size.y - 0.3f), 0f);
        }
    }
}
