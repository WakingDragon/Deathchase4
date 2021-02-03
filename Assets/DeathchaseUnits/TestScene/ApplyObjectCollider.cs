using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

public class ApplyObjectCollider : MonoBehaviour, ICollidable
{
    [SerializeField] private bool m_setupOnAwake = true;
    [SerializeField] private bool m_isKinematic = true;
    [SerializeField] private bool m_isTrigger = true;
    [SerializeField] private bool m_useGravity = false;
    private Rigidbody m_rb;
    private BoxCollider m_collider;

    private void Awake()
    {
        if(m_setupOnAwake) { Setup(); }
    }

    private void Setup()
    {
        m_rb = GetComponent<Rigidbody>();
        if(!m_rb) { m_rb = gameObject.AddComponent<Rigidbody>(); }
        m_rb.useGravity = m_useGravity;
        m_rb.isKinematic = m_isKinematic;

        m_collider = GetComponent<BoxCollider>();
        if (!m_collider) { m_collider = gameObject.AddComponent<BoxCollider>(); }
        m_collider.isTrigger = m_isTrigger;
    }
}
