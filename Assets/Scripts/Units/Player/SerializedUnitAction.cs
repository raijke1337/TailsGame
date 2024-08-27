using UnityEngine;

namespace Arcatech.Units
{
    [CreateAssetMenu(fileName = "Unit animation action",menuName = "Units/Actions/Animation action")]
    public class SerializedUnitAction : ScriptableObject
    {
        [SerializeField] protected bool _locksMovement;
        [SerializeField] string _animationName;
        [SerializeField] NextActionSettings _nextAct;
        public BaseUnitAction ProduceAction(BaseUnit unit)
        {
            return BaseUnitAction.BuildAction(unit,_locksMovement,_nextAct,_animationName);
        }

    }
}