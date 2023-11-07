using Arcatech.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class DodgeController : BaseControllerConditional, IStatsComponentForHandler, ITakesTriggers
    {

        Dictionary<DodgeStatType, StatValueContainer> _stats;
        public IReadOnlyDictionary<DodgeStatType, StatValueContainer> GetDodgeStats { get { return _stats; } }
        public ItemEmpties Empties { get; }
        public DodgeController(ItemEmpties ie) => Empties = ie;
        public int GetDodgeCharges() => _stats != null ? (int)_stats[DodgeStatType.Charges].GetCurrent : 0;

        private Queue<Timer> _timerQueue = new Queue<Timer>();

        public override void SetupStatsComponent()
        {
            if (!IsReady) return;
            _stats = new Dictionary<DodgeStatType, StatValueContainer>();
            var cfg = DataManager.Instance.GetConfigByID<DodgeStatsConfig>(CurrentlyEquippedItem.ID);

            if (cfg == null)
            {
                IsReady = false;
                throw new Exception($"Mising cfg by ID {CurrentlyEquippedItem.ID} from item {CurrentlyEquippedItem} : {this}");
            }

            foreach (var c in cfg.Stats)
            {
                _stats[c.Key] = new StatValueContainer(c.Value);
            }
            foreach (var st in _stats.Values)
            { st.Setup(); }
        }

        public override void LoadItem(EquipmentItem item, out string skill)
        {
            skill = null;
            if (item.ItemType == EquipItemType.Booster)
            {
                base.LoadItem(item, out skill);
                CurrentlyEquippedItem = item;
            }
            
        }


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
                //SoundPlayCallback(EquippedDodgeItem.GameItem.Sounds.SoundsDict[SoundType.OnUse]);
                return true;
            }
        }

        private void T_TimeUp(Timer arg)
        {
            _timerQueue.Dequeue();
            _stats[DodgeStatType.Charges].ChangeCurrent(1);
        }

        public override void UpdateInDelta(float deltaTime)
        {
            foreach (var timer in _timerQueue.ToList()) timer.TimerTick(deltaTime);
            base.UpdateInDelta(deltaTime);
        }



        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return _stats[DodgeStatType.Charges];
        }

    }



}