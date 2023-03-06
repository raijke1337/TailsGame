using UnityEditor;
using UnityEngine;

public class MainMenuCanvasController : MonoBehaviour
{

    [SerializeField] MenuPanel Main;
    [SerializeField] MenuPanel Continue;
    [SerializeField] MenuPanel Options;

    private void Start()
    {
        Main.OnToggle(true);
    }
    public void OnSettings()
    {
        Main.OnToggle(false);
        Continue.OnToggle(false);
        Options.OnToggle(true);
    }

    public void OnContinue()
    {
        Main.OnToggle(false);
        Continue.OnToggle(true);
        Options.OnToggle(false);
    }
    public void OnMain()
    {
        Main.OnToggle(true);
        Continue.OnToggle(false);
        Options.OnToggle(false);
    }

    public void OnQuit()
    {
        GameManager.Instance.QuitGame();
    }

}
