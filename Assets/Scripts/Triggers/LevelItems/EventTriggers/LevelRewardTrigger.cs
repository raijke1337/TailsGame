using Arcatech.Items;
using CartoonFX;
using UnityEngine;

namespace Arcatech.Triggers
{
    public class LevelRewardTrigger : BaseLevelEventTrigger
    {
        public Item Content { get
            {
                return _item;
            }
            set
            {
                _item = value;
                Start();
            }        
        
        }



        [SerializeField] protected Item _item;
        [SerializeField] protected Transform _itemSpot;
        [SerializeField] protected BaseEquippableItemComponent _genericItemDisplayPrefab;
        private BaseEquippableItemComponent _model;

    protected override void OnTriggerExit(Collider other)
        {
            ItemIsGone();
            base.OnTriggerExit(other);
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (_item == null) return; // prevents early activation for drops
            if (_model != null) { Destroy(_model.gameObject); }

            base.OnTriggerEnter(other);
        }
        protected virtual void ItemIsGone()
        {
            gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            Transform place;
            if (_itemSpot != null)
            {
                place = _itemSpot;
            }
            else
            {
                place = transform;
            }

            if (Content is Equip e)
            {
                _model = Instantiate(e.ItemPrefab, place);
            }
            else
            {
                _model = Instantiate(_genericItemDisplayPrefab, place);
            }
            _model.transform.SetPositionAndRotation(place.position, place.rotation);
        }
    }
}