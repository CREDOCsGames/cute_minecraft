using Character;
using Flow;
using UnityEngine;

namespace Puzzle
{
    public class AreaManagerComponent : MonoBehaviour
    {
        [SerializeField] private int _areaRange = 40;
        [SerializeField, Range(5, 1000)] private float _bridgeLimitDistance = 10f;
        private Vector3Int? _beforeAreaNum;

        private void EnterArea(Vector3Int sectorNum)
        {
            if (!AreaManager.TryGetArea(sectorNum, out var enterArea))
            {
                return;
            }

            _beforeAreaNum = sectorNum;
            GameManager.PuzzleArea.Range = enterArea.Range;
            enterArea.enabled = true;
            enterArea.OnEnter();
        }

        private void ExitArea(Vector3Int sectorNum)
        {
            if (!AreaManager.TryGetArea(sectorNum, out var beforeArea))
            {
                return;
            }

            beforeArea.OnExit();
            beforeArea.enabled = false;
            GameManager.PuzzleArea.Range = Area.zero;
        }

        private void Start()
        {
            GameManager.StageArea.OnEnter();

            AreaManager.AreaRange = _areaRange;
            AreaManager.BridgeLimitDistance = _bridgeLimitDistance;
            AreaManager.NumberingAreas();
        }

        private void OnDestroy()
        {
            GameManager.StageArea.OnExit();
            GameManager.PuzzleArea.Range = Area.zero;
        }

        // TODO: Separate puzzle area verification logic.
        private void Update()
        {
            var character = PlayerCharacterManager.Instance.ControlledCharacter;
            if (character == null)
            {
                return;
            }

            var pos = character.transform.position;
            pos.y -= 2.5f;

            CheckExit(pos);
            CheckEnter(pos);
        }

        private void CheckExit(Vector3 pos)
        {
            if (_beforeAreaNum == null ||
                !AreaManager.TryGetArea(_beforeAreaNum.Value, out var beforeArea))
            {
                return;
            }

            if (GameManager.PuzzleArea.Range == Area.zero ||
                beforeArea.HalfRange.Contains(pos))
            {
                return;
            }

            ExitArea(_beforeAreaNum.Value);
        }

        private void CheckEnter(Vector3 pos)
        {
            if (GameManager.PuzzleArea.Range != Area.zero)
            {
                return;
            }

            var areaNum = AreaManager.GetAreaNum(pos);
            if (!AreaManager.TryGetArea(areaNum, out var area))
            {
                return;
            }

            var enterRange = area.HalfRange;
            enterRange.extents *= 0.8f;
            enterRange.extents = new Vector3(enterRange.extents.x, area.HalfRange.extents.y, enterRange.extents.z);

            if (!enterRange.Contains(pos))
            {
                return;
            }

            EnterArea(areaNum);
        }
    }
}