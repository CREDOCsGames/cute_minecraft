using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Util
{
    public class AND : Condition
    {
        [SerializeField] List<Condition> mConditions;

        public override bool IsTrue()
        {
            if (mConditions.Count == 0)
            {
                Debug.Log("Condition count : 0");
                return true;
            }
            return !mConditions.Any(x => !x.IsTrue());
        }

        public override void SetFalse()
        {
            foreach(var condition in mConditions)
            {
                condition.SetFalse();
            }
        }
    }

}
