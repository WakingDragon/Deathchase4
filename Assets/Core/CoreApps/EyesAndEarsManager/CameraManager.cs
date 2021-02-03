using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

namespace BP.Core
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private EyesAndEarsLibrary camLibrary = null;

        private void Awake()
        {
            if (!camLibrary) { Debug.Log("no camera library on manager"); }
            camLibrary.Initialise();
        }

        private void OnDisable()
        {
            camLibrary.Initialise();
        }

        public void NewCameraEnabled(Camera newCam)
        {
            camLibrary.AddCamera(newCam);
        }
    }
}

