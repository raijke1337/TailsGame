using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Level
{
    public class FadingDecorComponent : MonoBehaviour//, IEquatable<FadingDecorComponent>
    {
        public List<Renderer> Renderers;
        public Vector3 Position;
        public List<Material> Materials;

        [HideInInspector] public float InitialAlpha;

        private void Awake()
        {
            Position = transform.position;
            if (Renderers.Count == 0)
            {
                Renderers.AddRange(GetComponentsInChildren<Renderer>()); // all or parts
            }
            foreach (var r in Renderers)
            {
                Materials.AddRange(r.materials);
            }

            if (Materials[0].HasProperty("_Color"))
            {
                InitialAlpha = Materials[0].color.a; // fade to fadedAlpha in fader comp then back to initial // it is poossible to implement an array 
            }
            else if (Materials[0].HasProperty("_BaseColor"))
            {
                InitialAlpha =  Materials[0].GetColor("_BaseColor").a;
            }
            else
            {
                Debug.LogWarning($"{Materials[0]} has no colors propereties and failed to setup in {this}");
            }
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}