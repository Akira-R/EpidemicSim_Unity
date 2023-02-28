using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mapbox.Unity.Map;
using Unity.AI.Navigation;
using NaughtyAttributes;

public class SimulationManager : MonoBehaviour
{
    enum SimState 
    {
        WaitToStart,
        Play,
        Pause,
        End,
        Length // for determine the size 
    }

    [Header("Map")]
    [SerializeField]
    private AbstractMap _abstractMap;

    private bool _newNavMeshRequired = true;

    
    void Start()
    {
        
    }

    void Update()
    {

    }
}
