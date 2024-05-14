using Arcatech.Texts;
using Arcatech.Units.Inputs;
using System.Linq;
using UnityEngine;
namespace Arcatech.Units
{
    public class PlayerUnit : BaseUnit
    {
        private InputsPlayer _playerController;
        protected Camera _faceCam;

        [SerializeField] RuntimeAnimatorController GameplayAnimator;
        [SerializeField] RuntimeAnimatorController SceneAnimator;


        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        #region managed
        public override void InitiateUnit()
        {
            UpdateComponents();

            _animator.runtimeAnimatorController = GameplayAnimator;
            base.InitiateUnit();

            ToggleCamera(true);

            if (!IsArmed) 
                _animator.runtimeAnimatorController=SceneAnimator;

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
                if (_playerController == null) return;
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
            else if (_playerController != null) // issue in scenes
            {

                
                _playerController.ChangeLayerEvent -= ChangeAnimatorLayer;
                _playerController.ShieldBreakHappened -= HandleShieldBreakEvent;
            }
        }



        #region animator and movement



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
        public void PlayerIsTalking(DialoguePart d)
        {
            Debug.Log($"Dialogue window is shown by player panel script, dialogue is: {d.Mood}");
        }
        

        #endregion

    }
}