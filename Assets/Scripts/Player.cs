using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float range = 20;

    private float timeToNextState = 5;

    [SerializeField]
    private float timeAddState = 2;

    [SerializeField]
    private float timeToSpawnTrail = 1;

    private float rotation;

    private float timer = 0;
    private float trailTimer = 0;


    private enum PlayerState
    {
        StartStairs,
        Listening,
        None
    }

    private PlayerState playerState = PlayerState.None;

    [SerializeField]
    private GameObject hideObject;

    [SerializeField]
    private GameObject trailPrefab;

    [SerializeField]
    private int collectCount = 0;

    [SerializeField]
    private AudioSource walkAudioSource;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Button startButton;

	// Use this for initialization
	void Start ()
    {

    }

    private void PlayAudio(AudioClip audioClip)
    {
        walkAudioSource.Stop();
        walkAudioSource.clip = audioClip;
        walkAudioSource.Play();
    }
    // Update is called once per frame
    void Update ()
    {

        if(playerState == PlayerState.None)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartMoving();
            }
        }
        if (playerState == PlayerState.StartStairs)
        {
            transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);


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
        if(playerState == PlayerState.Listening)
        {
            timer += Time.deltaTime;
            if (timer > timeToNextState)
            {
                playerState = PlayerState.None;
                HideScreen();

            }
        }
        rotation = rotation * rotationSpeed;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, ClampAngle(rotation * range, -range, range));

        float horizontal = GyroInput.getInstance().getTilt();
        horizontal = Mathf.Clamp(horizontal, -1, 1);

        UpdateRotation(horizontal);
    }

    private void SpawnTrail()
    {
        if (trailPrefab != null)
        {
            GameObject trail = GameObject.Instantiate(trailPrefab);
            trail.transform.position = transform.position;
        }
    }
    private void MoveDone()
    {
        walkAudioSource.Stop();
        playerState = PlayerState.None;
        timeToNextState = timeToNextState + timeAddState;
        HideScreen();
        if (startButton != null)
        {
            startButton.gameObject.SetActive(true);
        }
        timer = 0;

    }

    public void Listening(float waitSeconds)
    {

        timer = 0;
        playerState = PlayerState.Listening;

        walkAudioSource.Stop();
        timeToNextState = waitSeconds;
        ShowScreen();
    }
    public void StartMoving()
    {
        timer = 0;
        playerState = PlayerState.StartStairs;
        walkAudioSource.Play();


        ShowScreen();

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

    public void HideScreen()
    {
        hideObject.SetActive(true);
        hideObject.GetComponent<Animator>().SetTrigger("FadeOut");
    }

    public void ShowScreen()
    {
        hideObject.SetActive(true);
        hideObject.GetComponent<Animator>().SetTrigger("FadeIn");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollectAble collectAble = collision.gameObject.GetComponent<CollectAble>();
        if (collectAble != null)
        {
            Destroy(collectAble.gameObject);
            collectCount++;
        }

        StoryTrigger storyTrigger = collision.gameObject.GetComponent<StoryTrigger>();
        if(storyTrigger != null)
        {
            Listening(storyTrigger.GetSelectedAudioClip().length);
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