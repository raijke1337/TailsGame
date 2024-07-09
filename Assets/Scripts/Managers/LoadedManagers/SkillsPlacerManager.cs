using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Skills;
using Arcatech.Units;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    public class SkillsPlacerManager : MonoBehaviour, IManagedController
    {


        EventBinding<SpawnSkillEvent> skillPlaceBind;



        #region ManagerBase
        public virtual void StartController()
        {
            skillPlaceBind = new EventBinding<SpawnSkillEvent>(ServeSkillRequest);
            EventBus< SpawnSkillEvent>.Register(skillPlaceBind);
        }

        public virtual void ControllerUpdate(float delta)
        {

        }
        public virtual void FixedControllerUpdate(float fixedDelta)
        {

        }
        public virtual void StopController()
        {
            EventBus<SpawnSkillEvent>.Deregister(skillPlaceBind);
        }

        #endregion


        public void ServeSkillRequest(SpawnSkillEvent ev)
        {

            Debug.Log($"serve skill reqeust ");
        }

    }

}