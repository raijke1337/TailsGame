using Arcatech.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arcatech.Units
{
    [Serializable]
    public class ShieldController : BaseControllerConditional, IStatsComponentForHandler, ITakesTriggers
    {
        public IReadOnlyDictionary<ShieldStatType, StatValueContainer> GetShieldStats { get => _stats; }
        private Dictionary<ShieldStatType, StatValueContainer> _stats;


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
                foreach (KeyValuePair<ShieldStatType,StatValueContainer> p in cfg.Stats)
                {
                    _stats[p.Key] = new StatValueContainer(p.Value);
                    _stats[p.Key].Setup();
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

        public override void UpdateInDelta(float deltaTime)
        {
            base.UpdateInDelta(deltaTime);
            if (_equipment.TryGetValue(EquipItemType.Shield, out var s))
            {
                s.GetInstantiatedPrefab.UpdateInDelta(deltaTime);
            }
        }
    

        #endregion
        public TriggeredEffect ProcessHealthChange(TriggeredEffect effect)
        {
            if (effect.InitialValue >= 0f)
            {
                return effect;
            }
            else
            {
                var adjDmg = effect.InitialValue * _stats[ShieldStatType.ShieldAbsorbMult].GetCurrent;
                effect.InitialValue -= adjDmg;
                var AdjRep = effect.RepeatedValue * _stats[ShieldStatType.ShieldAbsorbMult].GetCurrent;
                effect.RepeatedValue -= AdjRep;

                TriggeredEffect _shieldAbsord = new TriggeredEffect(effect.ID, effect.StatType, adjDmg, AdjRep, effect.RepeatApplicationDelay, effect.TotalDuration, effect.Icon);
                _activeEffects.Add(_shieldAbsord);

                //SoundPlayCallback(EquippedShieldItem.Sounds.SoundsDict[SoundType.OnUse]); //SOUND

                return effect;
            }
        }

        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return _stats[ShieldStatType.Shield];
        }
    }

}