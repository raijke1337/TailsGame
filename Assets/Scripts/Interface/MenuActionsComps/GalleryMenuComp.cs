public class GalleryMenuComp : MenuPanelTiled
{
    public void OnBack()
    {
        GameManager.Instance.RequestLevelLoad("main");
    }

}

