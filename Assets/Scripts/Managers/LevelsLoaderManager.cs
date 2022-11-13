using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsLoaderManager : SingletonManagerBase
{
    private LevelData[] _levels;
    #region SingletonLogic

    protected static LevelsLoaderManager _instance = null;
    public override void InitSingleton()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance == this) Destroy(gameObject); // remove copies just in case
        SetupManager();
    }

    public override void SetupManager()
    {
        var cfg = Extensions.GetConfigByID<LevelsDictionary>().Datas;
        _levels = new LevelData[cfg.Length];
        for (int i = 0; i < cfg.Length; i++)
        {
            _levels[i] = cfg[i];
        }
    }

    public static LevelsLoaderManager GetInstance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    public LevelData[] GetLevelData
    {
        get
        {
            return _levels;
        }
    }


    private int _reqLevel;

    /// <summary>
    /// set load ondex and load pre level scene if index is a game level
    /// </summary>
    /// <param name="index">scene build index </param>
    public void RequestLevelLoad(int index)
    {
        if (index >= 2)
        {
            _reqLevel = index;
            SceneManager.LoadScene(1); // item selection
        }
        else SceneManager.LoadScene(index);
    }
    /// <summary>
    /// load previously requested level
    /// </summary>
    public void RequestLevelLoad()
    {
        SceneManager.LoadScene(_reqLevel);
    }

}
