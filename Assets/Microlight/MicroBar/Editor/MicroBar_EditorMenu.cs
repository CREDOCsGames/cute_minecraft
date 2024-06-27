using UnityEditor;
using UnityEngine;
using Microlight.MicroEditor;

namespace Microlight.MicroBar {
    // ****************************************************************************************************
    // For MicroBar menu in editor or right clicking in hierarchy
    // ****************************************************************************************************
    internal class MicroBar_EditorMenu : Editor {

        static string GetPrefabsFolder() {
            return MicroEditor_AssetUtility.FindFolderRecursively("Assets", "MicroBar") + "/Prefabs";
        }
        static void InstantiateBar(GameObject bar) {
            bar = Instantiate(bar);   // Instantiate
            bar.name = "HealthBar";   // Change name
            if(Selection.activeGameObject != null) {   // Make child if some object is selected
                bar.transform.SetParent(Selection.activeGameObject.transform, false);
            }
        }

        #region Bars
        [MenuItem("GameObject/Microlight/MicroBar/Blank", false, 10)]
        private static void AddBlankBar() {
            // Get prefab
            GameObject go = MicroEditor_AssetUtility.GetPrefab(GetPrefabsFolder(), "Blank_MicroBar");
            if(go == null) return;
            InstantiateBar(go);
        }
        [MenuItem("GameObject/Microlight/MicroBar/Simple", false, 20)]
        private static void AddSimpleBar() {
            // Get prefab
            GameObject go = MicroEditor_AssetUtility.GetPrefab(GetPrefabsFolder(), "Simple_MicroBar");
            if(go == null) return;
            InstantiateBar(go);
        }
        [MenuItem("GameObject/Microlight/MicroBar/Delayed", false, 30)]
        private static void AddDelayedBar() {
            // Get prefab
            GameObject go = MicroEditor_AssetUtility.GetPrefab(GetPrefabsFolder(), "Delayed_MicroBar");
            if(go == null) return;
            InstantiateBar(go);
        }
        [MenuItem("GameObject/Microlight/MicroBar/Disappear", false, 40)]
        private static void AddDisappearBar() {
            // Get prefab
            GameObject go = MicroEditor_AssetUtility.GetPrefab(GetPrefabsFolder(), "Disappear_MicroBar");
            if(go == null) return;
            InstantiateBar(go);
        }
        [MenuItem("GameObject/Microlight/MicroBar/Impact", false, 50)]
        private static void AddImpactBar() {
            // Get prefab
            GameObject go = MicroEditor_AssetUtility.GetPrefab(GetPrefabsFolder(), "Impact_MicroBar");
            if(go == null) return;
            InstantiateBar(go);
        }
        [MenuItem("GameObject/Microlight/MicroBar/Punch", false, 60)]
        private static void AddPunchBar() {
            // Get prefab
            GameObject go = MicroEditor_AssetUtility.GetPrefab(GetPrefabsFolder(), "Punch_MicroBar");
            if(go == null) return;
            InstantiateBar(go);
        }
        [MenuItem("GameObject/Microlight/MicroBar/Shake", false, 70)]
        private static void AddShakeBar() {
            // Get prefab
            GameObject go = MicroEditor_AssetUtility.GetPrefab(GetPrefabsFolder(), "Shake_MicroBar");
            if(go == null) return;
            InstantiateBar(go);
        }
        //[MenuItem("GameObject/Microlight/UI Image Health Bar", true)]
        //private static bool AddImageHealthBar_Validate() {
        //    return Selection.activeGameObject && Selection.activeGameObject.GetComponentInParent<Canvas>();
        //}
        #endregion
    }
}