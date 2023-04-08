

using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Items.Behaviour
{
    public class SceneItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private Canvas _canvas;

        [Header("DropAnimation")]
        [SerializeField] private float _dropRadius;
        [SerializeField] private float _dropRotation;
        [SerializeField] private float _dropAnimDuration;

        public event Action<SceneItem> ItemClicked;
        public bool TextEnabled {
            set
            {
                if (_textEnabled == value) return;
                
                _textEnabled = value;
                _canvas.enabled = value;
            }
        }
        private bool _textEnabled = true;

        private Sequence _sequence;
        private void Awake()
        {
            _button.onClick.AddListener(() => ItemClicked?.Invoke(this));
        }

        private void OnMouseDown()
        {
            ItemClicked?.Invoke(this);
        }

        public void SetItem(Sprite sprite, string itemName, Color textColor)
        {
            _spriteRenderer.sprite = sprite;
            _text.text = itemName;
            _text.color = textColor;
            TextEnabled = false;
        }

        public void PlayDrop(Vector2 position)
        {
            transform.position = position;
            Vector2 movePosition = position + new Vector2(Random.Range(-_dropRadius, _dropRadius), 0);
            _sequence = DOTween.Sequence();
            _sequence.Join(transform.DOMove(movePosition, _dropAnimDuration));
            _sequence.Append(transform.DORotate(new Vector3(0,0,Random.Range(-_dropRotation, _dropRotation)), _dropAnimDuration));
            _sequence.OnComplete(()=>TextEnabled = true);
        }
    }
}