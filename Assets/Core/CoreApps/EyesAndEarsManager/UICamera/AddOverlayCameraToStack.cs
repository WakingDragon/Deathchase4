using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BP.Core
{
    public class AddOverlayCameraToStack : MonoBehaviour
    {
        [SerializeField] private Camera m_mainCam = null;
        

        public void AddOverlayCamera(Camera overlayCam)
        {
            var cameraData = m_mainCam.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(overlayCam);
        }
    }
}


