using UnityEngine;

namespace RootMotion.Demos
{

    // Safely getting out of full screen desktop builds
    public class ApplicationQuit : MonoBehaviour
    {

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }
    }
}
