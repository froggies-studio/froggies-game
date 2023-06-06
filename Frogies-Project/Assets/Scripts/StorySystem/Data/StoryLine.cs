using System;
using UnityEngine;

namespace StorySystem.Data
{
    [Serializable]
    public struct StoryLine
    {
        [TextArea]
        [SerializeField] public string line;

        public string Line => line;
    }
}