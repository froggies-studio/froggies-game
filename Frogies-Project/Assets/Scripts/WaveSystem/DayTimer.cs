using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace WaveSystem
{
    public class DayTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private DayNightVisuals dayNightVisuals;
        [SerializeField] private bool isFixed;
        [SerializeField] private float dayDuration = 0.5f * 60;
        
        public Action OnDayEnd;
        public bool isDay;
        
        private float _time;
        
        private void Start()
        {
            ResetTimer();
        }

        public void ResetTimer()
        {
            gameObject.SetActive(true);
            _time = dayDuration;
            isDay = true;
        }

        public void UpdateTimer()
        {
            if (isFixed)
                return;
            
            if (_time > 0)
            {
                _time -= Time.deltaTime;
                UpdateTimerDisplay(_time);
                UpdateVisuals(_time);
            }
            else if (isDay)
            {
                ClearTimer();
                dayNightVisuals.TransitionToNight();
                if (OnDayEnd != null) OnDayEnd.Invoke();
            }
        }

        private void UpdateVisuals(float time)
        {
            float dayProgress = 1 - time / dayDuration;
            dayNightVisuals.UpdateVisuals(dayProgress, isDay);
        }

        public void ClearTimer()
        {
            gameObject.SetActive(false);
            isDay = false;
        }

        private void UpdateTimerDisplay(float time)
        {
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);
            
            string currentTime = String.Format("{00:00}:{1:00}", minutes, seconds);
            timeText.text = currentTime;
        }
    }
}