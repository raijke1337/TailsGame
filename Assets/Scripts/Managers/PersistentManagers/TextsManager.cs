using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Managers
{
    public class TextsManager : MonoBehaviour
    {
        #region SingletonLogic

        public static TextsManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
        }
        #endregion

        private Dictionary<string, TextContainer> textContainers = new Dictionary<string, TextContainer>();

        public TextContainer GetContainerByID(string ID)
        {
            try
            {
                return textContainers[ID];
            }
            catch
            {
                Debug.LogWarning($"No text container for ID {ID}");
                return new TextContainer();
            }
        }



    }

}