using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Deathchase
{
    public class DCCamFollow : MonoBehaviour
    {
        [SerializeField] private Vector3Var m_defaultOffset = null;
        [SerializeField] private float m_moveSpeed = 1f;
        [SerializeField] private float m_rotRate = 1f;
        private Transform m_lookTarget;
        private Transform m_moveTarget;

        private void LateUpdate()
        {
            if(m_lookTarget == null || m_moveTarget == null)
            {
                transform.position = new Vector3(
                    m_defaultOffset.Value.x,
                    m_defaultOffset.Value.y + 3f,
                    m_defaultOffset.Value.z
                    );
            }
            else
            {
                SmoothCamPosition();
            }
        }

        public void SetLookTarget(Transform lookTarget)
        {
            m_lookTarget = lookTarget;
        }

        public void SetMoveTarget(Transform moveTarget)
        {
            m_moveTarget = moveTarget;
        }

        private void SmoothCamPosition()
        {
            transform.position = Vector3.Lerp(transform.position, m_moveTarget.position, m_moveSpeed * Time.deltaTime);

            var lookRotation = Quaternion.LookRotation(m_lookTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, m_rotRate * Time.deltaTime);
        }
    }
}

