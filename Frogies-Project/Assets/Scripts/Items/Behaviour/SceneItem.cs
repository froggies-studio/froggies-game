using System;
using DG.Tweening;
using Items.Core;
using Items.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = System.Object;

namespace Items.Behaviour
{
    public class SceneItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;
        [SerializeField] private Canvas canvas;

        [SerializeField] private float interactionDistance = 2;
        [SerializeField] private float onSceneTime = 0.8f * 60;

        private Item _item;
        private float _droppingTime;

        public event Action<SceneItem> ItemClicked;
        public event Action<SceneItem> ItemTimePassed;
        public ItemDescriptor ItemDescriptor => _item.Descriptor;
        
        public bool TextEnabled {
            set
            {
                if (_textEnabled == value) return;
                
                _textEnabled = value;
                canvas.enabled = value;
            }
        }

        public float InteractionDistance => interactionDistance;

        private bool _textEnabled = true;

        private Sequence _sequence;
        private void Awake()
        {
            _droppingTime = Time.time;
            button.onClick.AddListener(() => ItemClicked?.Invoke(this));
        }

        private void OnMouseDown()
        {
            ItemClicked?.Invoke(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interactionDistance);
        }

        public void SetItem(Item item, Color textColor)
        {
            _item = item;
            spriteRenderer.sprite = _item.Descriptor.ItemSprite;
            text.text = _item.Descriptor.ItemId.ToString();
            text.color = textColor;
            TextEnabled = false;
        }

        public void DropItem(Vector2 position)
        {
            transform.position = position;
            TextEnabled = true;
        }

        private void Update()
        {
            if (_droppingTime + onSceneTime < Time.time)
                ItemTimePassed?.Invoke(this);
        }
    }
}