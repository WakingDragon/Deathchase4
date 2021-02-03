using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class EventToggleGOActive : MonoBehaviour
    {
        [SerializeField] private GameObject objectToToggle = null;

        private void Awake()
        {
            if(!objectToToggle) { Debug.Log("need a GO to toggle: " + gameObject.name); }
        }

        public void ToggleActiveStatus(bool isActive)
        {
            if(!objectToToggle) { return; }

            objectToToggle.SetActive(isActive);
        }

        public void MakeGameobjectActive()
        {
            if (!objectToToggle) { return; }

            objectToToggle.SetActive(true);
        }

        public void MakeGameobjectInactive()
        {
            if (!objectToToggle) { return; }

            objectToToggle.SetActive(false);
        }
    }
}

