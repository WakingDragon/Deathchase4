using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BP.Core
{
    [CreateAssetMenu(fileName = "eyesAndEarsLibrary", menuName ="Core/Cameras/(single) EyesAndEarsLibrary")]
    public class EyesAndEarsLibrary : ScriptableObject
    {
        [SerializeField] private List<Camera> mainCams = new List<Camera>();
        [SerializeField] private List<Camera> overlayCams = new List<Camera>();

        public void Initialise()
        {
            mainCams.Clear();
            overlayCams.Clear();
        }

        public void AddCamera(Camera cam)
        {
            var camData = cam.GetUniversalAdditionalCameraData();
            if(camData.renderType == CameraRenderType.Overlay)
            {
                cam.cullingMask = GameLayers.layerMask(GameLayer.OverlayCamera);
                if (!overlayCams.Contains(cam)) { overlayCams.Add(cam); }
                InjectOverlayToActiveMainCams(cam);
            }
            else
            {
                if (!mainCams.Contains(cam)) { mainCams.Add(cam); }
                InjectOverlayCameras(camData);
                UpdateMainCamsActiveStatus(cam);
                FilterDeadCams();
            }
        }

        private void InjectOverlayToActiveMainCams(Camera overlayCam)
        {
            foreach(Camera cam in mainCams)
            {
                if(cam.gameObject.activeSelf)
                {
                    var camData = cam.GetUniversalAdditionalCameraData();
                    if (!camData.cameraStack.Contains(overlayCam))
                    {
                        camData.cameraStack.Add(overlayCam);
                    }
                }
            }
        }

        private void InjectOverlayCameras(UniversalAdditionalCameraData camData)
        {
            foreach(Camera overlayCam in overlayCams)
            {
                if(overlayCam != null)
                {
                    if(!camData.cameraStack.Contains(overlayCam))
                    {
                        camData.cameraStack.Add(overlayCam);
                    }
                }
            }
        }

        private void UpdateMainCamsActiveStatus(Camera activeCam)
        {
            foreach(Camera cam in mainCams)
            {
                if(cam == activeCam)
                {
                    cam.gameObject.SetActive(true);
                }
                else
                {
                    if (cam != null) { cam.gameObject.SetActive(false); }
                }
            }
        }

        public void FilterDeadCams()
        {
            List<Camera> camsToDelete = new List<Camera>();

            for (int i = 0; i < mainCams.Count; i++)
            {
                if (mainCams[i] == null)
                {
                    camsToDelete.Add(mainCams[i]);
                }
            }

            if (camsToDelete.Count > 0)
            {
                foreach (Camera cam in camsToDelete)
                {
                    if(mainCams.Contains(cam)) { mainCams.Remove(cam); }
                }
            }

            camsToDelete.Clear();
        }
    }
}

