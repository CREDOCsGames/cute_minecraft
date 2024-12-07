#if UNITY_EDITOR
using UnityEngine;
using Util;

namespace Flow
{
    [CreateAssetMenu(menuName = "Custom/Data/StageOpensGenerator")]
    public class StageOpensGenerator : UniqueScriptableObject<StageOpensGenerator>
    {
        [SerializeField] private MatrixBool _stageOpens;
        [SerializeField] private MatrixString _stages;

        [ContextMenu("GenerateData")]
        public void GenerateData()
        {
            Debug.Assert(_stageOpens != null);
            Debug.Assert(_stages != null);

            _stageOpens.Matrix.Clear();
            for (var i = 0; i < _stages.Matrix.Count; i++)
            {
                if (_stages.Matrix[i].List.Count == 0)
                {
                    continue;
                }

                _stageOpens.Matrix.Add(new CustomList<bool>());
                for (var j = 0; j < _stages.Matrix[j].List.Count; j++)
                {
                    _stageOpens.Matrix[i].List.Add(false);
                }
            }
        }
    }
}
#endif