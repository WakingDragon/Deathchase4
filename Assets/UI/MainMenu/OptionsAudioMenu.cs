using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BP.Core
{
    public class OptionsAudioMenu : MonoBehaviour
    {
        [Header("UI elements")]
        [SerializeField] private Slider masterVolSlider = null;
        [SerializeField] private Slider soundtrackVolSlider = null;

        [Header("dependencies")]
        [SerializeField] private FloatVariable masterVolVar = null;
        [SerializeField] private FloatVariable soundtrackVolVar = null;
        [SerializeField] private FloatGameEvent notifyMasterVolChange = null;
        [SerializeField] private FloatGameEvent notifySoundtrackVolChange = null;

        private void OnEnable()
        {
            SetListenersForSliders();
            SetSlidersToStoredValues();
        }

        public void AdjustMasterVolume(float newVol)
        {
            notifyMasterVolChange.Raise(newVol);
        }

        public void AdjustSoundtrackVolume(float newVol)
        {
            notifySoundtrackVolChange.Raise(newVol);
        }

        private void SetListenersForSliders()
        {
            masterVolSlider.onValueChanged.AddListener(delegate { AdjustMasterVolume(masterVolSlider.value); });
            soundtrackVolSlider.onValueChanged.AddListener(delegate { AdjustSoundtrackVolume(soundtrackVolSlider.value); });
        }

        private void SetSlidersToStoredValues()
        {
            masterVolSlider.value = masterVolVar.Value;
            soundtrackVolSlider.value = soundtrackVolVar.Value;
        }
    }
}

