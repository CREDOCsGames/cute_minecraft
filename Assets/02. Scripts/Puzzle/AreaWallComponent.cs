using Flow;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    [RequireComponent(typeof(AreaComponent))]
    public class AreaWallComponent : MonoBehaviour
    {
        private enum Type
        {
            Wall,
            JumpWall
        }

        [SerializeField] private Side _side;
        private AreaWall _wall;
        private AreaComponent _area;
        [SerializeField] private Type _wallType;

        private void Start()
        {
            _area = GetComponent<AreaComponent>();

            var bounds = _area.HalfRange;
            bounds.center += Vector3.up * 5f;
            _wall = new AreaWall(_side, bounds, $"Objects/{_wallType}");

            _area.OnEnterEvent.AddListener(b => MakeWall($"Objects/{_wallType}"));
            _area.OnExitEvent.AddListener(b => _wall.Destroy());
            _area.OnClearEvent.AddListener(b =>
            {
                MakeWall($"Objects/{Type.Wall}");
                Invoke(nameof(MakeWay), 0.1f);
            });
        }

        private void MakeWall(string wall)
        {
            _wall.Destroy();
            _wall.SetWall(wall);
            _wall.Create();
        }

        private void MakeWay()
        {
            if (!_wall.Objects.Any())
            {
                return;
            }

            foreach (var obj in _wall.Objects)
            {
                if (!Physics.CheckBox(obj.transform.position, obj.transform.lossyScale / 2f, Quaternion.identity,
                        LayerMask.GetMask("Bridge")))
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