//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Audio;

//namespace BP.Core
//{


//    public class DelGameAudioMgr : MonoBehaviour
//    {

//        private string masterAudioName = "Master";  //these MUST match the mixerGroupName in the array
//        private string sfxAudioName = "SFX";
//        private string soundtrackAudioName = "Soundtrack";
//        private string uiAudioName = "UI";
//        private string ambienceAudioName = "Ambience";
//        private AudioSource masterSource;           //need one of these for each channel
//        private AudioSource sfxSource;
//        private AudioSource soundtrackSource;
//        private AudioSource uiSource;
//        private AudioSource ambienceSource;
//        [SerializeField] private List<VoiceItem> voices = new List<VoiceItem>();
//        private List<VoiceItem> voicesToRemove = new List<VoiceItem>();
//        private string MasterVol = "MasterVol";
//        private string SoundtrackVol = "SoundtrackVol";

//        private bool soundtrackIsOn = true;

//        private int highestPriority = 0;
//        private int lowestPriority = 5;


//        #region NEW play audio generic method with different overloads
//        public void PlayAudio(AudioCue cue, AudioPlayType type, int priority, AudioSource source)   //played from the supplied source
//        {
//            var item = CreateNewVoiceItemFromCue(cue, type, priority);

//            item.source = source;

//            AttemptAddSoundToList(item);
//            ReviewSoundList();
//        }

//        public void PlayAudio(AudioCue cue, AudioPlayType type, int priority)   //played from generic sources on cam
//        {
//            var item = CreateNewVoiceItemFromCue(cue, type, priority);

//            item.source = GetAudioSourceForTrack(cue.GetTrack());       //equivalent for source-supplied sounds is to find mixer group and assign source to it

//            AttemptAddSoundToList(item);
//            ReviewSoundList();
//        }

//        private VoiceItem CreateNewVoiceItemFromCue(AudioCue cue, AudioPlayType type, int priority)
//        {
//            priority = Mathf.Clamp(priority, highestPriority, lowestPriority);

//            var newClip = cue.SelectClip();

//            VoiceItem item = new VoiceItem
//            {
//                clip = newClip,
//                track = cue.GetTrack(),
//                source = null,      //to be assigned
//                loop = IsLooping(type),
//                length = newClip.length,
//                timeEnds = Time.time + newClip.length,
//                priority = priority,
//                isPlaying = false,
//                isGarbage = false
//            };

//            return item;
//        }

//        private bool IsLooping(AudioPlayType type)
//        {
//            if (type == AudioPlayType.PlayLooped)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private AudioSource GetAudioSourceForTrack(AudioTrack track)
//        {
//            AudioSource source;

//            switch (track)
//            {
//                case AudioTrack.Master:
//                    source = masterSource;
//                    break;

//                case AudioTrack.Soundtrack:
//                    source = soundtrackSource;
//                    break;

//                case AudioTrack.UI:
//                    source = uiSource;
//                    break;

//                case AudioTrack.Ambience:
//                    source = ambienceSource;
//                    break;

//                case AudioTrack.SFX:
//                    source = sfxSource;
//                    break;

//                default:
//                    source = sfxSource;
//                    break;
//            }

//            return source;
//        }

//        private void SetAudioTimeEnds(VoiceItem item)
//        {
//            item.timeEnds = Time.time + item.length;
//        }

//        private void PlayVoiceItem(VoiceItem item)
//        {
//            if (item.source.outputAudioMixerGroup == null)
//            {
//                SetAudioSourceToSFXChannel(item.source, item.track);
//            }

//            if (item.loop)
//            {
//                item.source.clip = item.clip;
//                item.source.loop = true;
//                item.source.Play();
//            }
//            else
//            {
//                item.source.PlayOneShot(item.clip);
//            }
//        }

//        private void StopVoiceItem(VoiceItem item)
//        {
//            item.isGarbage = true;

