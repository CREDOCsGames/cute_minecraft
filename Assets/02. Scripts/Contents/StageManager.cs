using PlatformGame.Util;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlatformGame.Util.ListHelper;

namespace PlatformGame.Contents
{
    [CreateAssetMenu(menuName = "Custom/Data/StageManager")]
    public class StageManager : UniqueScriptableObject<StageManager>
    {
        public string Stage => SceneManager.GetActiveScene().name == TitleScene ?
                               mStages.Matrix[StageIndex.y].List[StageIndex.x] :
                               TitleScene;

        [SerializeField] MatrixBool mStageOpens;
        [SerializeField] MatrixString mStages;
        [SerializeField] Vector2Int mStageIndex;
        [SerializeField] string mTitleScene;
        string TitleScene
        {
            get
            {
                Debug.Assert(!string.IsNullOrEmpty(mTitleScene));
                return mTitleScene;
            }
        }
        Vector2Int StageIndex
        {
            get => mStageIndex;
            set
            {
                value.y = Mathf.Clamp(value.y, 0, mStages.Matrix.Count - 1);
                value.x = Mathf.Clamp(value.x, 0, mStages.Matrix[value.y].List.Count - 1);

                var success = IsOpenStage(value);
                if (success)
                {
                    mStageIndex = value;
                }
            }
        }

        public void NextStage()
        {
            MoveStageIndex(new(1, 0));
        }

        public void PrevStage()
        {
            MoveStageIndex(new(-1, 0));
        }

        public void NextMode()
        {
            MoveStageIndex(new(-int.MaxValue, 1));
        }

        public void PrevMode()
        {
            MoveStageIndex(new(-int.MaxValue, -1));
        }

        public void OpenStage(Vector2Int stageIndex)
        {
            Debug.Assert(!IsOutOfRange(mStageOpens.Matrix, stageIndex.y));
            Debug.Assert(!IsOutOfRange(mStageOpens.Matrix[stageIndex.y].List, stageIndex.x));
            mStageOpens.Matrix[stageIndex.y].List[stageIndex.x] = true;
        }

        public void CloseStage(Vector2Int stageIndex)
        {
            Debug.Assert(!IsOutOfRange(mStageOpens.Matrix, stageIndex.y));
            Debug.Assert(!IsOutOfRange(mStageOpens.Matrix[stageIndex.y].List, stageIndex.x));
            mStageOpens.Matrix[stageIndex.y].List[stageIndex.x] = false;
        }

        bool IsOpenStage(Vector2Int stageIndex)
        {
            return mStageOpens.Matrix[stageIndex.y].List[stageIndex.x];
        }

        bool MoveStageIndex(Vector2Int stageIndex)
        {
            var nextStageIndex = StageIndex;
            nextStageIndex += stageIndex;
            StageIndex = nextStageIndex;

            return StageIndex == nextStageIndex;
        }

#if DEVELOPMENT
        public void SetText(TextMeshProUGUI ui)
        {
            ui.text = Stage;
        }
#endif
    }
}