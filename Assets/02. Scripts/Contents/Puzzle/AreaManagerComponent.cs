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
        var GM = GameManager.StageArea;

        AreaManager.AreaRange = AreaRange;
        AreaManager.BridgeLimitDistance = BridgeLimitDistance;
        AreaManager.NumberingAreas();
    }

    void OnDestroy()
    {
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
        var currentSectorNum = AreaManager.GetSectorNum(pos);
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
            if (area.HalfRange.Contains(pos))
            {
                EnterArea(mSectorNum.Value);
            }
        }



    }


}
