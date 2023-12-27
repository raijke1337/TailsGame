using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Triggers.Items
{
    public abstract class ControlledItem : MonoBehaviour
    {
        public abstract void DoControlAction(bool isPositive);
    }
}