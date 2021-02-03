using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.ObjectPooling
{
    public class ObjectRepool : MonoBehaviour
    {
        private IPoolableAsset m_asset;
        private ObjectPoolAsset m_pool;
        private bool m_autoRepool = false;
        private float m_repoolTime;

        public void Initialise(IPoolableAsset asset, ObjectPoolAsset pool)
        {
            m_asset = asset;
            m_pool = pool;
        }

        public void Activate()
        {
            if (m_asset.AutoRepool() > 0f) 
            { 
                SetLifetime(m_asset.AutoRepool());
                m_autoRepool = true;
            }
        }

        private void LateUpdate()
        {
            if (m_autoRepool && Time.time > m_repoolTime)
            {
                Repool();
            }
        }

        private void SetLifetime(float repoolInterval)
        {
            m_repoolTime = Time.time + repoolInterval;
        }

        public void Repool()
        {
            m_pool.Repool(gameObject);
        }
    }
}

