using System.Collections.Generic;
using UnityEngine;
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

        public void LoadFilm(Film film)
        {
            SceneManager.LoadSceneAsync(film.Name,LoadSceneMode.Additive);
        }
        public void UnloadFilm(Film film)
        {
            SceneManager.UnloadSceneAsync(film.Name);
        }
        public void DisableAllCamerasInScene()
        {
            _controls.Clear();
            foreach (var camera in Camera.allCameras)
            {
                _controls.Add(new() { Camera = camera, Active = camera.gameObject.activeSelf });
                camera.gameObject.SetActive(false);
            }
        }
        public void EnableAllCamerasInScene()
        {
            foreach (var control in _controls)
            {
                control.Camera.gameObject.SetActive(control.Active);

            }
            _controls.Clear();
        }
    }

}