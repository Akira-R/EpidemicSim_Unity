using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mapbox.Unity.Map;
using Unity.AI.Navigation;
using NaughtyAttributes;

public class SimulationManager : MonoSingleton<SimulationManager>
{
    enum SimState
    {
        Idle,
        Play,
        Pause,
        End,
    }

    [Header("Map Info")]
    [SerializeField]
    private AbstractMap _abstractMap;

    private NavMeshSurface _navSurface;
    private GameObject _mapObj;

    private bool _navBakeAllow = false;
    private bool _newNavMeshRequired = true;

    private SimState _simState = SimState.Idle;

    private void OnEnable()
    {
        _abstractMap.OnInitialized += MapInitialization;
    }

    private void OnDisable()
    {
        _abstractMap.OnInitialized -= MapInitialization;
    }

    void Start()
    {
        _mapObj = _abstractMap.gameObject;
        _navSurface = _mapObj.GetComponent<NavMeshSurface>();
    }

    void Update()
    {

    }

    private void MapInitialization()
    {
        _navBakeAllow = true;
    }

    [Button]
    public void StartSimulation() 
    {
        if (_newNavMeshRequired)
            if (!NavBake()) return;

        EntityManager.Instance?.TestEntitySetup();

        _simState = SimState.Play;
    }

    private bool NavBake()
    {
        if (!_navBakeAllow) return false;

        for (int i = 1; i < _mapObj.transform.childCount; i++)
        {
            foreach (Transform component in _mapObj.transform.GetChild(i))
            {
                if (component.gameObject.name[0] == 'R')
                    component.gameObject.layer = LayerMask.NameToLayer("Nav_Walkable");
            }
        }
        _navSurface.BuildNavMesh();
        _newNavMeshRequired = false;
        return true;
    }
}
