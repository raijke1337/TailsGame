using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class TrapWithTimerTrigger : StatsChangingTrigger
{
    public float Timer = 2f;
    private bool isEnabled = true;
    private Coroutine TogglingCor;
    ParticleSystem _trapEffect;

    private void Start()
    {
        _trapEffect = GetComponent<ParticleSystem>();
        StartCor();
    }


    private IEnumerator Toggling(float time)
    {
        float passed = 0f;
        while (passed < time)
        {
            passed += Time.deltaTime;
            yield return null;
        }
        isEnabled = !isEnabled;
        StartCor();
        yield return null;
    }
    private void StartCor()
    {
        TogglingCor = StartCoroutine(Toggling(Timer));
    }
    private void Update()
    {
        _coll.enabled = _trapEffect.enableEmission = isEnabled;
        // todo proper setup
    }

}

