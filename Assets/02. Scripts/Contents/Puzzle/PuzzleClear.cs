using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents.Puzzle
{
    public class PuzzleClear : MonoBehaviour
    {
        static List<PuzzleClear> mInstances = new();
        public static PuzzleClear Instance
        {
            get
            {
                var criterion = PlayerCharacterManager.Instance?.ControlledCharacter?.transform;
                var first = mInstances.First();
                Debug.Assert(first != null, $"Not found PuzzleClear in Scene.");
                criterion = criterion == null ? first.transform : criterion;
                var minDistance = mInstances.OrderBy(x => Vector3.Distance(x.transform.position, PlayerCharacterManager.Instance.ControlledCharacter.transform.position)).First();
                return minDistance == null ? first : minDistance;
            }
        }
        [SerializeField] UnityEvent mClearEvent;

        void Awake()
        {
            mInstances.Add(this);
        }
        private void OnDestroy()
        {
            mInstances.Remove(this);
        }

        public void InvokeClearEvent()
        {
            mClearEvent.Invoke();
        }
    }

}
