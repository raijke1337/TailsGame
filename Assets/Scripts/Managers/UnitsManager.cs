using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Zenject;
using System.Threading.Tasks;

public class UnitsManager : MonoBehaviour
{
   [Inject]
    public PlayerUnit GetPlayer { get; private set; }


    private UnitStatsUpdater _updater;
    public IReadOnlyCollection<NPCUnit> GetNPCs => _bots;
    private LinkedList<NPCUnit> _bots = new LinkedList<NPCUnit>();

    [SerializeField]
    private Transform _NPCpool;

    private async void Start()
    {
        var list = _NPCpool.GetComponentsInChildren<NPCUnit>();
        foreach (var entry in list)
        {
            _bots.AddLast(entry);
        }
        _updater = new UnitStatsUpdater(_bots, GetPlayer);

        await Task.Yield();

        _updater.RegisterUnitInScene(GetPlayer);
        foreach (var unit in _bots)
        {
            _updater.RegisterUnitInScene(unit);
        }
    }

    private void Update()
    {
        _updater.CalculateStatsOnUpdate(Time.deltaTime);
    }

    public void CreateUnit(NPCUnit unit)
    {
       _updater.RegisterUnitInScene(unit);
    }
    public void DeleteUnit(NPCUnit unit)
    {
        _updater.RegisterUnitInScene(unit,false);
    }

}

