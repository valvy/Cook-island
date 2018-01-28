using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] storyClips;
    private AudioSource audioSource;
    private bool hit = false;

    private bool played = false;
    public bool Hit { get { return hit; } set { hit = value; } }
	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = storyClips[Random.Range(0, storyClips.Length - 1)];

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public AudioClip GetSelectedAudioClip()
    {
        return audioSource.clip;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!played)
        {
            VA_AudioSource[] sources = GameObject.FindObjectsOfType<VA_AudioSource>();
            for(int i = 0; i < sources.Length; i++)
            {
                sources[i].BaseVolume *= 0.5f;
            }
            audioSource.Play();
            //Make transparent
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
            //hit = true;
            played = true;
        }
    }
}
