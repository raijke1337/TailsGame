using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ServiceGolemUnit : NPCUnit
{

    protected override void Start()
    {
        base.Start();
        (_controller as ServiceGolemInputs).SetDodgeCtrl(GetID);
    }

    #region dodge

    private Coroutine _dodgeCor;
    // stop the dodge like this
    private void OnCollisionEnter(Collision collision)
    {
        if (_dodgeCor != null && collision.gameObject.tag != "Ground")
        {
            _controller.IsControlsBusy = false;
            StopCoroutine(_dodgeCor);
        }
    }
    protected override void DodgeAction()
    {
        _dodgeCor = StartCoroutine(DodgingMovement());
    }
    private IEnumerator DodgingMovement()
    {
        var stats = (_controller as ServiceGolemInputs).GetDodgeController.GetDodgeStats;
        _controller.IsControlsBusy = true;

        Vector3 start = transform.position;
        Vector3 end = start + _controller.MoveDirection * stats[DodgeStatType.Range].GetCurrent;

        float p = 0f;
        while (p <= 1f)
        {
            p += Time.deltaTime * stats[DodgeStatType.Speed].GetCurrent;
            transform.position = Vector3.Lerp(start, end, p);
            yield return null;
        }
        _controller.IsControlsBusy = false;
        yield return null;
    }


    #endregion

}

