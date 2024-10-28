using UnityEngine.SceneManagement;
using Util;

namespace Flow
{
    internal class ReLoader : ILevelLoader
    {
        public WorkState State { get; private set; } = WorkState.Ready;

        public void LoadNext()
        {
            State = WorkState.Action;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            CoroutineRunner.InvokeDelayAction(() => State = WorkState.Ready, 1f);
        }

    }
}