using PlatformGame.Util;
using UnityEngine;

namespace PlatformGame.Contents.Puzzle
{
    public class PuzzlePiece : MonoBehaviour
    {
        public bool IsClear { get; protected set; }
        public static void EnablePieceInArea(Bounds area)
        {
            area.center = area.center + (Vector3.up * (area.extents.y / 2f + 0.5f));
            var extents = area.extents;
            extents.y = 1;
            area.extents = extents;
            foreach (var transform in RaycastUtil.FindObjects(area))
            {
                if (transform.TryGetComponent<PuzzlePiece>(out var piece))
                {
                    if (piece.IsClear)
                    {
                        return;
                    }
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

