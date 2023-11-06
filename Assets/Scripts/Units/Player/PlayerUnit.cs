using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Units.Inputs;
using System.Linq;
using UnityEngine;
namespace Arcatech.Units
{
    public class PlayerUnit : BaseUnit
    {
        private InputsPlayer _playerController;
        private VisualsController _visualController;
        private float[] _visualStagesHP;
        private int _currentVisualStageIndex;
        protected Camera _faceCam;
        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }


        #region items
        public override void InitiateUnit()
        {
            base.InitiateUnit();
            UpdateComponents();

            switch (GameManager.Instance.GetCurrentLevelData.Type)
            {
                case LevelType.Menu:
                    if (_visualController == null) _visualController = GetComponent<VisualsController>();
                    _visualController.Empties = _controller.GetEmpties;
                    _visualController.ClearVisualItems();
                    _visualController.CreateVisualItem(DataManager.Instance.GetSaveData.PlayerItems.EquipmentIDs.ToArray());
                    break;
                case LevelType.Scene:
                    return;
                case LevelType.Game:
                    foreach (var eq in DataManager.Instance.GetSaveData.PlayerItems.EquipmentIDs)
                    {
                        var cfg = DataManager.Instance.GetConfigByID<Equip>(eq);
                        CreateStartingEquipments(new EquipmentItem(cfg));
                    }
                    break;
            }

            ToggleCamera(true);

            var stats = _controller.GetStatsController;

            float maxHP = stats.GetBaseStats[BaseStatType.Health].GetMax;
            stats.GetBaseStats[BaseStatType.Health].ValueChangedEvent += ChangeVisualStage;

            int stages = _visualController.StagesTotal;
            _visualStagesHP = new float[stages];
            var coef = maxHP / stages;
            for (int i = 0; i < stages; i++)
            {
                _visualStagesHP[i] = maxHP;
                maxHP -= coef;
            }
            _currentVisualStageIndex = 0;
        }

        #endregion


        protected override void UpdateComponents()
        {
            base.UpdateComponents();
            if (_faceCam == null) _faceCam = GetComponentsInChildren<Camera>().First(t => t.CompareTag("FaceCamera"));
            if (_visualController == null) _visualController = GetComponent<VisualsController>();
            _visualController.Empties = _controller.GetEmpties;
        }



        protected override void ControllerEventsBinds(bool isEnable)
        {
            base.ControllerEventsBinds(isEnable);

            if (isEnable)
            {
                _playerController = _controller as InputsPlayer;
                _playerController.ChangeLayerEvent += ChangeAnimatorLayer;
            }
            else
            {
                _playerController.ChangeLayerEvent -= ChangeAnimatorLayer;
            }
        }

        #region works

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


        //private Vector3 currVelocity;
        //[SerializeField,Tooltip("How quickly the inertia of movement fades")] private float massDamp = 3f;


        private void PlayerMovement(Vector3 desiredDir, float delta)
        {
            if (_controller.IsInputsLocked) return;
            // DO NOT FIX WHAT ISNT BROKEN //

            //if (desiredDir == Vector3.zero)
            //{
            //    if (currVelocity.sqrMagnitude < 0.1f) currVelocity = Vector3.zero;
            //    else currVelocity = Vector3.Lerp(currVelocity, Vector3.zero, Time.deltaTime * massDamp);
            //}
            //else currVelocity = desiredDir;
            //transform.position += GetStats()[StatType.MoveSpeed].GetCurrent() * Time.deltaTime * currVelocity;
            // too slide-y

            // also good enough
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

        private void ChangeVisualStage(float value, float prevvalue)
        {

            Debug.Log($"Visual stage {prevvalue} -> {value}");

            //int newIndex = _currentVisualStageIndex + 1;
            //if (newIndex >= _visualStagesHP.Count()) return;
            //if (value <= _visualStagesHP[newIndex])
            //{
            //    _currentVisualStageIndex++;
            //    _visualController.AdvanceMaterialStage();
            //}
        }

        #endregion

        #region visuals

        public void DrawItem(string ID)
        {
            _visualController.CreateVisualItem(ID);
        }
        public void HideAllItems()
        {
            _visualController.ClearVisualItems();
        }
        public void HideItem(string ID) => _visualController.RemoveVisualItem(ID);

        #endregion

    }
}