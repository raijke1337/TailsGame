using Arcatech.Managers;
namespace Arcatech.UI
{
    public class GalleryMenuComp : InventoryItemsHolder
    {
        public void OnBack()
        {
            GameManager.Instance.OnReturnToMain();
        }

    }

}