using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Skills;
using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Units
{

    [Serializable]
    public class DodgeController : BaseControllerConditional, IManagedComponent, ITakesTriggers

    { // ALL LOGIC FOR TIMERS ETC MOVED TO DODGE SKILL CTRL OBJECT (Skill controller)
        public DodgeController(ItemEmpties em, BaseUnit ow) : base(em, ow)
        {

        }


        #region conditional

        protected override void FinishItemConfig(EquipmentItem item)
        {

            DodgeSkillConfigurationSO cfg = (DodgeSkillConfigurationSO)item.Skill;

            if (cfg == null)
            {
                IsReady = false;
            }
            //_booster = _items[EquipItemType.Booster];
        }
        protected override void InstantiateItem(EquipmentItem i)
        {
            i.SetItemEmpty(Empties.ItemPositions[EquipmentType.Booster]);
        }
        #endregion

        #region managed
        public override void StartComp()
        {
            if (!IsReady) // set ready by running OnItemAssign
            {
                // Debug.Log($"{this} is not ready for setup, items: {_equipment.Values.Count}");
                return;
            }
        }

        public override void UpdateInDelta(float deltaTime)
        {
        }

        public override void ApplyEffect(TriggeredEffect effect)
        {

        }

        public override void StopComp()
        {

        }

        #endregion

        #region ctrl

        //private Queue<Timer> _timerQueue = new Queue<Timer>();
       // private EquipmentItem _booster;


        //public bool IsDodgePossibleCheck()
        //{
        //    Debug.Log($"Check dodge in dodge ctrl, {_stats[DodgeStatType.Charges]} charges ");
        //    if (!IsReady) return false;


        //    if (_stats[DodgeStatType.Charges].GetCurrent == 0f) return false;
        //    else
        //    {
        //        _stats[DodgeStatType.Charges].ChangeCurrent(-1);
        //        var t = new Timer(_booster.Skill.Cooldown);
        //        _timerQueue.Enqueue(t);
        //        t.TimeUp += T_TimeUp;

        //       // EffectEventCallback(new EffectRequestPackage(_booster.Effects, EffectMoment.OnStart, _booster.GetInstantiatedPrefab().transform));

        //        return true;
        //    }
        //}

        //private void T_TimeUp(Timer arg)
        //{
        //    _timerQueue.Dequeue();
        //    _stats[DodgeStatType.Charges].ChangeCurrent(1);
        //    arg.TimeUp -= T_TimeUp; 
        //}


        #endregion

    }
}