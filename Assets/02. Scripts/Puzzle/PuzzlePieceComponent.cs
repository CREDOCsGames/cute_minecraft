using UnityEngine;
using Util;

namespace Puzzle
{
    public class PuzzlePieceComponent : MonoBehaviour
    {
        public bool IsClear { get; protected set; }

        public static void EnablePieceInArea(Bounds area)
        {
            area.center = area.center + (Vector3.up * (area.extents.y / 2f + 0.5f));
            var extents = area.extents;
            extents.y = 1;
            area.extents = extents;
            foreach (var transform in RaycastHelper.FindObjects(area))
            {
                if (!transform.TryGetComponent<PuzzlePieceComponent>(out var piece))
                {
                    continue;
                }

                if (piece.IsClear)
                {
                    return;
                }

                piece.enabled = true;
            }
        }

        public static void DisablePieceInArea(Bounds area)
        {
            foreach (var transform in RaycastHelper.FindObjects(area))
            {
                if (transform.TryGetComponent<PuzzlePieceComponent>(out var piece))
                {
                    piece.enabled = false;
                }
            }
        }
    }
}