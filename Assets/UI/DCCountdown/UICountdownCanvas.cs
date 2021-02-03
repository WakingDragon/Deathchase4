using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;
using TMPro;

namespace BP.UI
{
    public class UICountdownCanvas : MonoBehaviour
    {
        [SerializeField] private float m_interval = 1f;
        [SerializeField] private VoidGameEvent m_countdownCompleteEvent = null;

        [Header("dependencies")]
        [SerializeField] private TMP_Text m_TMProText = null;

        private void Awake()
        {
            if(!m_countdownCompleteEvent) { Debug.Log("no countdown event"); }
            if (!m_TMProText) { Debug.Log("no TMP text on countdown"); }

            m_TMProText.gameObject.SetActive(false);
        }

        public void StartCountdown()
        {
            StartCoroutine(DoCountdown());
        }

        private IEnumerator DoCountdown()
        {
            m_TMProText.text = "3";
            m_TMProText.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(m_interval);

            m_TMProText.text = "2";

            yield return new WaitForSecondsRealtime(m_interval);

            m_TMProText.text = "1";

            yield return new WaitForSecondsRealtime(m_interval);

            m_TMProText.text = "GO";

            yield return new WaitForSecondsRealtime(m_interval);

            m_TMProText.gameObject.SetActive(false);

            m_countdownCompleteEvent.Raise();
        }
    }
}

