using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Skills
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseSkill : MonoBehaviour, IAppliesTriggers, IExpires, IManaged //IHasSounds
    {
       //protected SkillControlSettingsSO _skillConfigurationSO;


        public BaseUnit Owner { get; set; }
        protected List<BaseStatTriggerConfig> _triggers;
        protected SkillAoESettings _settings;

        public event TriggerEventApplication TriggerApplicationRequestEvent;
        public event SimpleEventsHandler<IExpires> HasExpiredEvent;

        protected Collider _coll;
        private SkillAreaOfEffectComponent _placedEffect;
        protected string _skillID;

        protected virtual void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"Trigger enter {this} {other.gameObject.name}");
            _placedEffect = PlaceAndSubEffect(transform.position);
            _coll.enabled = false;
            _placedEffect.SkillAreaDoneEvent += CallExpiry; // todo?
        }

        // use this to apply all trigger effects set in SKillData
        protected virtual void CallTriggerHit(BaseUnit target)
        {
            if (_triggers.Count == 0 ) return;

            foreach (var tr in _triggers)
            {
                TriggerApplicationRequestEvent?.Invoke(tr.ID,target, Owner);
            }
        }
        protected virtual void CallExpiry() { HasExpiredEvent?.Invoke(this); }


        protected SkillAreaOfEffectComponent PlaceAndSubEffect(Vector3 tr)
        {
            //try
            //{
            //    _audio.PlaySound(SkillData.AudioData.SoundsDict[SoundType.OnExpiry], transform.position);
            //}
            //catch
            //{
            //    Debug.Log($"No sound of type OnExpiry for skill");
            //}

            var item = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<SkillAreaOfEffectComponent>());

            item.Data = new SkillAoESettings(_settings.GrowTime, _settings.StartRad, _settings.EndRad);

            item.transform.parent = null;
            item.transform.position = tr;

            item.TargetHitEvent += (t) => CallTriggerHit(t);

            return item;
        }

        public GameObject GetObject() => gameObject;

        private bool _assigned;
        public void AssignValues(SkillControlSettingsSO cfg)
        {

            _triggers = new List<BaseStatTriggerConfig>(cfg.Triggers);
            _settings = new SkillAoESettings(cfg.Cooldown, new Vector3(cfg.StartRad,cfg.StartRad),new Vector3( cfg.EndRad,cfg.EndRad));

            _assigned = true;

        }
        public virtual void AssignValues(SkillObjectForControls cfg)
        {

            _triggers = new List<BaseStatTriggerConfig>(cfg.GetTriggers);
            _settings = new SkillAoESettings(cfg.AoESettings.GrowTime, cfg.AoESettings.StartRad, cfg.AoESettings.EndRad);
            _skillID = cfg.ID;
            _assigned = true;
        }



        #region managed
        public virtual void SetupStatsComponent()
        {
            if (!_assigned)
            {
                Debug.LogWarning($"Skill {name} spawned and was not assigned data!");
            }

            _coll = GetComponent<Collider>();
            _coll.isTrigger = true;

#if UNITY_EDITOR
            Assert.IsNotNull(_settings, $"Skill config missing from skill {name}");
#endif
            //_control = new SkillObjectForControls(_skillConfigurationSO);
        }

        public void StopStatsComponent()
        {
            
        }

        public abstract void UpdateInDelta(float deltaTime);
        #endregion
    }





}