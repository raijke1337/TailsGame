using Arcatech.Items;
using CartoonFX;
using UnityEngine;

namespace Arcatech.Triggers
{
    public class LevelRewardTrigger : BaseLevelEventTrigger
    {
        public Item Content;
        public CFXR_Effect Effect;
        [SerializeField] private Transform _itemSpot;
        private BaseEquippableItemComponent _model;

        protected override void OnEnter()
        {
            Destroy(_model.gameObject);
            if (Effect != null)
            {
                Instantiate(Effect, transform);
            }
        }

        protected override void OnExit()
        {
            ItemIsGone();
        }

        protected virtual void ItemIsGone()
        {
            this.gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            if (Content is Equip e && _itemSpot != null)
            {
                _model = Instantiate(e.Item,transform);
                _model.transform.SetPositionAndRotation(transform.position, transform.rotation);

            }
        }
    }
}