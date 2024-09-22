using PlatformGame;
using PlatformGame.Manager;
using UnityEngine;

public class AreaManagerComponent : MonoBehaviour
{
    [SerializeField] int AreaRange = 40;
    [SerializeField, Range(5, 1000)] float BridgeLimitDistance = 10f;
    Vector3Int? mBeforeAreaNum;

    void EnterArea(Vector3Int sectorNum)
    {
        if (!AreaManager.TryGetArea(sectorNum, out var enterArea))
        {
            return;
        }
        mBeforeAreaNum = sectorNum;
        GameManager.PuzzleArea.Range = enterArea.Range;
        enterArea.enabled = true;
        enterArea.OnEnter();
    }

    void ExitArea(Vector3Int sectorNum)
    {
        if (!AreaManager.TryGetArea(sectorNum, out var beforeArea))
        {
            return;
        }
        beforeArea.OnExit();
        beforeArea.enabled = false;
        GameManager.PuzzleArea.Range = Area.zero;
    }

    void Start()
    {
        GameManager.StageArea.OnEnter();

        AreaManager.AreaRange = AreaRange;
        AreaManager.BridgeLimitDistance = BridgeLimitDistance;
        AreaManager.NumberingAreas();
    }

    void OnDestroy()
    {
        GameManager.StageArea.OnExit();
        GameManager.PuzzleArea.Range = Area.zero;
    }

    // TODO: Separate puzzle area verification logic.
    void Update()
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

    void CheckExit(Vector3 pos)
    {
        if (mBeforeAreaNum == null ||
            !AreaManager.TryGetArea(mBeforeAreaNum.Value, out var beforeArea))
        {
            return;
        }

        if (GameManager.PuzzleArea.Range == Area.zero ||
            beforeArea.HalfRange.Contains(pos))
        {
            return;
        }

        ExitArea(mBeforeAreaNum.Value);
    }

    void CheckEnter(Vector3 pos)
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
