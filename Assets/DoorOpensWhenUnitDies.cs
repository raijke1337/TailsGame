using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpensWhenUnitDies : MonoBehaviour
{
    public BaseUnit Unit;
    private Vector3 start;
    [SerializeField] private Vector3 end;

    private void Start()
    {
        Unit.BaseUnitDiedEvent += Unit_BaseUnitDiedEvent;
    }

    private void Unit_BaseUnitDiedEvent(BaseUnit arg)
    {
        start = transform.position;
        StartCoroutine(MoveDown());
    }

    private IEnumerator MoveDown()
    {
        float progress = 0;
        while (progress <= 1)
        {
            progress += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, progress);
        }
        yield return null;
    }
}
