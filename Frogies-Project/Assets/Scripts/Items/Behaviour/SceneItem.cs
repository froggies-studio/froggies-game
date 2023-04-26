using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Behaviour
{
    public class SceneItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private Canvas _canvas;

        [SerializeField] private float _interactionDistance = 2;

        public event Action<SceneItem> ItemClicked;
        public bool TextEnabled {
            set
            {
                if (_textEnabled == value) return;
                
                _textEnabled = value;
                _canvas.enabled = value;
            }
        }

        public float InteractionDistance => _interactionDistance;

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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _interactionDistance);
        }

        public void SetItem(Sprite sprite, string itemName, Color textColor)
        {
            _spriteRenderer.sprite = sprite;
            _text.text = itemName;
            _text.color = textColor;
            TextEnabled = false;
        }

        public void DropItem(Vector2 position)
        {
            transform.position = position;
            TextEnabled = true;
        }
    }
}