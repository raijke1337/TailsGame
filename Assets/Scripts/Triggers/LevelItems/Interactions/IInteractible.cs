using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Triggers
{
    public interface IInteractible : ITargetable
    {
        void AcceptInteraction(IInteractor actor);
        Vector3 Position { get; }
    }



}