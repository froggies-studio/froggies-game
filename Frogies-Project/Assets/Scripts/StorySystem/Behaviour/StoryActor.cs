using System;
using StorySystem.Data;
using TMPro;
using UnityEngine;

namespace StorySystem.Behaviour
{
    public class StoryActor : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        public void Act(StoryLine line, Action finishCallback)
        {
            text.text = line.Line;
            finishCallback?.Invoke();
        }

        public void ForceStopActing()
        {
            
        }
    }
}