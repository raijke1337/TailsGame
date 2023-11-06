using Arcatech.Managers;

namespace Arcatech.UI
{
    public class MainMenuComp : MenuPanel
    {
        public void OnGallery()
        {
            GameManager.Instance.RequestLevelLoad("gallery");
        }

        public void OnStartNewGame()
        {
            GameManager.Instance.OnStartNewGame();
        }


    }
}