using NW;
using Puzzle;
using UnityEngine;

public class SlimeSpawnerInstance : PuzzleInstance
{
    [SerializeField] private Transform _slime;
    [SerializeField] private AnimationCurve _jumpCurve;
    private Transform _baseTransform;

    protected override void Instantiate(CubeMapReader puzzleData)
    {
        _baseTransform = puzzleData.BaseTransform;
    }
    protected override void SetDataLink(out IDataLink dataLink)
    {
        throw new System.NotImplementedException();
    }
    protected override void SetPresentation(out IPresentation presentation)
    {
        presentation = new SlimeSpawnerPresentation(_baseTransform, _slime, _jumpCurve);
    }
    protected override void SetDataReader(out NW.DataReader reader)
    {
        throw new System.NotImplementedException();
    }

}
