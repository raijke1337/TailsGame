using Arcatech.Units;
using UnityEngine;

public class DoorOpensWhenEnemyDIes : MonoBehaviour
{
    [SerializeField] private BaseUnit UnitToKill;
    [SerializeField] private float SpeedMult = 1f;
    [SerializeField] private float AnimationTime = 1f;

    private void Start()
    {
        if (UnitToKill != null)
        {
            UnitToKill.BaseUnitDiedEvent += UnitToKill_BaseUnitDiedEvent;
        }
    }

    private void UnitToKill_BaseUnitDiedEvent(BaseUnit arg)
    {
        Open();
    }

    private void Open()
    {
        UnitToKill.BaseUnitDiedEvent -= UnitToKill_BaseUnitDiedEvent;
        Destroy(gameObject);
    }
}
