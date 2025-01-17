using Battle;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle
{
    public class CubePuzzleDataReader
    {
        public event Action<Face> OnRotatedStage;
        public Face ReadWindow { get; private set; }
        public byte Width => _cubeMapReader.Width;
        public Throw Throw => _cubePuzzleData.Trow;
        public readonly Transform BaseTransform;
        public readonly Vector3 BaseTransformSize;
        public Vector3 BaseTransformPlacePivot =>
            BaseTransform.position
            + Vector3.up * BaseTransformSize.y / 2f
            - (Vector3.right + Vector3.forward) * (Width / 2);
        public readonly List<ICore> GlobalCoreObservers = new();
        public readonly List<IInstance> GlobalInstanceObservers = new();
        private readonly CubeMapReader _cubeMapReader;
        private readonly CubePuzzleData _cubePuzzleData;

        public CubePuzzleDataReader(CubePuzzleData puzzleData, UnityEvent<Face> onRotatedStage)
        {
            onRotatedStage.AddListener((face)=>OnRotatedStage?.Invoke(face));
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
        public void MoveReadWindow(Face nextReadWindow)
        {
            ReadWindow = nextReadWindow;
        }
        public void ReadAllCores(out List<ICore> cores)
        {
            cores = new List<ICore>();
            cores.AddRange(_cubePuzzleData.GlobalCores);
            cores.AddRange(_cubePuzzleData.Faces[(int)ReadWindow].Cores);
            cores.AddRange(GlobalCoreObservers);
        }
        public void ReadAllCores(Face face, out List<ICore> cores)
        {
            cores = new List<ICore>();
            cores.AddRange(_cubePuzzleData.GlobalCores);
            cores.AddRange(_cubePuzzleData.Faces[(int)face].Cores);
            cores.AddRange(GlobalCoreObservers);
        }
        public void ReadAllInstances(out List<IInstance> instances)
        {
            instances = new List<IInstance>();
            instances.AddRange(_cubePuzzleData.GlobalInstances);
            instances.AddRange(_cubePuzzleData.Faces[(int)ReadWindow].Instances);
            instances.AddRange(GlobalInstanceObservers);
        }
        public void ReadAllInstances(Face face, out List<IInstance> instances)
        {
            instances = new List<IInstance>();
            instances.AddRange(_cubePuzzleData.GlobalInstances);
            instances.AddRange(_cubePuzzleData.Faces[(int)face].Instances);
            instances.AddRange(GlobalInstanceObservers);
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
            float offsetY = (BaseTransformSize.x / 2) - 0.5f;
            var x = index[0];
            var y = index[1];
            var face = index[2];
            switch (face)
            {
                case (byte)Face.front:
                    position = new Vector3(x - offset, y - offset, 0) + Vector3.forward * ((offsetY) + 0.5f);
                    rotation = Quaternion.Euler(90, 0, 0);
                    break;
                case (byte)Face.back:
                    position = Quaternion.Euler(0, 0, 180) * new Vector3(x - offset, y - offset, 0) + Vector3.back * ((offsetY) + 0.5f);
                    rotation = Quaternion.Euler(-90, 0, 0);
                    break;
                case (byte)Face.top:
                    position = Quaternion.Euler(270, 0, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.up * ((offsetY) + 0.5f);
                    rotation = Quaternion.identity;
                    break;
                case (byte)Face.bottom:
                    position = Quaternion.Euler(90, 0, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.down * ((offsetY) + 0.5f);
                    rotation = Quaternion.Euler(180, 0, 0);
                    break;
                case (byte)Face.left:
                    position = Quaternion.Euler(0, 90, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.left * ((offsetY) + 0.5f);
                    rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case (byte)Face.right:
                    position = Quaternion.Euler(0, 270, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.right * ((offsetY) + 0.5f);
                    rotation = Quaternion.Euler(0, 0, -90);
                    break;
                default:
                    position = Vector3.zero;
                    rotation = Quaternion.identity;
                    break;
            };
        }
    }
}