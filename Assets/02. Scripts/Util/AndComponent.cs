using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util
{
    public class ANDComponent : Condition
    {
        [SerializeField] private List<Condition> _conditions;

        public override bool IsTrue()
        {
            if (_conditions.Count != 0)
            {
                return _conditions.All(x => x.IsTrue());
            }

            return true;
        }

        public override void SetFalse()
        {
            foreach (var condition in _conditions)
            {
                condition.SetFalse();
            }
        }
    }
}