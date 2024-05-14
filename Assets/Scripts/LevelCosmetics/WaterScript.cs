using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    [SerializeField] protected Vector3 _tgtLocalPosition;
    [SerializeField] protected float _speedMult;
    private Vector3 _startPosition;
    private Vector3 _desiredPosition;


    private void Start()
    {
        _startPosition = transform.localPosition;
        _desiredPosition = transform.localPosition + _tgtLocalPosition;
        if (_desiredPosition != _startPosition)
        {
            StartCoroutine(MoveToDesired());
        }
    }

    private IEnumerator MoveToDesired()
    {
        float t = 0;

        while (transform.localPosition != _desiredPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,_desiredPosition,t);
            t+= Time.deltaTime * _speedMult;
            yield return null;
        }
        StartCoroutine(MoveBack());
        yield return null;
    }
    private IEnumerator MoveBack()
    {
        float t = 0;
        while (transform.localPosition != _startPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startPosition, t);
            t += Time.deltaTime * _speedMult;
            yield return null;
        }
        StartCoroutine(MoveToDesired());
        yield return null;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
