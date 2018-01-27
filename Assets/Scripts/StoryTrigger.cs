using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] storyClips;
    private AudioSource audioSource;
    private bool hit = false;
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
        if(!hit)
        {
            audioSource.Play();
            hit = true;
        }
    }
}
