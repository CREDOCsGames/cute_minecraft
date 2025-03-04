﻿using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{

    /// <summary>
    /// Simple GUI for quickly testing out interactions.
    /// </summary>
    public class InteractionSystemTestGUI : MonoBehaviour
    {

        [Tooltip("The object to interact to")]
        public InteractionObject interactionObject;
        [Tooltip("The effectors to interact with")]
        public FullBodyBipedEffector[] effectors;

        private InteractionSystem interactionSystem;

        void Awake()
        {
            interactionSystem = GetComponent<InteractionSystem>();
        }

        void OnGUI()
        {
            if (interactionSystem == null) return;

            if (GUILayout.Button("Start Interaction With " + interactionObject.name))
            {
                if (effectors.Length == 0) Debug.Log("Please select the effectors to interact with.");

                foreach (FullBodyBipedEffector e in effectors)
                {
                    interactionSystem.StartInteraction(e, interactionObject, true);
                }
            }

            if (effectors.Length == 0) return;

            if (interactionSystem.IsPaused(effectors[0]))
            {
                if (GUILayout.Button("Resume Interaction With " + interactionObject.name))
                {

                    interactionSystem.ResumeAll();
                }
            }
        }
    }
}
