using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BP.Core
{
    public class OverlayCamera : MonoBehaviour
    {
        [SerializeField] private CameraGameEvent m_notifyNewOverlayCamEvent = null;
        private Camera m_overlayCam;
        //private UniversalAdditionalCameraData m_cameraData;

        private void Start()
        {
            m_overlayCam = GetComponent<Camera>();
            m_notifyNewOverlayCamEvent.Raise(m_overlayCam);
            SetupCamera();
        }

        private void SetupCamera()
        {
            //m_cameraData = m_overlayCam.GetUniversalAdditionalCameraData();
            m_overlayCam.cullingMask = GameLayers.layerMask(GameLayer.OverlayCamera);
        }
    }
}

