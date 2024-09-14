using PlatformGame.Mamager;
using PlatformGame.Manager;
using UnityEngine;

namespace PlatformGame.Contents.Puzzle
{
    [RequireComponent(typeof(AreaComponent))]
    public class AreaWallComponent : MonoBehaviour
    {
        [SerializeField] Side Side;
        AreaWall mWall;
        AreaComponent mArea;

        void Awake()
        {
            mArea = GetComponent<AreaComponent>();
            mWall = new AreaWall(Side, mArea.Range);
            mArea.OnEnterEvent.AddListener((b) => mWall.Create());
            mArea.OnExitEvent.AddListener(b => mWall.Destroy());
        }
    }

}
