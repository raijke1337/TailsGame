using Arcatech.Units.Inputs;
using System.Linq;
using UnityEngine;
namespace Arcatech.Units
{
    public class PlayerUnit : BaseUnit
    {
        private InputsPlayer _playerController;
        protected Camera _faceCam;


        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        #region managed
        public override void InitiateUnit()
        {
            base.InitiateUnit();
            UpdateComponents();

            ToggleCamera(true);

            //var stats = _controller.GetStatsController;

            //float maxHP = stats.GetBaseStats[BaseStatType.Health].GetMax;
            // stats.GetBaseStats[BaseStatType.Health].ValueChangedEvent += ChangeVisualStage;

            // int stages = _visualController.StagesTotal;
            //_visualStagesHP = new float[stages];
            //var coef = maxHP / stages;
            //for (int i = 0; i < stages; i++)
            //{
            //  _visualStagesHP[i] = maxHP;
            //  maxHP -= coef;
            //}
        }
        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            if (_controller.InputDirectionOverride != Vector3.zero) // for Scenes
            {
                PlayerMovement(_controller.InputDirectionOverride, delta);
            }
            else
            {
                PlayerMovement(_playerController.GetMoveDirection, delta); // weird ass null here sometimes
            }

        }

        #endregion

        protected override void UpdateComponents()
        {
            base.UpdateComponents();
            if (_faceCam == null) _faceCam = GetComponentsInChildren<Camera>().First(t => t.CompareTag("FaceCamera"));
        }

        protected override void ControllerEventsBinds(bool isEnable)
        {
            base.ControllerEventsBinds(isEnable);

            if (isEnable)
            {
                _playerController = _controller as InputsPlayer;
                _playerController.ChangeLayerEvent += ChangeAnimatorLayer;
                _playerController.ShieldBreakHappened += HandleShieldBreakEvent;
            }
            else
            {
                _playerController.ChangeLayerEvent -= ChangeAnimatorLayer;
                _playerController.ShieldBreakHappened -= HandleShieldBreakEvent;
            }
        }



        #region animator and movement



        //private Vector3 currVelocity;
        //[SerializeField,Tooltip("How quickly the inertia of movement fades")] private float massDamp = 3f;


        private void PlayerMovement(Vector3 desiredDir, float delta)
        {
            if (_controller.IsInputsLocked) return;
            transform.position += delta * desiredDir
                * GetStats[BaseStatType.MoveSpeed].GetCurrent;
        }

        private void ChangeAnimatorLayer(EquipItemType type)
        {
            // 1  2 is ranged 3  is melee
            switch (type)
            {
                case EquipItemType.MeleeWeap:
                    _animator.SetLayerWeight(0, 0f);
                    _animator.SetLayerWeight(1, 0f);
                    _animator.SetLayerWeight(2, 100f);
                    break;
                case EquipItemType.RangedWeap:
                    _animator.SetLayerWeight(0, 100f);
                    _animator.SetLayerWeight(1, 100f);
                    _animator.SetLayerWeight(2, 0f);
                    break;
            }
        }

        public void ComboWindowStart()
        {
            _animator.SetBool("AdvancingCombo", true);
            _playerController.IsInMeleeCombo = true;
        }
        public void ComboWindowEnd()
        {
            _animator.SetBool("AdvancingCombo", false);
            _playerController.IsInMeleeCombo = false;
        }



        #endregion

        #region inventory

        protected override void InitInventory()
        {

            var savedEquips = DataManager.Instance.GetSaveData.Items;

            GetUnitInventory = new UnitInventoryComponent(savedEquips, this);
            CreateStartingEquipments(GetUnitInventory);
        }
        #endregion


        #region visual
        private void HandleShieldBreakEvent()
        {
            Debug.Log($"Inputs report shield break event");
        }

        #endregion

    }
}