using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Level
{
    public class EditorFlagHIder : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var mesh = gameObject.GetComponent<MeshFilter>();
            mesh.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}