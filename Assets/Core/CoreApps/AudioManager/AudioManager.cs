using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BP.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }
        public static bool isEnabled  { get; private set; }

        [SerializeField] private AudioManagerAsset m_audioAsset;

        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            if(!m_audioAsset) { Debug.Log("no audio asset in audio manager"); }

            m_audioAsset.InitialiseFromInstance(this);
            isEnabled = true;
        }

        private void OnDestroy()
        {
            m_audioAsset.Deactivate();
        }

        public void SetAudioAsset(AudioManagerAsset audioAsset) { m_audioAsset = audioAsset; }

        #region volume levels, toggle, pitch
        public void ToggleMasterVolume(bool on)
        {
            m_audioAsset.ToggleChannel(AudioTrack.Master, on);
        }
        public void ToggleSoundtrackVolume(bool on)
        {
            m_audioAsset.ToggleChannel(AudioTrack.Soundtrack, on);
        }
        public void SetMasterVolume(float level)
        {
            m_audioAsset.SetChannelVolume(AudioTrack.Master, level);
        }
        public void SetAmbienceVolume(float level)
        {
            m_audioAsset.SetChannelVolume(AudioTrack.Ambience, level);
        }
        public void SetSFXVolume(float level)
        {
            m_audioAsset.SetChannelVolume(AudioTrack.SFX, level);
        }
        public void SetSoundtrackVolume(float level)
        {
            m_audioAsset.SetChannelVolume(AudioTrack.Soundtrack, level);
        }
        public void SetUIVolume(float level)
        {
            m_audioAsset.SetChannelVolume(AudioTrack.UI, level);
        }
        public void AdjustSoundtrackPitch(float pitch)
        {
            m_audioAsset.AdjustPitch(AudioTrack.Soundtrack, pitch);
        }
        #endregion

        public void Play(AudioCue cue)
        {
            m_audioAsset.Play(cue);
        }

        public void PlayFromSrc(AudioCue cue, AudioSource src)
        {
            m_audioAsset.PlayFromSrc(cue,src);
        }
    }
}

