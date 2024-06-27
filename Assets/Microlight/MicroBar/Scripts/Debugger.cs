using UnityEngine;

namespace Microlight.MicroBar {
    // ****************************************************************************************************
    // Base script for MicroBar, holds base functionality of health bar
    // Manager for other features added to the health bar
    // ****************************************************************************************************
    internal static class Debugger {
        static bool DEBUGGER => false;
        static bool ERROR => true;
        static bool WARNING => true;
        static bool MINOR_WARNING => true;
        static bool LOGS => true;
        static bool ALL_LOGS => false;

        static void Log(string message) {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        #region Warnings
        internal static void MissingBarReference() {
            if(!DEBUGGER) return;
            if(!WARNING) return;
            //return;

            Debug.LogWarning($"MicroBar: Animation has no reference to the MicroBar.");
        }
        internal static void InitializationFailed(MicroBar bar) {
            if(!DEBUGGER) return;
            if(!WARNING) return;
            //return;

            Debug.LogWarning($"MicroBar: Initialization for the { bar.gameObject.name } failed.");
        }
        internal static void NoImage() {
            if(!DEBUGGER) return;
            if(!WARNING) return;
            //return;

            Debug.LogWarning($"MicroBar: Micro bar animation has no.");
        }
        internal static void UnknownAnimExecution(AnimExecution execution) {
            if(!DEBUGGER) return;
            if(!WARNING) return;
            //return;

            Debug.LogWarning($"MicroBar: Unknown animation execution: {execution}.");
        }
        internal static void UnknownAnimEffect(AnimEffect effect) {
            if(!DEBUGGER) return;
            if(!WARNING) return;
            //return;

            Debug.LogWarning($"MicroBar: Unknown animation effect: {effect}.");
        }

        internal static void UnknownValueMode(ValueMode mode) {
            if(!DEBUGGER) return;
            if(!WARNING) return;
            //return;

            Debug.LogWarning($"MicroBar: Unknown value mode: {mode}.");
        }
        internal static void UnknownAnimAxis(AnimAxis axis) {
            if(!DEBUGGER) return;
            if(!WARNING) return;
            //return;

            Debug.LogWarning($"MicroBar: Unknown anim axis: {axis}.");
        }
        internal static void UnknownTransformProperty(TransformProperties property) {
            if(!DEBUGGER) return;
            if(!WARNING) return;
            //return;

            Debug.LogWarning($"MicroBar: Unknown transform property: {property}.");
        }
        #endregion

        #region Minor warnings
        internal static void NotInitialized() {
            if(!DEBUGGER) return;
            if(!MINOR_WARNING) return;
            //return;

            Debug.LogWarning($"MicroBar: Your MicroBar bar has not been initialized so it can not be interacted with.\nInitialize the bar to interact with it.");
        }
        #endregion

        #region Logs
        internal static void UpdatedMaxHP(float newMaxHP) {
            if(!DEBUGGER) return;
            if(!LOGS) return;
            //return;

            Log($"MicroBar: Updated max HP to {newMaxHP}.");
        }
        internal static void SilentUpdate() {
            if(!DEBUGGER) return;
            if(!LOGS) return;
            //return;

            Log($"MicroBar: Bar has been silently updated.");
        }
        #endregion

        #region All Logs
        internal static void UpdatedCurrentHP(float newCurrentHP) {
            if(!DEBUGGER) return;
            if(!ALL_LOGS) return;
            //return;

            Log($"MicroBar: Updated current HP to {newCurrentHP}.");
        }
        internal static void DestroyedBar(MicroBar bar) {
            if(!DEBUGGER) return;
            if(!ALL_LOGS) return;
            //return;

            Log($"MicroBar: {bar.gameObject.name} has been destroyed.");
        }
        internal static void InitializedBar(MicroBar bar) {
            if(!DEBUGGER) return;
            if(!ALL_LOGS) return;
            //return;

            Log($"MicroBar: {bar.gameObject.name} has been initialized.");
        }
        internal static void DefaultValuesSnapshoted() {
            if(!DEBUGGER) return;
            if(!ALL_LOGS) return;
            //return;

            Log($"MicroBar: Default values have been snapshoted.");
        }
        #endregion
    }
}