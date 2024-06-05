using Arcatech.AI;
using Arcatech.Items;
using Arcatech.Managers;
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
        [SerializeField] protected EnemyStatsConfig _enemyStatsConfig;

        public override void InitiateUnit()
        {
            base.InitiateUnit();
            if (_npcController == null) _npcController = _controller as InputsNPC;
            if (!CompareTag("Enemy"))
                Debug.LogWarning($"Set enemy tag for{name}");
            _npcController.UnitsGroup = UnitsGroup;
        }
        // used by room controller
        public void ForceCombat()
        {
            _npcController.ForceCombat();
        }

        protected override void OnDamageEvent(float value)
        {
            base.OnDamageEvent(value);
            ForceCombat();
            OnUnitAttackedEvent?.Invoke(this);
        }

        protected override void InitInventory()
        {
            GetUnitInventory = new UnitInventoryComponent(new UnitInventoryItemConfigsContainer(_defaultItems),this);
            CreateStartingEquipments(GetUnitInventory);
        }

        public override ReferenceUnitType GetUnitType()
        {
            return _enemyStatsConfig.UnitType;
        }
    }

}