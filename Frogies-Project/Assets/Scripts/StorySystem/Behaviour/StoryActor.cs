using System;
using DG.Tweening;
using JetBrains.Annotations;
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
        
        [CanBeNull] private Tween _textTween;
        
        public void Act(StoryLine line, Action finishCallback)
        {
            textPanel.gameObject.SetActive(true);
            DOTweenTMPAnimator tweenTMPAnimator = new DOTweenTMPAnimator(text);
            _textTween = tweenTMPAnimator.DOText(line.Line, .65f).OnComplete(() =>
            {
                finishCallback?.Invoke();
            }).Play().SetUpdate(true);
        }

        public void ForceStopActing()
        {
            //_textTween.Kill(true);
        }
        
        public void Deactivate()
        {
            text.text = "";
            textPanel.gameObject.SetActive(false);
        }
    }
}