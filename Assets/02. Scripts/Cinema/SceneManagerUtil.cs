using System.IO;
using UnityEngine.SceneManagement;

namespace Util
{
    public static class SceneManagerUtil
    {
        public static bool IsSceneUnvalid(string sceneName)
        {
            return false;
            // TODO
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++)
            {
                var path = SceneUtility.GetScenePathByBuildIndex(i);
                var scene = Path.GetFileNameWithoutExtension(path);
                if (scene == sceneName)
                {
                    return false;
                }
            }
            return true;
        }
    }

}
