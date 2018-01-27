using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public delegate void OnMoveDoneEvent(Vector2 postionMoved);
    public event OnMoveDoneEvent OnMoveDone;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float range = 20;

    private float timeToNextState = 5;

    [SerializeField]
    private float timeToSpawnTrail = 1;

    private float rotation;

    private float timer = 0;
    private float trailTimer = 0;


    private enum PlayerState
    {
        StartStairs,
        Waiting,
        EndStairs,
        None
    }

    private PlayerState playerState = PlayerState.None;

    [SerializeField]
    private GameObject hideObject;

    [SerializeField]
    private GameObject trailPrefab;

    private Vector2 startPosition;

    [SerializeField]
    private Text debugText;

    private float zeroPoint = 0;

    [SerializeField]
    private int collectCount = 0;

    [SerializeField]
    private AudioClip startStairsClip;

    [SerializeField]
    private AudioClip endStairsClip;

    [SerializeField]
    private AudioClip therapyClip;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Button startButton;

	// Use this for initialization
	void Start ()
    {
        //StartMoving();
        startPosition = transform.position;

    }

    private void PlayAudio(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    // Update is called once per frame
    void Update ()
    {

        if (playerState == PlayerState.StartStairs || playerState == PlayerState.EndStairs)
        {
            transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
            transform.Rotate(new Vector3(0, 0, rotation * rotationSpeed * Time.deltaTime));
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, ClampAngle(transform.eulerAngles.z, -range, range));

            float horizontal = GyroInput.getInstance().getTilt();
            horizontal = Mathf.Clamp(horizontal, -1, 1);

            if (debugText != null)
            {
                debugText.text = horizontal.ToString();
            }
            UpdateRotation(horizontal);

            timer += Time.deltaTime;
            trailTimer += Time.deltaTime;

            if (trailTimer > timeToSpawnTrail)
            {
                SpawnTrail();
                trailTimer = 0;
            }

            if (timer > timeToNextState)
            {
                MoveDone();
            }

        }
        if(playerState == PlayerState.Waiting)
        {
            timer += Time.deltaTime;

            if(timer > timeToNextState)
            {
                ListenDone();
            }
        }

    }

    private void SpawnTrail()
    {
        if (trailPrefab != null)
        {
            GameObject trail = GameObject.Instantiate(trailPrefab);
            trail.transform.position = transform.position;
        }
    }

    private void ListenDone()
    {
        playerState = PlayerState.EndStairs;
        timeToNextState = endStairsClip.length;
        PlayAudio(endStairsClip);

        AudioSource[] audioSources = GameObject.FindObjectsOfType<AudioSource>();
        for(int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].mute = false;
        }
    }
    private void MoveDone()
    {
        if(playerState == PlayerState.EndStairs)
        {
            scoreText.text = "Score: " + collectCount;
            playerState = PlayerState.None;
            if (hideObject != null)
            {
                hideObject.SetActive(false);
                if (startButton != null)
                {
                    startButton.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            playerState = PlayerState.Waiting;
            timeToNextState = therapyClip.length;
            AudioSource[] audioSources = GameObject.FindObjectsOfType<AudioSource>();
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].mute = true;
            }
            audioSource.mute = false;
            PlayAudio(therapyClip);
        }
        timer = 0;
        /*
        Vector2 positionMoved = new Vector2(Mathf.Abs(transform.position.x - startPosition.x), Mathf.Abs(transform.position.y - startPosition.y));
        if (OnMoveDone != null)
        {
            OnMoveDone(positionMoved);
        }*/
    }
    public void StartMoving()
    {
        timer = 0;
        playerState = PlayerState.StartStairs;
        timeToNextState = startStairsClip.length;
        PlayAudio(startStairsClip) ;

        if (hideObject != null)
        {
            hideObject.SetActive(true);
        }

    }
    public void OnStartClick()
    {
        StartMoving();
        if(startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }
    }
    public void UpdateRotation(float rotation)
    {
        this.rotation = rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollectAble collectAble = collision.gameObject.GetComponent<CollectAble>();
        Debug.Log(collectAble);
        if (collectAble != null)
        {
            Destroy(collectAble.gameObject);
            collectCount++;
        }
    }

    // Thank you internet
    // https://forum.unity.com/threads/limiting-rotation-with-mathf-clamp.171294/
    private float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }
}
