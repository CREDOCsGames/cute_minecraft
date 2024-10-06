using PlatformGame.Character.Combat;
using PlatformGame.Manager;
using PlatformGame.Util;
using UnityEngine;

namespace PlatformGame.Contents.Puzzle
{
    [CreateAssetMenu(menuName = "Custom/Util/AreaScriptable")]
    public class PuzzleAreaHandler : UniqueScriptableObject<PuzzleAreaHandler>
    {
        public static void InvokeMonement(ActionData action)
        {
            var pos = GameManager.PuzzleArea.Range.center;
            var sector = AreaManager.GetAreaNum(pos);
            if (!AreaManager.TryGetArea(sector, out var area))
            {
                return;

            }
            var character = area.GetComponent<Character.CharacterComponent>();
            if (character == null || !(character.State is Character.CharacterState.Idle))
            {
                return;
            }

            character.DoAction(action);
            PuzzlePieceComponent.DisablePieceInArea(area.Range);
            CoroutineRunner.InvokeDelayAction(() => PuzzlePieceComponent.EnablePieceInArea(area.Range), 1.1f);
        }
    }

}
