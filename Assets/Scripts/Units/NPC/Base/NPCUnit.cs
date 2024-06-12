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
            if (_npcController == null) _npcController = _controller as InputsNPC;
            if (!CompareTag("Enemy"))
                Debug.LogWarning($"Set enemy tag for{name}");
            _npcController.UnitsGroup = UnitsGroup;
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

        protected override void HandleDamage(float value)
        {
            _npcController.ForceCombat();
            OnUnitAttackedEvent?.Invoke(this);
        }

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
            if (_controller.DebugMessage)
            {
                Debug.Log($"Added item {i.Description.Title}");
            }
        }

        protected override void StartJump()
        {
            if (_controller.DebugMessage)
            {
                Debug.Log($"{GetFullName} wants to jump but can't!");
            }
        }

        protected override void LandJump(string arg)
        {
            if (_controller.DebugMessage)
            {
                Debug.Log($"{GetFullName} wants to land jump but can't!");
            }
        }
    }

}