//            if (item.loop)
//            {
//                if (item.source != null) { item.source.Stop(); }
//            }
//            else
//            {
//                //no way to stop one-shots
//            }
//        }
//        #endregion

//        #region sound list manager
//        public void StopAllSoundsOnTrack(AudioTrack track)
//        {
//            bool takeOutGarbage = false;
//            foreach (VoiceItem voice in voices)
//            {
//                if (voice.track == track)
//                {
//                    voice.isGarbage = true;
//                    if (voice.source != null) { voice.source.Stop(); }
//                    takeOutGarbage = true;
//                }
//            }

//            if (takeOutGarbage)
//            {
//                ClearGarbageFromList();
//            }
//        }

//        public void StopAllSoundsFromSource(AudioSource source)
//        {
//            bool takeOutGarbage = false;
//            foreach (VoiceItem voice in voices)
//            {
//                if (voice.source == source)
//                {
//                    voice.isGarbage = true;
//                    takeOutGarbage = true;
//                }
//            }

//            if (takeOutGarbage)
//            {
//                source.Stop();
//                ClearGarbageFromList();
//            }
//        }

//        private void AttemptAddSoundToList(VoiceItem item)
//        {
//            //count number of voices in list that are in right list
//            AudioTrack track = item.track;
//            int numberOfTrackVoices = 0;

//            //straight add voice
//            foreach (VoiceItem voice in voices)
//            {
//                if (voice.track == item.track)
//                {
//                    numberOfTrackVoices++;
//                }
//            }

//            if (numberOfTrackVoices < GetNumberOfVoicesAssignedToChannel(item.track))
//            {
//                AddSoundToList(item);
//                return;
//            }

//            //check if voices are lower priority
//            int lowestPriorityFound = highestPriority;
//            int indexOfLowestPriorityFound = -1;

//            for (int i = 0; i < voices.Count; i++)
//            {
//                if (voices[i].track == item.track)
//                {
//                    if (voices[i].priority < item.priority && voices[i].priority > lowestPriorityFound)
//                    {
//                        indexOfLowestPriorityFound = i;
//                    }
//                }
//            }

//            if (indexOfLowestPriorityFound > -1)
//            {
//                RemoveSoundFromList(voices[indexOfLowestPriorityFound]);
//                AddSoundToList(item);
//            }

//            //Debug.Log("number of voices assigned to channel: " + GetNumberOfVoicesAssignedToChannel(item.track));
//            //Debug.Log(numberOfTrackVoices);
//        }

//        private void ReviewSoundList()
//        {
//            foreach (VoiceItem voice in voices)
//            {
//                if (voice.loop == false && voice.timeEnds < Time.time)
//                {
//                    voice.isGarbage = true;
//                }
//            }

//            ClearGarbageFromList();
//        }

//        private void ClearGarbageFromList()
//        {
//            for (int i = voices.Count - 1; i >= 0; i--)
//            {
//                if (voices[i].isGarbage)
//                {
//                    RemoveSoundFromList(voices[i]);
//                }
//            }
//        }

//        private void AddSoundToList(VoiceItem item)
//        {
//            voices.Add(item);
//            SetAudioTimeEnds(item);
//            PlayVoiceItem(item);
//        }

//        private void RemoveSoundFromList(VoiceItem item)
//        {
//            voices.Remove(item);
//            StopVoiceItem(item);
//        }
//        #endregion

//        #region queries to channel list
//        private AudioMixerGroup GetAudioGroupRef(string audioGroupName)
//        {
//            for (int i = 0; i < channels.Length; i++)
//            {
//                if (channels[i].mixerGroupName == audioGroupName) { return channels[i].mixerGroup; }
//            }
//            return null;
//        }

//        private int GetNumberOfVoicesAssignedToChannel(AudioTrack track)
//        {
//            foreach (GameAudioChannel channel in channels)
//            {
//                if (channel.mixerGroupName == track.ToString())
//                {
//                    return channel.numVoicesAssigned;
//                }
//            }
//            return 0;
//        }
//        #endregion

//    }
//}

