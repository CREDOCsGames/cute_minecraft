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
            ? _stages.Matrix[StageIndex.y].List[StageIndex.x]
            : TitleScene;

        [SerializeField] private MatrixBool _stageOpens;
        [SerializeField] private MatrixString _stages;
        [SerializeField] private Vector2Int _stageIndex;
        [SerializeField] private string _titleScene;

        public event Action OnChangeEvent;

        public string TitleScene
        {
            get
            {
                Debug.Assert(!string.IsNullOrEmpty(_titleScene));
                return _titleScene;
            }
        }

        public Vector2Int StageIndex
        {
            get => _stageIndex;
            private set
            {
                value.y = Mathf.Clamp(value.y, 0, _stages.Matrix.Count - 1);
                value.x = Mathf.Clamp(value.x, 0, _stages.Matrix[value.y].List.Count - 1);

                if (!IsOpenStage(value) || _stageIndex.Equals(value))
                {
                    return;
                }

                _stageIndex = value;
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
            var nextStage = _stageIndex;
            nextStage.x++;
            if (IsOutOfRange(_stageOpens.Matrix, nextStage.x))
            {
                return;
            }

            if (IsOpenStage(nextStage))
            {
                return;
            }


            OpenStage(nextStage);
        }

        private void OpenStage(Vector2Int stageIndex)
        {
            if (IsOutOfRange(_stageOpens.Matrix, stageIndex.y) ||
                IsOutOfRange(_stageOpens.Matrix[stageIndex.y].List, stageIndex.x))
                return;

            _stageOpens.Matrix[stageIndex.y].List[stageIndex.x] = true;
        }

        public void CloseStage(Vector2Int stageIndex)
        {
            if (IsOutOfRange(_stageOpens.Matrix, stageIndex.y) ||
                IsOutOfRange(_stageOpens.Matrix[stageIndex.y].List, stageIndex.x))
                return;

            _stageOpens.Matrix[stageIndex.y].List[stageIndex.x] = false;
        }

        private bool IsOpenStage(Vector2Int stageIndex)
        {
            if (IsOutOfRange(_stageOpens.Matrix, stageIndex.y) ||
                IsOutOfRange(_stageOpens.Matrix[stageIndex.y].List, stageIndex.x))
                return false;

            return _stageOpens.Matrix[stageIndex.y].List[stageIndex.x];
        }

        private bool MoveStageIndex(Vector2Int stageIndex)
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