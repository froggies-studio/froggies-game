using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WaveSystem
{
    public class WaveBarController : MonoBehaviour
    {
        [SerializeField] private GameObject _waveBar;
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _currentWaveProgressText;
        private WaveController _waveController;
        
        public void Setup(WaveController waveController)
        {
            _waveController = waveController;
            _waveController.OnWaveStarted += Setup;
            _waveController.OnWaveCleared += Hide;
            _waveController.OnEnemyKilled += UpdateWaveCounter;
        }

        private void Setup()
        {
            _waveBar.SetActive(true);
            _slider.maxValue=_waveController.GetAmountOfEnemies();
            _slider.value = _waveController.GetAmountOfEnemies();
            _currentWaveProgressText.text = _waveController.GetAmountOfEnemies().ToString();
        }

        private void Hide()
        {
            _waveBar.SetActive(false);
        }

        private void UpdateWaveCounter()
        {
            _slider.value = _waveController.GetAmountOfEnemies();
            _currentWaveProgressText.text = _waveController.GetAmountOfEnemies().ToString();
            
        }
    }
}
