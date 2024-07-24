using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents.Compartment
{
    public class Puzzle : MonoBehaviour
    {
        public UnityEvent OnCompleteEvent;
        public List<Compartments> Compartments;
        bool mbComplete;

        void Awake()
        {
            Debug.Assert(Compartments != null);
        }

        public void OnColorize()
        {
            mbComplete = Compartments.All(x => x.SymbolColor == x.PaintedColor);
            if (mbComplete)
            {
                OnCompleteEvent.Invoke();
            }
        }
    }
}
