using Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour, IInstance, IPuzzleInstance
{
    public DataReader DataReader { get; private set; } = new FlowerReader();
    private Transform _cube;
    private int _flowerCount;
    public void InstreamData(byte[] data)
    {
        if (data.Equals(FlowerReader.FLOWER_CREATE.Equals(data)))
        {
            _flowerCount++;
            _cube.position += Vector3.down;
        }
    }

    public void SetMediator(IMediatorInstance mediator)
    {
        throw new System.NotImplementedException();
    }

    public void Init(CubePuzzleDataReader puzzleData)
    {
        _cube = puzzleData.BaseTransform;
    }
}
