using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Cinema
{
    public class CameraUtil
    {
        private struct CameraControl
        {
            public Camera Camera;
            public bool Active;
        }
        private readonly List<CameraControl> _controls = new();
        private EventSystem _mainEventSystem;

        public void DisableAllCamerasInScene()
        {
            _controls.Clear();
            foreach (var camera in Camera.allCameras)
            {

                _controls.Add(new() { Camera = camera, Active = camera.gameObject.activeSelf });
                camera.gameObject.SetActive(false);
            }
            _mainEventSystem = EventSystem.current;
            if (_mainEventSystem != null)
            {
                _mainEventSystem.enabled = false;
            }
        }
        public void EnableAllCamerasInScene()
        {
            foreach (var control in _controls)
            {
                if (control.Camera == null)
                {
                    continue;
                }
                control.Camera.gameObject.SetActive(control.Active);

            }
            _controls.Clear();
            if (_mainEventSystem != null)
            {
                _mainEventSystem.enabled = true;
            }
        }
    }

}