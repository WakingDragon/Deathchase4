using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;
using BP.Core;
using System;

namespace BP.Units.Weapons
{
    [CreateAssetMenu(fileName = "new_FXAsset", menuName = "FX/FX Asset")]
    public class FXAsset : ScriptableObject, IPoolableAsset
    {
        [SerializeField] private GameObject m_prefab = null;
        //private ObjectPoolAsset m_pool;

        [Header("sfx")]
        [SerializeField] private AudioCue onStartSFX = null;
        [SerializeField] private bool onStartSFXIsLocal = false;

        [Header("Pooling settings")]
        [SerializeField] private int m_defaultNumberToPool = 5;
        [SerializeField] private bool m_expandablePool = true;
        [SerializeField] private float m_autoRepoolLife = 2f;

        #region FX
        public void Play(Vector3 worldPos, ObjectPoolAsset pool)
        {
            GameObject go = GenerateFXGO(worldPos, pool);
            if (!go) { Debug.Log("no go created "); return; }
            PlayOnStartSFX(go);
        }

        public void Play(Transform p, Vector3 localPos, ObjectPoolAsset pool)
        {
            GameObject go = GenerateFXGO(p.position + localPos, pool);
            if(!go) { Debug.Log("no go created "); return; }
            go.transform.parent = p;
            go.transform.localPosition = localPos;
            go.transform.localRotation = Quaternion.Euler(p.forward);
            PlayOnStartSFX(go);
        }

        private GameObject GenerateFXGO(Vector3 worldPos, ObjectPoolAsset pool)
        {
            GameObject go;

            if (!pool)
            {
                go = Instantiate(m_prefab);
                go.transform.position = worldPos;

                if(m_autoRepoolLife > 0f)
                {
                    go.AddComponent<AutoDestroy>().SetLifespan(m_autoRepoolLife);
                }
            }
            else
            {
                go = pool.GetObjectFromPool(this, worldPos, Quaternion.identity);
            }

            return go;
        }

        private void PlayOnStartSFX(GameObject go)
        {
            if(onStartSFX)
            { 
                if (onStartSFXIsLocal)
                {
                    var src = go.GetComponent<AudioSource>();
                    if (!src) { src = go.AddComponent<AudioSource>(); }
                    onStartSFX.Play(src);
                }
                else
                {
                    onStartSFX.Play();
                }
            }
        }
        #endregion

        #region IPoolable
        public string GetName() { return this.name.ToString(); }
        public IPoolableAsset GetIPoolableAsset() { return this; }
        public int GetNumberToPool() { return m_defaultNumberToPool; }
        public bool GetIsPoolExpandable() { return m_expandablePool; }

        public GameObject InstantiatePrefabToPool(Transform parent)
        {
            //This can be used to put in very unique elements to pooled object spawning
            GameObject go = Instantiate(m_prefab, parent);
            //InitialiseAmmoGO(go);
            return go;
        }

        public float AutoRepool() { return m_autoRepoolLife; }
        #endregion

    }
}

