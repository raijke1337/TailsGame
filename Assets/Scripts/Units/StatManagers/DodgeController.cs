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
    public class DodgeController : BaseControllerConditional, IStatsComponentForHandler, ITakesTriggers

    {
        public DodgeController(ItemEmpties em, BaseUnit ow) : base(em, ow)
        {

        }


        Dictionary<DodgeStatType, StatValueContainer> _stats;
        public IReadOnlyDictionary<DodgeStatType, StatValueContainer> GetDodgeStats { get { return _stats; } }
        public int GetDodgeCharges() => _stats != null ? (int)_stats[DodgeStatType.Charges].GetCurrent : 0;

        private Queue<Timer> _timerQueue = new Queue<Timer>();
        private EquipmentItem _booster;

#region conditional

        protected override void FinishItemConfig(EquipmentItem item)
        {

            DodgeSkillConfiguration cfg = (DodgeSkillConfiguration) item.ItemSkillConfig;

            if (cfg == null)
            {
                IsReady = false;
               // throw new Exception($"Mising cfg by ID {item.ID} from item {item} : {this}");
            }
            else
            {
                _stats = new Dictionary<DodgeStatType, StatValueContainer>();
                

                foreach (var c in cfg.DodgeSkillStats)
                {
                    _stats[c.Key] = new StatValueContainer(c.Value);
                }
                foreach (var st in _stats.Values)
                { 
                    st.Setup();
                }
            }
            _booster = _equipment[EquipItemType.Booster];
        }
        protected override void InstantiateItem(EquipmentItem i)
        {
            i.SetItemEmpty(Empties.ItemPositions[EquipItemType.Booster]);
        }
#endregion

        #region managed
        public override void SetupStatsComponent()
        {
            if (!IsReady) // set ready by running OnItemAssign
            {
                // Debug.Log($"{this} is not ready for setup, items: {_equipment.Values.Count}");
                return;
            }
        }

        public override void UpdateInDelta(float deltaTime)
        {
            foreach (var timer in _timerQueue.ToList()) timer.TimerTick(deltaTime);
            base.UpdateInDelta(deltaTime);
        }

        #endregion

        #region dodging
        public bool IsDodgePossibleCheck()
        {
            if (_stats == null) return false;
            if (_stats[DodgeStatType.Charges].GetCurrent == 0f) return false;
            else
            {
                _stats[DodgeStatType.Charges].ChangeCurrent(-1);
                var t = new Timer(_booster.ItemSkillConfig.Cooldown);
                _timerQueue.Enqueue(t);
                t.TimeUp += T_TimeUp;

                EffectEventCallback(new EffectRequestPackage(_booster.GetEffects, EffectMoment.OnStart, _booster.GetInstantiatedPrefab().transform));

                return true;
            }
        }

        private void T_TimeUp(Timer arg)
        {
            _timerQueue.Dequeue();
            _stats[DodgeStatType.Charges].ChangeCurrent(1);
        }


        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return _stats[DodgeStatType.Charges];
        }


        #endregion

    }
}