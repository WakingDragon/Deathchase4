using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using BP.Core;


public class AudioTest : MonoBehaviour
{
    [SerializeField] private AudioManagerAsset audioAsset = null;
    //[SerializeField] private AudioClip clip = null;
    private float pitch = 1f;
    private bool isOn = true;
    [SerializeField] private FloatGameEvent pitchEvent = null;
    [SerializeField] private BoolGameEvent toggleEvent = null;
    //[SerializeField] private AudioCueGameEvent audioCueEvent = null;
    [SerializeField] private AudioCue cue = null;

    private void Awake()
    {
        if(!audioAsset) { Debug.Log("no audio asset set"); }
    }

    private void Start()
    {
        //audioAsset.Initialise();
        //audioAsset.SoundTest(clip);

        if (!AudioManager.instance)
        {
            Debug.Log("no instance");
        }
        else
        {
            Debug.Log("instance exists");
        }

        
    }

    private void Update()
    {
        Debug.Log("audio mgr status: " + AudioManager.isEnabled);

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //audioCueEvent.Raise(cue);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pitchEvent.Raise(pitch);
            pitch += 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toggleEvent.Raise(isOn);
            isOn = !isOn;
        }
    }
}
