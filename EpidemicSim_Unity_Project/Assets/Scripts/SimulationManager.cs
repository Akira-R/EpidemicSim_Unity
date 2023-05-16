using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mapbox.Unity.Map;
using Unity.AI.Navigation;
using NaughtyAttributes;
using Unity.VisualScripting;

public class SimulationManager : MonoSingleton<SimulationManager>
{
    enum SimState
    {
        Idle,
        Play,
        Pause,
        ReInit,
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
        EventManager.Instance.AddListener<EntityManager.OnPlaceModified>(OnPlaceModified);
    }

    private void OnDisable()
    {
        _abstractMap.OnInitialized -= MapInitialization;
        EventManager.Instance.RemoveListener<EntityManager.OnPlaceModified>(OnPlaceModified);
    }

    void Start()
    {
        _mapObj = _abstractMap.gameObject;
        _navSurface = _mapObj.GetComponent<NavMeshSurface>();
    }

    private void MapInitialization()
    {
        _navBakeAllow = true;
    }

    [Button]
    public void StartSimulation() 
    {
        if (_simState == SimState.Idle)
        {
            if (_newNavMeshRequired)
                if (!NavBake()) return;

            EntityManager.Instance.TestEntitySetup();
            GetMapVisualMat();
            EntityManager.Instance.UnitFirstPath();

            _simState = SimState.Play;
        }
        else if (_simState == SimState.ReInit)
        {
            EntityManager.Instance.PlaceEntitySetup();

            _simState = SimState.Play;
        }
    }

    [Button]
    public void ResetSimulation()
    {
        _simState = SimState.Idle;  
        EntityManager.Instance.ClearEntity();
    }

    private bool NavBake()
    {
        //if (!_navBakeAllow) return false;

        for (int i = 1; i < _mapObj.transform.childCount; i++)
        {
            foreach (Transform component in _mapObj.transform.GetChild(i))
            {
                if (component.tag == "RoadObj")
                    component.gameObject.layer = LayerMask.NameToLayer("Nav_Walkable");
            
            }
        }
        _navSurface.BuildNavMesh();
        _newNavMeshRequired = false;
        return true;
    }

    private void OnPlaceModified(IEvent e)
    {
        EntityManager.OnPlaceModified data = e as EntityManager.OnPlaceModified;
        if (data == null) return;
            _simState = SimState.ReInit;
    }

    private void GetMapVisualMat() 
    {
        // Get all building Objs
        for (int i = 1; i < _mapObj.transform.childCount; i++)
        {
            VisualFilter.Instance?.AddMapCompMat(_mapObj.transform.GetChild(i).GetComponent<Renderer>().material);
            //Debug.Log(_mapObj.transform.GetChild(i).gameObject);
            foreach (Transform transform in _mapObj.transform.GetChild(i))
            {
                Renderer transformRender = transform.GetComponent<Renderer>();
                foreach (Material mat in transformRender.materials)
                {
                    VisualFilter.Instance?.AddMapCompMat(mat);
                }
            }
        }
    }

    public int GetSimState() 
    {
        return (int)_simState;
    }
}
