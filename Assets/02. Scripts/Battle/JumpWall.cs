using Controller;
using Puzzle;
using UnityEngine;

namespace Battle
{
    public class JumpWall : MonoBehaviour
    {
        private enum DIR
        {
            RIGHT, LEFT, FRONT, BACK
        }
        [SerializeField] DIR _dir;
        private static float _updateTime;
        private CubeInstance _cube;
        private void OnTriggerEnter(Collider other)
        {
            if (Time.time < _updateTime + 1.5f)
            {
                return;
            }

            if (other.transform.TryGetComponent<CharacterComponent>(out var con))
            {
                _cube = GameObject.FindAnyObjectByType<CubeInstance>();
                if (_cube != null)
                {
                    _updateTime = Time.time;

                    switch (_dir)
                    {
                        case DIR.LEFT:
                            _cube.TurnLeft();
                            break;
                        case DIR.BACK:
                            _cube.TurnBack();
                            break;
                        case DIR.RIGHT:
                            _cube.TurnRight();
                            break;
                        case DIR.FRONT:
                            _cube.TurnFront();
                            break;
                        default:
                            break;
                    }

                }

            }
        }
    }

}
