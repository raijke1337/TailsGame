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

        public StateMachine GetStateMachine { get => _npcController.GetFSM; }
        public void SetUnitRoom(RoomController room) => _npcController.UnitRoom = room;

        //used by room ctrl
        public event SimpleEventsHandler<NPCUnit> OnUnitAttackedEvent;
        //public event SimpleEventsHandler<NPCUnit> OnUnitSpottedPlayerEvent;
        [SerializeField] protected UnitItemsSO _defaultItems;





        public override void InitiateUnit()
        {
            base.InitiateUnit();
            if (!CompareTag("Enemy"))
                Debug.LogWarning($"Set enemy tag for{name}");

            _controller.GetStatsController.GetBaseStats[BaseStatType.Health].ValueChangedEvent += OnOuch;

        }
        // used by room controller
        public void ForceCombat()
        {
            _npcController.ForceCombat();
        }
        private void OnOuch(float curr, float prev)
        {
            if (curr < prev)
            {
                ForceCombat();
                OnUnitAttackedEvent?.Invoke(this);
            }
        }
        public void AiToggle(bool isProcessing)
        {
            if (_npcController == null) SetNPCInputs();
            _npcController.SwitchState(isProcessing);
            // todo thi is a bandaid
        }

        private void SetNPCInputs() => _npcController = _controller as InputsNPC;
        public virtual void OnUnitSpottedPlayer()
        {
            // nothing here
            Debug.Log($"{GetFullName} saw player");
        }


        protected override void InitInventory()
        {
            GetUnitInventory = new UnitInventoryComponent(DataManager.Instance.GenerateDefaultInventory(_defaultItems), this);
            CreateStartingEquipments(GetUnitInventory);

        }

    }

}