using System;
using Core;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class BackGroundTransition : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _backgrounds;
    [SerializeField] private float _transitionTime;
    
    [SerializeField] private float _yFactorInTransition;
    [SerializeField] private float _yFactorOutTransition;
    
    private Transform _playerTransform;
    [CanBeNull] private Sequence _sequence;
    private bool _showBg = true;

    private void Start()
    {
        _playerTransform = GlobalSceneManager.Instance.PlayerTransform;
    }

    private void Update()
    {
        switch (_showBg)
        {
            case true when _playerTransform.position.y <= _yFactorInTransition:
            {
                _showBg = false;
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                foreach (var background in _backgrounds)
                {
                    _sequence.Append(background.DOFade(0, _transitionTime));
                }

                break;
            }
            case false when _playerTransform.position.y >= _yFactorOutTransition:
            {
                _showBg = true;
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                foreach (var background in _backgrounds)
                {
                    _sequence.Append(background.DOFade(1, _transitionTime));
                }

                break;
            }
        }
    }
}
