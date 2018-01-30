using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{

    private Rigidbody Body;
    private Collider Collider;

    public bool GamePaused;
    bool isShaking;
    int ShieldStatus;
    int Count;
    int Score;
    int ProgramsInfected;

    public float Friction;
    public float ScaleIncrease;
    public float Velocity;
    public float DownloadSpeed;
    public float DownloadProgress;
    float CamDistance;
    float GameTime;
    float RotAngle;
    float ShakeDuration;
    float ShieldDuration;
    float Input_h;
    float Input_v;

    public string PlaceholderTag;

    Vector3 PlayerDirection;
    Vector3 StartingCamPos;
    Vector2 BottomCorner;
    Vector2 TopCorner;

    public float MaxShieldDuration = 10;

    public GameObject MainCamera;
    public GameObject MainPlayer;
    public GameObject GO_Panel;
    public GameObject ShieldAnimationNormal;
    public GameObject ShieldAnimationBlink;
    public GameObject ExplosionAnimation;

    public Text GOstats;
    public Text ScoreText;

    public Audio ACode;
    private bool shaderAnimationUp = true;

    void Start()
    {
        Body = GetComponent<Rigidbody>();
        Collider = GetComponent<SphereCollider>();

        ScoreText.text = "" + Score + " bytes";

        StartingCamPos = MainCamera.transform.position;
        Body.velocity = Vector3.zero;
        Body.angularVelocity = Vector3.zero;
        GamePaused = false;
        Score = 0;
        ProgramsInfected = 0;
        GameTime = 0f;
        ShakeDuration = 1f;
        ShieldStatus = 0;

        ACode.PlaySFX(7);

        CamDistance = Vector3.Distance(Vector3.zero, Camera.main.transform.position);
        BottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, CamDistance));
        TopCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, CamDistance));
    }

    void Update()
    {
        Input_h = Input.GetAxisRaw("Horizontal");
        Input_v = Input.GetAxisRaw("Vertical");

#if UNITY_ANDROID
        Input_h = Input.acceleration.x * 3;
        Input_v = Input.acceleration.y * 3;
#endif
    }

    void FixedUpdate()
    {
        if (GamePaused == false)
        {
            GameTime += Time.deltaTime;

            if (Input_h != 0 || Input_v != 0)
            {
#if UNITY_STANDALONE
                Body.AddForce(new Vector3(Input_h * Velocity, Input_v * Velocity, 0f));
#endif
#if UNITY_WEBGL
                Body.AddForce(new Vector3(Input_h * Velocity, Input_v * Velocity, 0f));
#endif
#if UNITY_ANDROID
                Body.AddForce(new Vector3(Input_h * Velocity, Input_v * Velocity , 0f), ForceMode.Acceleration);
#endif
            }

#if UNITY_ANDROID
            Vector3 maxVelocity = Body.velocity;
            if (maxVelocity.x > 1) maxVelocity.x = 1;
            if (maxVelocity.x < -1) maxVelocity.x = -1;
            if (maxVelocity.y < -1) maxVelocity.y = -1;
            if (maxVelocity.y < -1) maxVelocity.y = -1;
            if (Input_h == 0) { maxVelocity.x = 0; }
            if (Input_v == 0) { maxVelocity.y = 0; }

            Body.velocity = maxVelocity;
#endif

            RotatePlayer();
            OutOfBounds();
            SlowDown();
            ShakeCamera();
            ShieldBuff();
        }
    }

    void SlowDown()
    {
        PlayerDirection = Body.velocity.normalized;
        PlayerDirection = PlayerDirection * -1f;
        Body.AddForce(PlayerDirection * Friction);
    }

    void RotatePlayer()
    {
        if (Body.velocity.magnitude > 0.1f)
        {
            RotAngle = Mathf.Atan2(Body.velocity.y, Body.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.forward);
        }
    }

    void OutOfBounds()
    {
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

    void ShakeCamera()
    {
        if (isShaking == true)
        {
            if (ShakeDuration > 0)
            {
                MainCamera.transform.position = StartingCamPos + Random.insideUnitSphere * 0.7f;
                ShakeDuration -= Time.deltaTime;
            }
            else
            {
                isShaking = false;
                ExplosionAnimation.GetComponent<ParticleSystem>().Clear();
                ExplosionAnimation.SetActive(false);
                MainCamera.transform.position = StartingCamPos;
            }
        }
    }

    void ShieldBuff()
    {
        if (ShieldStatus == 0)
        {
            ShieldAnimationBlink.SetActive(false);
        }
        if (ShieldStatus == 1)
        {
            //Blinking shield animation here
            if (ShieldDuration > 0f)
            {
                ShieldDuration -= Time.deltaTime;
                ShieldAnimationBlink.SetActive(true);
                ShieldAnimationNormal.SetActive(false);
            }
            else
            {
                ShieldStatus = 0;
            }
        }
        if (ShieldStatus == 2)
        {
            //Shield sprite here
            if (ShieldDuration > 0f)
            {
                ShieldDuration -= Time.deltaTime;
                ShieldAnimationNormal.SetActive(true);
            }
            else
            {
                ShieldStatus = 1;
                ShieldDuration = 2f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy" && GameObject.FindGameObjectsWithTag("Downloading").Length == 0)
        {
            PlaceholderTag = other.gameObject.tag;
            other.gameObject.tag = "Downloading";
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (ShieldStatus == 0)
            {
                Body.velocity = Vector3.zero;
                Body.angularVelocity = Vector3.zero;
                GamePaused = true;
                ACode.PlaySFX(5);

                GOstats.text = "You survived for ";
                if (GameTime > 3600f)
                {
                    GOstats.text += Mathf.Floor(GameTime / 3600f) + ":";
                }
                GOstats.text += Mathf.Floor(GameTime / 60f).ToString("00") + ":";
                GOstats.text += Mathf.Floor(GameTime % 60).ToString("00");

                GOstats.text += "\nYou infected " + ProgramsInfected + " program";
                if (ProgramsInfected > 1)
                {
                    GOstats.text += "s";
                }
                GOstats.text += "\n\n         Bytes : " + Score;

                GO_Panel.SetActive(true);
            }
            if (ShieldStatus == 2)
            {
                ShieldStatus = 1;
                ShieldDuration = 2f;
            }
        }
        if (other.gameObject.tag == "Downloading")
        {
            if (GamePaused == false)
            {
                if (other.GetComponent<Program>() == null)
                {
                    DownloadProgress += DownloadSpeed * Time.deltaTime;
                    DoAnimationShaderDownload();
                }
                else
                {
                    if (!other.GetComponent<Program>().DoAnimationDie)
                    {
                        DownloadProgress += DownloadSpeed * Time.deltaTime;
                        DoAnimationShaderDownload();
                    }
                }
            }
            if (DownloadProgress >= 100f)
            {
                if (PlaceholderTag == "Data")
                {
                    Score += 10;
                    ProgramsInfected++;
                    ACode.PlaySFX(4);

                    other.GetComponent<Program>().DoAnimationDestroy();

                    transform.localScale += new Vector3(ScaleIncrease, ScaleIncrease, 0f);
                }

                DownloadProgress = 0f;
                ScoreText.text = "" + Score + " bytes";
                CancelAnimationShaderDownload();
                if (PlaceholderTag == "PUDestroy")
                {
                    Score += 5;
                    Destroy(other.gameObject);
                    ACode.PlaySFX(1);

                    Count = GameObject.FindGameObjectsWithTag("Data").Length;
                    while (Count > 0)
                    {
                        Destroy(GameObject.FindGameObjectsWithTag("Data")[Count - 1]);
                        Count--;
                    }

                    Count = GameObject.FindGameObjectsWithTag("Enemy").Length;
                    while (Count > 0)
                    {
                        Destroy(GameObject.FindGameObjectsWithTag("Enemy")[Count - 1]);
                        Count--;
                    }
                    Count = GameObject.FindGameObjectsWithTag("PUShield").Length;
                    while (Count > 0)
                    {
                        Destroy(GameObject.FindGameObjectsWithTag("PUShield")[Count - 1]);
                        Count--;
                    }
                    isShaking = true;
                    ExplosionAnimation.SetActive(true);

                    ShakeDuration = 1f;
                }
                if (PlaceholderTag == "PUShield")
                {
                    Score += 5;
                    Destroy(other.gameObject);
                    ShieldStatus = 2;
                    ShieldDuration = MaxShieldDuration;
                    ACode.PlaySFX(2);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Downloading") { DownloadProgress = 0f; }
        if (other.gameObject.tag != "Enemy") { other.gameObject.tag = PlaceholderTag; }

        CancelAnimationShaderDownload();
    }

    private void CancelAnimationShaderDownload()
    {
        Camera.main.GetComponent<ShaderEffect_CorruptedVram>().shift = -0.58f;
        shaderAnimationUp = true;
        Camera.main.GetComponent<ShaderEffect_CorruptedVram>().enabled = false;
    }

    private void DoAnimationShaderDownload()
    {
        Camera.main.GetComponent<ShaderEffect_CorruptedVram>().enabled = true;
        if (shaderAnimationUp)
        {
            Camera.main.GetComponent<ShaderEffect_CorruptedVram>().shift += 0.01f;
        }
        else
        {
            Camera.main.GetComponent<ShaderEffect_CorruptedVram>().shift -= 0.01f;
        }

        if (Camera.main.GetComponent<ShaderEffect_CorruptedVram>().shift >= 2) shaderAnimationUp = false;
        if (Camera.main.GetComponent<ShaderEffect_CorruptedVram>().shift <= -2) shaderAnimationUp = true;

    }
}