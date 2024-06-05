using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class ShieldController : BaseControllerConditional, IStatsComponentForHandler, ITakesTriggers
    {
      //  public IReadOnlyDictionary<ShieldStatType, StatValueContainer> GetShieldStats { get => _stats; }
        private Dictionary<ShieldStatType, StatValueContainer> _stats;
        public event SimpleEventsHandler ShieldBrokenEvent;

        #region conditional
        public ShieldController(ItemEmpties em, BaseUnit ow) : base(em, ow)
        {

        }
        protected override void FinishItemConfig(EquipmentItem item)
        {

            var cfg = DataManager.Instance.GetConfigByID<ShieldSettings>(_equipment[EquipItemType.Shield].ID);

            if (cfg == null)
            {
                IsReady = false;
                throw new Exception($"Mising cfg by ID {item.ID} from item {item} : {this}");
            }
            else
            {
                _stats = new Dictionary<ShieldStatType, StatValueContainer>();
                foreach (KeyValuePair<ShieldStatType, StatValueContainer> p in cfg.Stats)
                {
                    _stats[p.Key] = new StatValueContainer(p.Value);
                    _stats[p.Key].Setup();
                }

            }
        }
        protected override void InstantiateItem(EquipmentItem i)
        {
            i.SetItemEmpty(Empties.ItemPositions[EquipItemType.Shield]);
        }
        #endregion


        #region managed

        public override void UpdateInDelta(float deltaTime)
        {
            base.UpdateInDelta(deltaTime);
            if (_equipment.TryGetValue(EquipItemType.Shield, out var s))
            {
                s.DoUpdates(deltaTime);
                _stats[ShieldStatType.Shield].ChangeCurrent(_stats[ShieldStatType.ShieldRegen].GetCurrent * _stats[ShieldStatType.ShieldRegenMultiplier].GetCurrent* deltaTime);
            }
        }


        #endregion
        public TriggeredEffect ProcessHealthChange(in TriggeredEffect effect)
        { // debug shield logic TODO

            if (effect.InitialValue >= 0f)
            {
                return effect; // heal effect
            }
            else 
            {
                float currentSh = _stats[ShieldStatType.Shield].GetCurrent;
                float toAbsorbInitAdj = -effect.InitialValue * _stats[ShieldStatType.ShieldAbsorbMult].GetCurrent;
                // positive & positive


                if (toAbsorbInitAdj <= currentSh)
                {
                    effect.RepeatedValue = 0;
                    effect.InitialValue += toAbsorbInitAdj; // neg += positive 
                }

                if (toAbsorbInitAdj  > currentSh)
                {
                    float remains = toAbsorbInitAdj - currentSh; // positive
                    toAbsorbInitAdj = currentSh;

                    effect.InitialValue = -remains;
                    ShieldBrokenEvent?.Invoke();

                }


                TriggeredEffect _shieldAbsord = new TriggeredEffect(effect.ID, effect.StatType, -toAbsorbInitAdj, 0, effect.RepeatApplicationDelay, effect.TotalDuration, effect.Icon);
                _activeEffects.Add(_shieldAbsord);



                return effect;
            }
        }

        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return _stats[ShieldStatType.Shield];
        }

        public override string GetUIText { get => ($"{_stats[ShieldStatType.Shield].GetCurrent} / {_stats[ShieldStatType.Shield].GetMax}"); }


        #region AI
        public IReadOnlyDictionary<ShieldStatType, StatValueContainer> GetShieldStats { get => _stats; }
        #endregion
    }

}