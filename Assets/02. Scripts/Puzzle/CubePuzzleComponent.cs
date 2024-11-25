using System.Linq;
using UnityEngine;
using Util;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        [SerializeField] ScriptableObject[] Puzzles;
        private CubePuzzleMadiator madiator;
        private MatrixBool _mapData;


        void OnEnable()
        {
            if(!Puzzles.Where(x => x as IPuzzleInstance != null).Any())
            {
                return;
            }

            madiator = new CubePuzzleMadiator(_mapData);
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
