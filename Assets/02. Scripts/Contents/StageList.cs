using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Contents
{
    [CreateAssetMenu(fileName = "StageList", menuName = "Custom/StageList")]
    public class StageList : ScriptableObject
    {
        [SerializeField] List<string> mStageNames;
        public List<string> Names => mStageNames;
    }
}