using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using static Util.ListHelper;

namespace Flow
{
    [CreateAssetMenu(menuName = "Custom/Data/StageManager")]
    public class StageManager : ScriptableObject
    {
        public static StageManager Instance => Resources.Load<StageManager>("Stage/StageManager");

        public string Stage => SceneManager.GetActiveScene().name == TitleScene
            ? mStages.Matrix[StageIndex.y].List[StageIndex.x]
            : TitleScene;

        [SerializeField] MatrixBool mStageOpens;
        [SerializeField] MatrixString mStages;
        [SerializeField] Vector2Int mStageIndex;
        [SerializeField] string mTitleScene;

        public event Action OnChangeEvent;

        string TitleScene
        {
            get
            {
                Debug.Assert(!string.IsNullOrEmpty(mTitleScene));
                return mTitleScene;
            }
        }

        public Vector2Int StageIndex
        {
            get => mStageIndex;
            private set
            {
                value.y = Mathf.Clamp(value.y, 0, mStages.Matrix.Count - 1);
                value.x = Mathf.Clamp(value.x, 0, mStages.Matrix[value.y].List.Count - 1);

                if (!IsOpenStage(value) || mStageIndex.Equals(value))
                {
                    return;
                }

                mStageIndex = value;
                OnChangeEvent?.Invoke();
            }
        }

        public void NextStage()
        {
            MoveStageIndex(new Vector2Int(1, 0));
        }

        public void PrevStage()
        {
            MoveStageIndex(new Vector2Int(-1, 0));
        }

        public void NextMode()
        {
            MoveStageIndex(new Vector2Int(-int.MaxValue, 1));
        }

        public void PrevMode()
        {
            MoveStageIndex(new Vector2Int(-int.MaxValue, -1));
        }

        public void ClearCurrentStage()
        {
            var nextStage = mStageIndex;
            nextStage.x++;
            if (IsOutOfRange(mStageOpens.Matrix, nextStage.x))
            {
                return;
            }

            if (IsOpenStage(nextStage))
            {
                return;
            }


            OpenStage(nextStage);
        }

        void OpenStage(Vector2Int stageIndex)
        {
            if (IsOutOfRange(mStageOpens.Matrix, stageIndex.y) ||
                IsOutOfRange(mStageOpens.Matrix[stageIndex.y].List, stageIndex.x))
                return;

            mStageOpens.Matrix[stageIndex.y].List[stageIndex.x] = true;
        }

        public void CloseStage(Vector2Int stageIndex)
        {
            if (IsOutOfRange(mStageOpens.Matrix, stageIndex.y) ||
                IsOutOfRange(mStageOpens.Matrix[stageIndex.y].List, stageIndex.x))
                return;

            mStageOpens.Matrix[stageIndex.y].List[stageIndex.x] = false;
        }

        bool IsOpenStage(Vector2Int stageIndex)
        {
            if (IsOutOfRange(mStageOpens.Matrix, stageIndex.y) ||
                IsOutOfRange(mStageOpens.Matrix[stageIndex.y].List, stageIndex.x))
                return false;

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