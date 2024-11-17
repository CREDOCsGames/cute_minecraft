using System.Linq;
using UnityEngine;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        [SerializeField] private ScriptableObject[] _puzzles;
        private CubePuzzleMediator _mediator;


        private void OnEnable()
        {
            if (!_puzzles.Where(x => x as IPuzzleInstance != null).Any())
            {
                return;
            }

            var width = _puzzles.Where(x => x as IPuzzleInstance != null).Max(x => (x as IPuzzleInstance).Width);
            _mediator = new CubePuzzleMediator(width);
            foreach (var puzzle in _puzzles)
            {
                if (puzzle is IPuzzleInstance && puzzle is ScriptableObject obj)
                {
                    var instance = GameObject.Instantiate(obj) as IPuzzleInstance;
                    _mediator.AddPuzzle(instance);
                }
                else
                {
                    Debug.Assert(false, $"{puzzle.name} is not IPuzzleInstance. type {puzzle.GetType()}");
                }
            }
            _mediator.Init();
        }

        private void OnDisable()
        {
            _mediator = null;
        }
    }

}
