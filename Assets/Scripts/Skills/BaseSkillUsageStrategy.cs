using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.UI;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Skills
{
    public  class BaseSkillUsageStrategy : IUsablesStrategy    , IIconContent
    {
        public BaseUnit Owner {get;protected set;}
        SerializedUnitAction Action { get; }
        SerializedSkill Config { get; }
        protected BaseEquippableItemComponent Spawner;

        int MaxCharges { get; }
        float ChargeReload { get; }
        float InternalDelay { get; }

        Queue<CountDownTimer> _chargesTimers;
        CountDownTimer _internalCdTimer;
        int _remainingCharges;



        public BaseSkillUsageStrategy(BaseEquippableItemComponent item, SerializedUnitAction act, BaseUnit unit, SerializedSkill cfg, int charges, float reload, float intcd)
        {
            Owner = unit;
            Config = cfg;
            ChargeReload = reload;
            InternalDelay = intcd;
            MaxCharges = charges;
            Action = act;

            _remainingCharges = MaxCharges;
            _chargesTimers = new Queue<CountDownTimer>(charges);
            _internalCdTimer = new CountDownTimer(InternalDelay);
            _internalCdTimer.Start();
            Spawner = item;
        }

        public bool TryUseItem(out BaseUnitAction action)
        {
            action = Action.ProduceAction(Owner);
            if (!_internalCdTimer.IsReady || !action.IsDone) return false;
            else
            {
                if (_remainingCharges > 0)
                {
                    var t = new CountDownTimer(ChargeReload);
                    _internalCdTimer.Start();
                    t.Start();
                    _chargesTimers.Enqueue(t);
                    _remainingCharges--;
                    t.OnTimerStopped += () => _chargesTimers.Dequeue();
                    DoSkillLogic();
                    return true;
                }
            }
            return false;
        }

        protected virtual void DoSkillLogic()
        {
            Debug.Log($"No logic in skill {this}");
        }

        protected virtual void OnActionsComplete()
        {
            Debug.Log($"finished using {this}");
        }


        public virtual void Update(float delta)
        {
            foreach (var t in _chargesTimers.ToList())
            {
                t?.Tick(delta);
            }
            _internalCdTimer?.Tick(delta);
        }

        #region UI
        public Sprite Icon => Config.Description.Picture;
        public float CurrentNumber => MaxCharges - _chargesTimers.Count;
        public float MaxNumber => MaxCharges;

        #endregion

    }

    public class ProjectileSkillUsageStrategy : BaseSkillUsageStrategy
    {
        SerializedStatsEffectConfig[] Effects;
        public ProjectileSkillUsageStrategy(SerializedProjectileConfiguration proj, SerializedStatsEffectConfig[] effects, BaseEquippableItemComponent item, SerializedUnitAction act, BaseUnit unit, SerializedSkill cfg, int charges, float reload, float intcd) : base(item, act, unit, cfg, charges, reload, intcd)
        {
            Projectile = proj;
            Effects = effects;
        }

        SerializedProjectileConfiguration Projectile { get; }
        protected override void DoSkillLogic()
        {
            Projectile.ProduceProjectile(Owner, Spawner.transform, Effects);
            Debug.Log($"Used skill {this} , spawn projectile");
        }

    }

    public class SelfEffectSkillUsageStrategy : BaseSkillUsageStrategy
    {

        public SelfEffectSkillUsageStrategy(SerializedStatsEffectConfig[] effects, BaseEquippableItemComponent item, SerializedUnitAction act, BaseUnit unit, SerializedSkill cfg, int charges, float reload, float intcd) : base(item,act, unit, cfg, charges, reload, intcd)
        {
            Effects = effects;
        }
        SerializedStatsEffectConfig[] Effects;  
        protected override void DoSkillLogic()
        {
            Debug.Log($"Used skill {this} , apply {Effects} to self");

            if (Owner is DummyUnit d)
            {
                foreach (var e in Effects)
                {
                    d.ApplyEffect(new StatsEffect(e));
                }
            }
        }
    }
    public class ApplyForceSkillUsageStrategy : BaseSkillUsageStrategy
    {
        Vector3 Force;
        public ApplyForceSkillUsageStrategy(Vector3 dir, BaseEquippableItemComponent item, SerializedUnitAction act, BaseUnit unit, SerializedSkill cfg, int charges, float reload, float intcd) : base(item,act, unit, cfg, charges, reload, intcd)
        {
            Force = dir;
        }
        protected override void DoSkillLogic()
        {
            Debug.Log($"Used skill {this} , apply force {Force} to self");
            Owner.GetComponent<Rigidbody>().AddForce(Vector3.Scale(Owner.transform.forward ,Force),ForceMode.Impulse);
        }
    }
}