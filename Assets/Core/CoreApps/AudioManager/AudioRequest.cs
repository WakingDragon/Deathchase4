using UnityEngine;

namespace BP.Core
{
    [System.Serializable]
    public class AudioRequest
    {
        public AudioCue m_cue;
        public AudioSource m_src;

        public AudioRequest(AudioCue cue, AudioSource src)
        {
            m_cue = cue;
            m_src = src;
        }
    }
}
