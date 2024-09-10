using Arcatech.Actions;
using UnityEngine;

namespace Arcatech.Units
{
    [CreateAssetMenu(fileName = "Unit animation action",menuName = "Actions/Animation action")]
    public class SerializedUnitAction : ScriptableObject
    {
        [SerializeField] protected bool _locksMovement;
        [SerializeField] string _animationName;
        [SerializeField,Range(0f,1f)] protected float _exitTime;
        [SerializeField] NextActionSettings _nextAct;
        [SerializeField] SerializedActionResult _onStart;
        [SerializeField] SerializedActionResult _onFinish;
        public BaseUnitAction ProduceAction(BaseUnit unit)
        {
            return BaseUnitAction.BuildAction(unit,_locksMovement,_nextAct,_animationName,_exitTime,_onStart,_onFinish);
        }

    }
}