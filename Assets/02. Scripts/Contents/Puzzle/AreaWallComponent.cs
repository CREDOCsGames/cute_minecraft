using PlatformGame.Mamager;
using PlatformGame.Manager;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Contents.Puzzle
{
    [RequireComponent(typeof(AreaComponent))]
    public class AreaWallComponent : MonoBehaviour
    {
        enum Type { Wall, JumpWall }
        [SerializeField] Side Side;
        AreaWall mWall;
        AreaComponent mArea;
        [SerializeField] Type WallType;

        void Start()
        {
            mArea = GetComponent<AreaComponent>();

            var bounds = mArea.HalfRange;
            bounds.center += Vector3.up * 5f;
            mWall = new AreaWall(Side, bounds, $"Objects/{WallType}");

            mArea.OnEnterEvent.AddListener(b => MakeWall($"Objects/{WallType}"));
            mArea.OnExitEvent.AddListener(b => mWall.Destroy());
            mArea.OnClearEvent.AddListener(b => { MakeWall($"Objects/{Type.Wall}"); Invoke(nameof(MakeWay), 0.1f); });
        }

        void MakeWall(string wall)
        {
            mWall.Destroy();
            mWall.SetWall(wall);
            mWall.Create();
        }

        void MakeWay()
        {
            if (!mWall.Objects.Any())
            {
                return;
            }

            foreach (var obj in mWall.Objects)
            {
                if (!Physics.CheckBox(obj.transform.position, obj.transform.lossyScale / 2f, Quaternion.identity, LayerMask.GetMask("Bridge")))
                {
                    continue;
                }

                var colliders = obj.GetComponents<Collider>();
                colliders[0].enabled = false;
                colliders[1].enabled = true;
                colliders[2].enabled = true;
            }
        }

    }
}
