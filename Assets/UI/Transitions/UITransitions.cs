using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BP.Core;
using System;

namespace BP.UI
{
    public class UITransitions : MonoBehaviour
    {
        [Header("variables")]
        [SerializeField] private Texture m_shapeTex = null;
        [SerializeField] private Color m_color = Color.black;
        [SerializeField] private float m_startScale = 0f;
        [SerializeField] private float m_endScale = 20f;
        [SerializeField] private float m_fadeStart = 10f;
        [SerializeField] private bool m_invertMask = false;

        [Header("dependencies")]
        [SerializeField] private Image m_transitionPanel = null;
        [SerializeField] private CanvasGroup m_solidPanelGroup = null;
        [SerializeField] private Image m_solidPanelImage = null;
        [SerializeField] private FloatVariable m_transitionDuration = null;

        private void Start()
        {
            DefaultStatus();
        }

        public void StartTransition()
        {
            m_transitionPanel.material.SetTexture("_MaskTexture", m_shapeTex);
            m_transitionPanel.material.SetColor("_FadeColor", m_color);
            m_solidPanelImage.color = m_color;

            StopAllCoroutines();
            StartCoroutine(Transition(true, m_startScale, m_endScale));
        }

        public void EndTransition()
        {
            m_transitionPanel.material.SetTexture("_MaskTexture", m_shapeTex);
            m_transitionPanel.material.SetColor("_FadeColor", m_color);
            m_solidPanelImage.color = m_color;

            StopAllCoroutines();
            StartCoroutine(Transition(false, m_endScale, m_startScale));
        }

        private IEnumerator Transition(bool isStarting, float startScale, float endScale)
        {
            float scale;
            float transitionProgress;
            float fade;
            float timer = 0f;
            m_transitionPanel.gameObject.SetActive(true);

            

            //prep for start
            if (isStarting)
            {
                m_solidPanelGroup.gameObject.SetActive(true);
                m_solidPanelGroup.alpha = 0f;
            }
            else
            {
                m_solidPanelGroup.alpha = 1f;
            }

            //loop over changing scale during transition time
            while (timer <= m_transitionDuration.Value)
            {
                //transition sweep
                transitionProgress = timer / m_transitionDuration.Value;
                scale = transitionProgress * (endScale - startScale) + startScale;
                m_transitionPanel.material.SetFloat("_MaskScale", scale);

                //fade in panel
                if (isStarting && scale >= m_fadeStart)
                {
                    fade = (scale - m_fadeStart) / (endScale - m_fadeStart);
                    m_solidPanelGroup.alpha = fade;
                }
                if (!isStarting && scale <= m_fadeStart)
                {
                    fade = (scale - endScale) / (m_fadeStart - endScale);
                    m_solidPanelGroup.alpha = fade;
                }

                timer += m_transitionDuration.Value * Time.deltaTime * 1.1f;
                yield return null;
            }

            //prep for end
            if (isStarting)
            {
                m_solidPanelGroup.alpha = 1f;
            }
            else
            {
                m_solidPanelGroup.alpha = 0f;
                m_solidPanelGroup.gameObject.SetActive(false);
            }
            m_transitionPanel.gameObject.SetActive(false);
        }

        private void DefaultStatus()
        {
            m_transitionPanel.material.SetTexture("_MaskTexture", m_shapeTex);
            m_transitionPanel.material.SetColor("_FadeColor", m_color);
            m_transitionPanel.material.SetFloat("_MaskScale", m_startScale);
            m_transitionPanel.material.SetInt("_InvertMask", MaskInversion(m_invertMask));
            m_solidPanelImage.color = m_color;
            m_solidPanelGroup.alpha = 0f;

            m_solidPanelGroup.gameObject.SetActive(false);
            m_transitionPanel.gameObject.SetActive(false);
        }

        private int MaskInversion(bool invertMask)
        {
            if(m_invertMask)
            {
                return 1;
            }
            return 0;
        }
    }
}


