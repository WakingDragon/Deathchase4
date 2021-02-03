using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

public class GOAudioTest : MonoBehaviour
{
    //[SerializeField] private AudioRequestEvent requestEvent = null;
    //[SerializeField] private AudioCueGameEvent cueEvent = null;
    [SerializeField] private AudioCue cue = null;
    private float maxWait = 2f;
    private float nextNoise;
    private AudioSource src;


    private void Start()
    {
        nextNoise = Time.time + Random.Range(0f, maxWait);
        src = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Time.time > nextNoise)
        {
            cue.Play(src);
            nextNoise = Time.time + Random.Range(0f, maxWait);
        }
    }
}
