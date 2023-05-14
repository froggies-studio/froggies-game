using System;
using DG.Tweening;
using StorySystem.Data;
using TMPro;
using UnityEngine;
using Utility;

namespace StorySystem.Behaviour
{
    public class StoryActor : MonoBehaviour
    {
        [SerializeField] protected Transform textPanel;
        [SerializeField] protected TMP_Text text;
        public void Act(StoryLine line, Action finishCallback)
        {
            textPanel.gameObject.SetActive(true);
            DOTweenTMPAnimator tweenTMPAnimator = new DOTweenTMPAnimator(text);
            tweenTMPAnimator.DOText(line.Line, .65f).OnComplete(() =>
            {
                finishCallback?.Invoke();
            });
            finishCallback?.Invoke();
        }

        public void ForceStopActing()
        {
            
        }
        
        public void Deactivate()
        {
            text.text = "";
            textPanel.gameObject.SetActive(false);
        }
    }
}