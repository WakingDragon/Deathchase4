using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BP.Core
{
    [System.Serializable]
    public class GameAudioChannel
    {
        public AudioTrack track;
        public string mixerGroupName;   //must be the string in the mixerGroup
        public AudioMixerGroup mixerGroup;
        public AudioSource audioSource;
        public string volControlString;
        public int numVoicesAssigned;
    }

    [System.Serializable]
    public class VoiceItem
    {
        public AudioClip clip;
        public AudioTrack track;
        public AudioSource source;
        public bool loop;
        public float length;
        public float timeEnds;
        public int priority = 5;    //0 is highest
        public bool isPlaying = false;
        public bool isGarbage = false;
    }

    public enum AudioTrack { Master, SFX, Soundtrack, UI, Ambience };

    public enum AudioPlayType { Play, PlayLooped, PlayOneShot };

    

    [CreateAssetMenu(fileName = "audioManagerAsset", menuName = "Audio/(single) Audio Manager")]
    public class AudioManagerAsset : ScriptableObject
    {
        //private bool m_isInitialised = false;
        [SerializeField] private GameObject m_audioPrefab;
        private AudioManager m_instance;

        [Header("channels")]
        [SerializeField] private GameAudioChannel[] m_RawChannels = null;
        private Dictionary<AudioTrack,GameAudioChannel> m_channels = new Dictionary<AudioTrack, GameAudioChannel>();

        [Header("volume variables")]
        [SerializeField] private FloatVariable masterVolVar = null;
        [SerializeField] private FloatVariable soundtrackVolVar = null;

        #region activation
        public void InitialiseFromInstance(AudioManager manager)
        {
           m_instance = manager;
            SetupAudioSources(manager.gameObject);
            SetVolumesToMaxForStart();
            CompileChannelDictionary();

            //m_isInitialised = true;
        }

        private void SetupAudioSources(GameObject go)
        {
            foreach(GameAudioChannel channel in m_RawChannels)
            {
                channel.audioSource = go.AddComponent<AudioSource>();
                channel.audioSource.playOnAwake = false;
                channel.audioSource.volume = 1f;
                channel.audioSource.outputAudioMixerGroup = channel.mixerGroup;
            }
        }
        public void SetVolumesToMaxForStart()
        {
            masterVolVar.Value = 0f;
            soundtrackVolVar.Value = 0f;
        }
        private void CompileChannelDictionary()
        {
            foreach(GameAudioChannel channel in m_RawChannels)
            {
                m_channels.Add(channel.track, channel);
            }
        }

        public void Deactivate()
        {
            m_instance = null;
            //m_isInitialised = false;
        }
        #endregion

        #region channel changes
        public void SetChannelVolume(AudioTrack channel, float level)
        {
            level = Mathf.Clamp(level, -80f, 0f);
            m_channels[channel].mixerGroup.audioMixer.SetFloat(m_channels[channel].volControlString, level);
            masterVolVar.Value = level;
        }
        public void ToggleChannel(AudioTrack channel, bool on)
        {
            if(on)
            {
                SetChannelVolume(channel, 0f);
            }
            else
            {
                SetChannelVolume(channel, -80f);
            }
        }
        public void AdjustPitch(AudioTrack channel, float pitch)
        {
            pitch = Mathf.Clamp(pitch, 0f, 2f);
            m_channels[channel].audioSource.pitch = pitch;
        }
        #endregion

        public void Play(AudioCue cue)
        {
            cue.PlayOneShotToSource(m_channels[cue.GetTrack()].audioSource);
        }

        public void PlayFromSrc(AudioCue cue, AudioSource src)
        {
            if (src == null)
            {
                Play(cue);
            }

            cue.PlayClipToSource(src);
        }
    }
}

