using PlatformGame.Character.Combat;
using PlatformGame.Manager;
using PlatformGame.Util;
using UnityEngine;

namespace PlatformGame.Contents.Puzzle
{
    [CreateAssetMenu(menuName = "Custom/Util/AreaScriptable")]
    public class AreaScriptable : UniqueScriptableObject<AreaScriptable>
    {
        public static void InvokeMonement(ActionData action)
        {
            var pos = GameManager.PuzzleArea.Range.center;
            var sector = AreaManager.GetAreaNum(pos);
            if (!AreaManager.TryGetArea(sector, out var area))
            {
                return;

            }
            var character = area.GetComponent<Character.Character>();
            if (character == null || !(character.State is Character.CharacterState.Idle))
            {
                return;
            }

            character.DoAction(action);
            PuzzlePiece.DisablePieceInArea(area.Range);
            CoroutineRunner.InvokeDelayAction(() => PuzzlePiece.EnablePieceInArea(area.Range), 1.1f);
        }
    }

}
