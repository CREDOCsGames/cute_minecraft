using System.Linq;

namespace NW
{
    public enum Face : byte { Top, Left, Front, Right, Back, Bottom }

    public interface IMediator
    {

    }

    public class MediatorCenter : IMediatorCore, IMediatorInstance
    {
        private readonly PuzzleDataReader _dataReader;

        public MediatorCenter(CubePuzzleData puzzleData)
        {
            _dataReader = new PuzzleDataReader(puzzleData);
        }
        public void StartPuzzle(Face nextFace)
        {
            CleanUnusedResources(nextFace);
            SettingNewlyUsedResources(nextFace);
        }
        public void InstreamDataCore<T>(byte[] data) where T : DataReader
        {
            _dataReader.ReadAllInstances(out var cores);
            var observers = cores.Where(x => x.DataReader is T);
            foreach (var instance in observers)
            {
                instance.InstreamData(data);
            }
        }
        public void InstreamDataInstance<T>(byte[] data) where T : DataReader
        {
            _dataReader.ReadAllInstances(out var instances);
            var observers = instances.Where(x => x.DataReader is T);
            foreach (var instance in observers)
            {
                instance.InstreamData(data);
            }
        }
        private void CleanUnusedResources(Face nextFace)
        {
            _dataReader.ReadAllCores(out var usedCores);
            _dataReader.ReadAllInstances(out var usedInstances);
            EventBinder.UnbindEvent(usedCores, this);
            EventBinder.UnbindEvent(usedInstances, this);
        }
        private void SettingNewlyUsedResources(Face nextFace)
        {
            _dataReader.MoveReadWindow(nextFace);
            _dataReader.ReadAllCores(out var cores);
            _dataReader.ReadAllInstances(out var instances);
            EventBinder.BindEvent(cores, this);
            EventBinder.BindEvent(instances, this);
        }

    }
}
