using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;
using TMPro;
using BP.Deathchase;

namespace BP.UI
{
    public class UILevelIntroCanvas : MonoBehaviour
    {
        [Header("dependencies")]
        //[SerializeField] private StringVariable m_levelTitleText = null;
        [SerializeField] private CanvasGroup m_canvasGroup = null;
        [SerializeField] private TMP_Text m_TMProText = null;
        //[SerializeField] private DCLevelAsset m_currentLevel = null;

        private void Awake()
        {
            //if (!m_levelTitleText) { Debug.Log("no level title stringvar on countdown"); }
            if (!m_canvasGroup) { Debug.Log("no canvas group"); }
            if (!m_TMProText) { Debug.Log("no TMP text"); }

            //m_TMProText.gameObject.SetActive(false);
            m_canvasGroup.gameObject.SetActive(false);
        }

        public void ShowIntro(DCLevelAsset levelAsset)
        {
            m_TMProText.text = levelAsset.LevelName();
            m_canvasGroup.gameObject.SetActive(true);
        }

        public void CloseIntro()
        {
            m_canvasGroup.gameObject.SetActive(false);
        }
    }
}

