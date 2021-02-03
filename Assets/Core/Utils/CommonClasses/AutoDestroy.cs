using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] float m_lifespan = 2f;
        private float m_timeToDie;

        private void Start()
        {
            m_timeToDie = Time.time + m_lifespan;
        }

        private void Update()
        {
            if (m_timeToDie <= Time.time)
            {
                Destroy(gameObject);
            }
        }
        public void SetLifespan(float lifespan) { m_lifespan = lifespan; }
    }
}

