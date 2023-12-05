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
        }
        protected override void InstantiateItem(EquipmentItem i)
        {
            var b = i.GetInstantiatedPrefab;
            b.transform.parent = Empties.SheathedWeaponEmpty;
            b.transform.SetPositionAndRotation(Empties.SheathedWeaponEmpty.position, Empties.SheathedWeaponEmpty.rotation);
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
                var t = new Timer(_stats[DodgeStatType.Cooldown].GetCurrent);
                _timerQueue.Enqueue(t);
                t.TimeUp += T_TimeUp;
                SoundPlayCallback(_equipment[EquipItemType.Booster].GetAudio(EffectMoment.OnStart));
                ParticlesPlayCallback(_equipment[EquipItemType.Booster].GetParticle(EffectMoment.OnStart),Empties.OthersEmpty);
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