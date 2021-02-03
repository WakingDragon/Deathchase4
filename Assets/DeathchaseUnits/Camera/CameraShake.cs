using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Deathchase
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float m_duration = 0.5f;
        [SerializeField] private float m_magnitude = 3f;

        public void Shake()
        {
            StartCoroutine(DoShake(m_duration, m_magnitude));
        }

        private IEnumerator DoShake(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;
            float elapsed = 0f;
            while(elapsed < duration)
            {
                float x = Random.Range(-1f, 1f);
                float y = Random.Range(-1f, 1f);
                float z = Random.Range(-1f, 1f);

                transform.localPosition = new Vector3(x, y, z);

                elapsed += Time.deltaTime;

                yield return null;
            }
            transform.localPosition = originalPos;
        }
    }
}

