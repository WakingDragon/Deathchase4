using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class CameraNotifier : MonoBehaviour
    {
        //always attach to same GO as the camera component
        [SerializeField] private CameraGameEvent addMainCamEvent = null;
        private Camera cam;

        private void OnEnable()
        {
            if(!addMainCamEvent) 
            { Debug.Log("main cam notification game events missing from " + gameObject.name); }

            cam = GetComponent<Camera>();
            if (!cam) { Debug.Log("MainCamera attached to go without camera"); }

            addMainCamEvent.Raise(cam);
        }
    }
}

