using PlatformGame.Util;
using UnityEngine;

namespace PlatformGame.Contents.Puzzle
{
    public class PuzzlePiece : MonoBehaviour
    {
        public static void EnablePieceInArea(Bounds area)
        {
            area.center = area.center + Vector3.up * area.size.y/2;
            foreach (var transform in RaycastUtil.FindObjects(area))
            {
                if (transform.TryGetComponent<PuzzlePiece>(out var piece))
                {
                    piece.enabled = true;
                }
            }
        }

        public static void DisablePieceInArea(Bounds area)
        {
            foreach (var transform in RaycastUtil.FindObjects(area))
            {
                if (transform.TryGetComponent<PuzzlePiece>(out var piece))
                {
                    piece.enabled = false;
                }
            }
        }
    }

}

