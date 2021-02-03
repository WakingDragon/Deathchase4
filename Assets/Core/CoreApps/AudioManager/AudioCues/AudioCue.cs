using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    [CreateAssetMenu(fileName = "AudioCue", menuName = "Audio/Audio Cue")]
    public class AudioCue : ScriptableObject
    {
        [SerializeField] private AudioTrack track = AudioTrack.SFX;
        [SerializeField] AudioClip[] clips = null;
        [SerializeField] bool randomizeVolPitch = false;    //if false then play max pitch and vol
        [SerializeField] float minVolume = 0f;
        [SerializeField] float maxVolume = 1f;
        [SerializeField] float minPitch = 0.5f;
        [SerializeField] float maxPitch = 2f;
        [SerializeField] bool loop = false;
        [SerializeField] [Range(0, 1)] float spatialBlend = 0f;

        public void SetLooping(bool isLooping) { loop = isLooping; }

        public void Play(AudioSource src)
        {
            if (AudioManager.isEnabled) { AudioManager.instance.PlayFromSrc(this, src); }
        }

        public void Play()
        {
            if (AudioManager.isEnabled) { AudioManager.instance.Play(this); }
        }

        public float PlayClipToSource(AudioSource source)
        {
            source.clip = clips[Random.Range(0, clips.Length)];

            if (randomizeVolPitch)
            {
                source.volume = Random.Range(minVolume, maxVolume);
                source.pitch = Random.Range(minPitch, maxPitch);
            }
            else
            {
                source.volume = maxVolume;
                source.pitch = maxPitch;
            }
            source.loop = loop;
            source.spatialBlend = spatialBlend;
            source.Play();
            return source.clip.length;
        }

        public float PlayOneShotToSource(AudioSource source)
        {
            var clip = clips[Random.Range(0, clips.Length)];

            if (randomizeVolPitch)
            {
                source.volume = Random.Range(minVolume, maxVolume);
                source.pitch = Random.Range(minPitch, maxPitch);
            }
            else
            {
                source.volume = maxVolume;
                source.pitch = maxPitch;
            }
            source.loop = this.loop;
            source.spatialBlend = spatialBlend;
            if(!loop)
            {
                source.PlayOneShot(clip);
            }
            else
            {
                source.clip = clip;
                source.Play();
            }
            return clip.length;
        }

        public AudioClip SelectClip()
        {
            return clips[Random.Range(0, clips.Length)];
        }
        public AudioTrack GetTrack() { return track; }
    }
}


