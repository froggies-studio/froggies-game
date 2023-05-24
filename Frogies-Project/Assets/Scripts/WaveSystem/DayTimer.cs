using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace WaveSystem
{
    public class DayTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private DayNightVisuals dayNightVisuals;
        
        public Action OnDayEnd;
        public bool _isDay;
        
        private float _dayDuration = 0.5f * 60;
        private float _time;
        
        private void Start()
        {
            ResetTimer();
        }

        public void ResetTimer()
        {
            gameObject.SetActive(true);
            _time = _dayDuration;
            _isDay = true;
        }

        public void UpdateTimer()
        {
            if (_time > 0)
            {
                _time -= Time.deltaTime;
                UpdateTimerDisplay(_time);
                UpdateVisuals(_time);
            }
            else if (_isDay)
            {
                ClearTimer();
                dayNightVisuals.TransitionToNight();
                if (OnDayEnd != null) OnDayEnd.Invoke();
            }
        }

        private void UpdateVisuals(float time)
        {
            float dayProgress = 1 - time / _dayDuration;
            dayNightVisuals.UpdateVisuals(dayProgress, _isDay);
        }

        public void ClearTimer()
        {
            gameObject.SetActive(false);
            _isDay = false;
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