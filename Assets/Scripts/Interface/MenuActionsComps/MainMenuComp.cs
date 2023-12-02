using Arcatech.Managers;
using UnityEngine;

namespace Arcatech.UI
{
    public class MainMenuComp : MonoBehaviour
    {
        #region run through unity
        public void OnGallery()
        {
            GameManager.Instance.RequestLevelLoad("gallery");
        }

        public void OnStartNewGame()
        {
            GameManager.Instance.OnStartNewGame();
        }
        public void OnQuit()
        {
            GameManager.Instance.QuitGame();
        }
        #endregion

    }
}