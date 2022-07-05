using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Puzzle
{
    [RequireComponent(typeof(RectTransform))]
    public class TileComponent : MonoBehaviour, IPointerClickHandler
    {
        public bool IsOpen { get; set; }
        public Dictionary<bool, Sprite> ImageDict = new Dictionary<bool, Sprite>();
        private Image _currentImg;

        public TileComponent Pair { get; set; }



        public event PuzzleManager.MiniGameEvents<TileComponent> OnFlipEvent;

        public void SetImages(bool isOpen, Sprite image) => ImageDict[isOpen] = image;        

        public void Flip()
        {
            IsOpen = !IsOpen;
            _currentImg.sprite = ImageDict[IsOpen];
        }

        private void Start()
        {
            _currentImg = GetComponent<Image>();
            _currentImg.sprite = ImageDict[false];
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnFlipEvent?.Invoke(this);
        }
    }

}