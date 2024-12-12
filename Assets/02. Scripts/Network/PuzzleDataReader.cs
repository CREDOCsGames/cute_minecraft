using System.Collections.Generic;

namespace NW
{
    public class PuzzleDataReader
    {
        private bool _bStart;
        private Face _currentReadWindow;
        private CubePuzzleData _cubePuzzleData;

        public PuzzleDataReader(CubePuzzleData cubePuzzleData)
        {
            _cubePuzzleData = cubePuzzleData;
        }
        public void MoveReadWindow(Face nextReadWindow)
        {
            DestroyUnusedResources(_currentReadWindow, nextReadWindow);
            CreateNewlyUsedResources(_currentReadWindow, nextReadWindow);
            _bStart = true;
        }
        public void ReadAllCores(out List<ICore> cores)
        {
            PuzzleDataReaderUtil.ReadAllCore(_cubePuzzleData, _currentReadWindow, out cores);
        }
        public void ReadAllInstances(out List<IInstance> instances)
        {
            PuzzleDataReaderUtil.ReadAllInstance(_cubePuzzleData, _currentReadWindow, out instances);
        }

        private void DestroyUnusedResources(Face before, Face affter)
        {
            if (!_bStart)
            {
                return;
            }

            PuzzleDataReaderUtil.ReadDifferenceOfSets(_cubePuzzleData, before, affter, out List<ICore> destroyTargetsFromCore);
            foreach (var item in destroyTargetsFromCore)
            {
                //(item as IDestroyable)?.Destroy();
            }

            PuzzleDataReaderUtil.ReadDifferenceOfSets(_cubePuzzleData, before, affter, out List<ICore> destroyTargetsFromInstance);
            foreach (var item in destroyTargetsFromInstance)
            {
                //(item as IDestroyable)?.Destroy();
            }
        }
        private void CreateNewlyUsedResources(Face before, Face affter)
        {
            List<ICore> instantiateTargetsFromCore;
            List<IInstance> instantiateTargetsFromInstance;

            if (_bStart)
            {
                PuzzleDataReaderUtil.ReadDifferenceOfSets(_cubePuzzleData, before, affter, out instantiateTargetsFromCore);
                PuzzleDataReaderUtil.ReadDifferenceOfSets(_cubePuzzleData, before, affter, out instantiateTargetsFromInstance);
            }
            else
            {
                PuzzleDataReaderUtil.ReadAllCore(_cubePuzzleData, affter, out instantiateTargetsFromCore);
                PuzzleDataReaderUtil.ReadAllInstance(_cubePuzzleData, affter, out instantiateTargetsFromInstance);
            }

            foreach (var item in instantiateTargetsFromCore)
            {
                //(item as IDestroyable)?.Instantiate();
            }
            foreach (var item in instantiateTargetsFromInstance)
            {
                //(item as IDestroyable)?.Instantiate();
            }
        }

    }
}