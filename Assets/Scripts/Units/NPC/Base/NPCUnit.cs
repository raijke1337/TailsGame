using Arcatech.AI;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Triggers;
using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.Units
{
    [RequireComponent(typeof(InputsNPC))]
    public class NPCUnit : BaseUnit
    {
        private InputsNPC _npcController;
        public RoomUnitsGroup UnitsGroup { get; set; }

        //used by room ctrl
        public event SimpleEventsHandler<NPCUnit> OnUnitAttackedEvent;
        [SerializeField] protected UnitItemsSO _defaultItems;
        [SerializeField] private LevelRewardTrigger _dropPrefab;
        [SerializeField] private Item _drop; 


        [Space, SerializeField] protected EnemyStatsConfig _enemyStatsConfig;

        public override void InitiateUnit()
        {
            base.InitiateUnit();
            if (_npcController == null) _npcController = _inputs as InputsNPC;
            if (!CompareTag("Enemy"))
                Debug.LogWarning($"Set enemy tag for{name}");
            _npcController.UnitsGroup = UnitsGroup;
        }

        protected override UnitInventoryItemConfigsContainer SelectSerializedItemsConfig()
        {
            return new UnitInventoryItemConfigsContainer( _defaultItems);
        }

        public override ReferenceUnitType GetUnitType()
        {
            return _enemyStatsConfig.UnitType;
        }

        protected override void HandleDamage(float value)
        {
            _npcController.ForceCombat();
            OnUnitAttackedEvent?.Invoke(this);
        }
        public void ForceCombat() => _npcController.ForceCombat();
        protected override void HandleDeath()
        {
            if (_drop != null)
            {
                var drop = Instantiate(_dropPrefab, transform.position, transform.rotation);
                drop.Content = _drop;
                GameManager.Instance.GetGameControllers.LevelManager.RegisterNewTrigger(drop, true);
            }

        }

        protected override void OnItemAdd(Item i)
        {
            if (_inputs.DebugMessage)
            {
                Debug.Log($"Added item {i.Description.Title}");
            }
        }
    }

}