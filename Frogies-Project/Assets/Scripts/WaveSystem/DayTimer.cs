using System;
using TMPro;
using UnityEngine;

namespace WaveSystem
{
    public class DayTimer : MonoBehaviour
    {
        private float _dayDuration = 0.5f * 60f;
        private float _time;
        public Action OnDayEnd;
        public bool _isDay;
        [SerializeField] private TMP_Text timeText;

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
            }
            else if (_isDay)
            {
                ClearTimer();
                if (OnDayEnd != null) OnDayEnd.Invoke();
            }
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