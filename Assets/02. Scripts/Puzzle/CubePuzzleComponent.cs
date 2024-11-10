using System.Linq;
using UnityEngine;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        [SerializeField] ScriptableObject[] Puzzles;
        CubePuzzleMadiator madiator;


        void OnEnable()
        {
            if(!Puzzles.Where(x => x as IPuzzleInstance != null).Any())
            {
                return;
            }

            var width = Puzzles.Where(x => x as IPuzzleInstance != null).Max(x => (x as IPuzzleInstance).Width);
            madiator = new CubePuzzleMadiator(width);
            foreach (var puzzle in Puzzles)
            {
                if (puzzle is IPuzzleInstance && puzzle is ScriptableObject obj)
                {
                    var instance = GameObject.Instantiate(obj) as IPuzzleInstance;
                    madiator.AddPuzzle(instance);
                }
                else
                {
                    Debug.Assert(false, $"{puzzle.name} is not IPuzzleInstance. type {puzzle.GetType()}");
                }
            }
            madiator.Init();
        }

        private void OnDisable()
        {
            madiator = null;
        }
    }

}
