using UnityEngine;
namespace Arcatech.UI
{
    public class SettingsMenuComp : MonoBehaviour
    {
        public void OnReset()
        {
            Debug.Log($"Settings reset NYI");
        }
        public void OnSave()
        {
            Debug.Log($"Settings save NYI");
        }

        public void NewSetting(string typ)
        {
            Debug.Log($"Chaned setting {typ} NYI");
        }
    }
}