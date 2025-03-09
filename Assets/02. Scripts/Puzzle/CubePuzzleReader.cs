using Battle;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle
{
    public class CubePuzzleReader
    {
        public event Action<Face, Face> OnRotated;
        public Face Stage { get; private set; }
        public Face PlayingFace { get; private set; }
        public byte Width => _cubeMapReader.Width;
        public Throw Throw => _cubePuzzleData.Trow;
        public readonly Transform BaseTransform;
        public readonly Vector3 BaseTransformSize;
        public Vector3 BaseTransformPlacePivot =>
            BaseTransform.position
            + Vector3.up * BaseTransformSize.y / 2f
            - (Vector3.right + Vector3.forward) * (Width / 2);
        public readonly List<ICore> CoreObservers = new();
        public readonly List<IInstance> InstanceObservers = new();
        private readonly CubeMapReader _cubeMapReader;
        private readonly CubePuzzleData _cubePuzzleData;

        public CubePuzzleReader(CubePuzzleData puzzleData, CubePuzzleEvent puzzleEvent)
        {
            puzzleEvent.OnRotated += (pre, next) => OnRotated?.Invoke(pre, next);
            OnRotated += (p, n) => PlayingFace = n;
            _cubePuzzleData = puzzleData;
            _cubeMapReader = new(new CubeMap<byte>(puzzleData.Width, puzzleData.Elements));
            BaseTransform = puzzleData.BaseTransform;
            BaseTransformSize = puzzleData.BaseTransformSize.extents;
        }
        public bool TryGetElements(out byte[] elements)
            => _cubeMapReader.TryGetElements(out elements);
        public List<byte[]> GetIndex()
            => _cubeMapReader.GetIndex();
        public byte GetElement(byte x, byte y, byte z)
        => _cubeMapReader.GetElement(x, y, z);
        public byte GetElement(byte[] index)
        => _cubeMapReader.GetElement(index);
        public List<byte> GetFace(byte face)
            => _cubeMapReader.GetFace(face);
        public void NextLevel(Face nextReadWindow)
        {
            Stage = nextReadWindow;
        }
        public void ReadAllCores(out List<ICore> cores)
        {
            cores = new List<ICore>();
            cores.AddRange(_cubePuzzleData.GlobalCores);
            cores.AddRange(_cubePuzzleData.Faces[(int)Stage].Cores);
            cores.AddRange(CoreObservers);
        }
        public void ReadAllCores(Face face, out List<ICore> cores)
        {
            cores = new List<ICore>();
            cores.AddRange(_cubePuzzleData.GlobalCores);
            cores.AddRange(_cubePuzzleData.Faces[(int)face].Cores);
            cores.AddRange(CoreObservers);
        }
        public void ReadAllInstances(out List<IInstance> instances)
        {
            instances = new List<IInstance>();
            instances.AddRange(_cubePuzzleData.GlobalInstances);
            instances.AddRange(_cubePuzzleData.Faces[(int)Stage].Instances);
            instances.AddRange(InstanceObservers);
        }
        public void ReadAllInstances(Face face, out List<IInstance> instances)
        {
            instances = new List<IInstance>();
            instances.AddRange(_cubePuzzleData.GlobalInstances);
            instances.AddRange(_cubePuzzleData.Faces[(int)face].Instances);
            instances.AddRange(InstanceObservers);
        }
        public void ReadDifferenceOfSets(Face A, Face B, out List<IInstance> differenceOfSets)
        {
            differenceOfSets = new List<IInstance>();
            var setA = _cubePuzzleData.Faces[(byte)A].Instances;
            var setB = _cubePuzzleData.Faces[(byte)B].Instances;
            foreach (var item in setA)
            {
                if (setB.Contains(item))
                {
                    continue;
                }
                differenceOfSets.Add(item);
            }
        }
        public void ReadIntersection(Face A, Face B, out List<IInstance> intersection)
        {
            intersection = new List<IInstance>();
            var setA = _cubePuzzleData.Faces[(byte)A].Instances;
            var setB = _cubePuzzleData.Faces[(byte)B].Instances;
            foreach (var item in setB)
            {
                if (setA.Contains(item))
                {
                    continue;
                }
                intersection.Add(item);
            }
        }
        public void ReadDifferenceOfSets(Face A, Face B, out List<ICore> differenceOfSets)
        {
            differenceOfSets = new List<ICore>();
            var setA = _cubePuzzleData.Faces[(byte)A].Cores;
            var setB = _cubePuzzleData.Faces[(byte)B].Cores;
            foreach (var item in setA)
            {
                if (setB.Contains(item))
                {
                    continue;
                }
                differenceOfSets.Add(item);
            }
        }
        public void ReadIntersection(Face A, Face B, out List<ICore> intersection)
        {
            intersection = new List<ICore>();
            var setA = _cubePuzzleData.Faces[(byte)A].Cores;
            var setB = _cubePuzzleData.Faces[(byte)B].Cores;
            foreach (var item in setB)
            {
                if (setA.Contains(item))
                {
                    continue;
                }
                intersection.Add(item);
            }
        }
        public void GetPositionAndRotation(byte[] index, out Vector3 position, out Quaternion rotation)
        {
            Debug.Assert(index.Length == 3);
            float offset = Width / 2;
            var x = index[0];
            var y = index[1];
            position = BaseTransform.position + new Vector3(x - offset, BaseTransformSize.y / 2f, -(y - offset));
            rotation = Quaternion.identity;
        }
    }
}