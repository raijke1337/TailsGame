using Arcatech.Managers;
namespace Arcatech.UI
{
    public class GalleryMenuComp : MenuPanelTiled
    {
        public void OnBack()
        {
            GameManager.Instance.OnReturnToMain();
        }

    }

}