using UnityEngine;

namespace Puzzle
{
    public class PuzzleAttackBoxComponent : AttackBoxComponent
    {
        [SerializeField][Range(0, 255)] byte _type;
        public byte Type => _type;
    }

}
