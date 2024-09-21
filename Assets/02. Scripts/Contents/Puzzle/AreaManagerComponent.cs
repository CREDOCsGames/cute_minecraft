using PlatformGame;
using PlatformGame.Manager;
using UnityEngine;

public class AreaManagerComponent : MonoBehaviour
{
    [SerializeField] int AreaRange = 40;
    [SerializeField, Range(5, 1000)] float BridgeLimitDistance = 10f;
    float LoadUnit;
    Vector3Int? mSectorNum;

    void EnterArea(Vector3Int sectorNum)
    {
        if (!AreaManager.TryGetArea(sectorNum, out var enterArea))
        {
            return;
        }
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
    }

    void Start()
    {
        // Memory Up
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

    void Update()
    {
        var character = PlayerCharacterManager.Instance.ControlledCharacter;
        if (character == null)
        {
            return;
        }

        var pos = character.transform.position;
        pos.y -= 2.5f;
        var currentSectorNum = AreaManager.GetAreaNum(pos);
        if (!currentSectorNum.Equals(mSectorNum))
        {
            mSectorNum = currentSectorNum;
        }

        if (!AreaManager.TryGetArea(mSectorNum.Value, out var area))
        {
            return;
        }
        if (GameManager.PuzzleArea.Range != Area.zero)
        {
            if (!area.HalfRange.Contains(pos))
            {
                ExitArea(mSectorNum.Value);
            }
        }
        else
        {
            var enterRange = area.HalfRange;
            enterRange.extents *= 0.8f;
            enterRange.extents = new Vector3(enterRange.extents.x, area.HalfRange.extents.y, enterRange.extents.z);
            if (enterRange.Contains(pos))
            {
                EnterArea(mSectorNum.Value);
            }
        }



    }


}
