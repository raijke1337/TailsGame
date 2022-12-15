using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotates : MonoBehaviour
{

    [Range(0, 1f), SerializeField] private float SpeedMult = 0.1f;

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += new Vector3(0,1f,0) * SpeedMult * Time.deltaTime;
    }
}
