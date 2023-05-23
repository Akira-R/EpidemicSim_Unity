using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using NaughtyAttributes;


public class EntityManager : MonoSingleton<EntityManager>
{
    public class OnPlaceModified : IEvent { }

    [Header("Entity Info")]
    [SerializeField]
    private GameObject _unitPrefab;
    [SerializeField]
    private GameObject _placePrefab;
    [SerializeField]
    private List<UnitEntity> _units;
    [SerializeField]
    private List<PlaceEntity> _places;

    public List<UnitEntity> Units { get { return _units; } }
    public List<PlaceEntity> Places { get { return _places; } }

    public class TwoKeyDictionary<K1, K2, T> : Dictionary<K1, Dictionary<K2, T>> 
    {
        public void Add(K1 key1, K2 key2, T value)
        {
            if (!ContainsKey(key1))
            {
                this[key1] = new Dictionary<K2, T>();
            }
            this[key1][key2] = value;
        }

        public T Get(K1 key1, K2 key2)
        {
            if (ContainsKey(key1) && this[key1].ContainsKey(key2))
            {
                return this[key1][key2];
            }
            return default(T);
        }
    }

    [SerializeField]
    private TwoKeyDictionary<int, int, NavMeshPath> _placePaths = new TwoKeyDictionary<int, int, NavMeshPath>();
    public TwoKeyDictionary<int, int, NavMeshPath> PlacePaths { get { return _placePaths; } }

    //[Header("Test Value")]
    //[SerializeField]
    //private int _unitCount = 0;
    //[SerializeField]
    //private int _placeCount = 0;
    //[SerializeField]
    //private List<GameObject> _placeObjs;

    [Header("Unit Properties")]
    [SerializeField]
    private int _pathLength = 3;
    [SerializeField]
    private float _stayDelay = 5.0f;

    [SerializeField]
    private float _recoverDelay_Mild = 7;
    public float recoverDelay_Mild => _recoverDelay_Mild;
    [SerializeField]
    private float _recoverDelay_Severe = 14;
    public float recoverDelay_Severe => _recoverDelay_Severe;

    //private List<Transform> _buildings;
    //[SerializeField]
    public GameObject _mapObj;

    [Header("Unit State Count")]
    [SerializeField] private int _susceptibleCount = 0;
    [SerializeField] private int _infectiousCount = 0;
    [SerializeField] private int _recoveredCount = 0;

    [Header("Basic Reproductive Number")]
    [SerializeField] public int previousInfectiousCount = 1;
    [SerializeField] private float rNaught = 0f;

    private DistributionRandom _distributionRandom = new DistributionRandom();

    [SerializeField]
    public bool infectByPlace = false;
    [SerializeField]
    public bool enableFuzzyLogic = false;

    private bool _displayMarker = true;


    [Button]
    public void TestEntitySetup()
    {
        //// places
        //foreach (GameObject placeObj in _placeObjs) 
        //    _places.Add(placeObj.GetComponent<PlaceEntity>());

        // Find All Spawn point
        List<Transform> spawnPoints = new List<Transform>();
        for (int i = 1; i < _mapObj.transform.childCount; i++)
        {
            foreach (Transform obj in _mapObj.transform.GetChild(i))
            {
                foreach (Transform child in obj)
                {
                    if (child.CompareTag("SpawnPoint"))
                    {
                        spawnPoints.Add(obj);
                    }
                }
            }
        }

        // units
        for (int i = 0; i < VariableManager.Instance.variables.PopulationNumber; i++)
        {
            UnitEntity unit = Instantiate(_unitPrefab,transform).GetComponent<UnitEntity>();
            unit.GenerateUnitPath(_pathLength, _places.Count, spawnPoints[i % spawnPoints.Count]);
            _units.Add(unit);
        }

        List<int> unitProtections = _distributionRandom.GetDistributionRandom(_units.Count, (int)VariableManager.Instance.variables.PopulationProtection);

        for (int i = 0; i < _units.Count; i++)
            _units[i].SetProtectionValue(unitProtections[i]);

        // first Infectious
        _units[0].SetInfectState((int)UnitEntity.InfState.Infectious);
    }

    public void PlaceEntitySetup() 
    {
        //foreach (GameObject placeObj in _placeObjs)
        //    _places.Add(placeObj.GetComponent<PlaceEntity>());

        // Find All Spawn point
        List<Transform> spawnPoints = new List<Transform>();
        for (int i = 1; i < _mapObj.transform.childCount; i++)
        {
            foreach (Transform obj in _mapObj.transform.GetChild(i))
            {
                foreach (Transform child in obj)
                {
                    if (child.CompareTag("SpawnPoint"))
                    {
                        spawnPoints.Add(obj);
                    }
                }
            }
        }

        for (int i = 0; i < VariableManager.Instance.variables.PopulationNumber; i++)
            _units[i].GenerateUnitPath(_pathLength, _places.Count, spawnPoints[i % spawnPoints.Count]);
    }

    //[Button]
    //public void AutoPlace()
    //{
    //    List<int> randomBuildingIndices = new List<int>();

    //    // Get all building Objs
    //    for (int i = 1; i < _mapObj.transform.childCount; i++)
    //    {
    //        foreach (Transform building in _mapObj.transform.GetChild(i))
    //        {
    //            if (building.gameObject.name[0] == 'E') 
    //                _buildings.Add(building);
    //        }
    //    }

    //    // Random pick marker from buildings
    //    int randomSize = _buildings.Count / _placeCount;
    //    for (int i = 0; i < _placeCount; i++)
    //    {
    //        int randomOffset = Random.Range(1, randomSize);
    //        randomBuildingIndices.Add((i * randomSize) + randomOffset);
    //    }

    //    // Instantiate marker Objs
    //    for (int i = 0; i < _placeCount; i++)
    //    {
    //        GameObject place = Instantiate(_placePrefab, _buildings[randomBuildingIndices[i]].position, transform.rotation, transform);
            
    //        //NavMeshHit navHit;
    //        //if (NavMesh.SamplePosition(place.transform.position, out navHit, 10.0f, NavMesh.AllAreas))
    //        //    transform.position = navHit.position;
    //        //else
    //        //    Debug.Log("No Nav hit found.");

    //        _placeObjs.Add(place);
    //    }
    //}

    [Button]
    public void UnitFirstPath()
    {
        foreach (UnitEntity unit in _units)
            unit.UpdateFirstPath();

        //StartCoroutine(WaitTo_AllUnitFindPath());
    }

    [Button]
    public void ClearUnitEntity()
    {
        foreach (UnitEntity unit in _units)
            unit.gameObject.Destroy();
        
        _units.Clear();
    }


    private void OnEnable()
    {
        //EventManager.Instance.AddListener<UnitEntity.OnInfect>(InfCalculate);
    }

    private void OnDisable()
    {
        //EventManager.Instance.RemoveListener<UnitEntity.OnInfect>(InfCalculate);
    }

    public void AddPlaceRequest() 
    {
        GameObject placeObj = Instantiate(_placePrefab, transform);
        _places.Add(placeObj.GetComponent<PlaceEntity>());
    }

    public void RemovePlaceRequest(GameObject placeObj)
    {
        _places.Remove(placeObj.GetComponent<PlaceEntity>());
        placeObj.Destroy();
    }

    public void ClearPlaceContainer()
    {
        foreach (PlaceEntity place in _places)
            place.ClearContainer();
    }

    public void UpdateStateCount() 
    {
        _susceptibleCount = 0;
        _infectiousCount = 0;
        _recoveredCount = 0;

        foreach (var unit in _units) 
        {
            if (unit.InfectionState == UnitEntity.InfState.Susceptible)
                _susceptibleCount++;
            if (unit.InfectionState == UnitEntity.InfState.Infectious)
                _infectiousCount++;
            if (unit.InfectionState == UnitEntity.InfState.Recovered)
                _recoveredCount++;
        }
    }

    public void GetUnitsStateCount(out int susCount, out int infCount, out int recCount) 
    {
        UpdateStateCount();
        susCount = _susceptibleCount;
        infCount = _infectiousCount;    
        recCount = _recoveredCount;
    }

    [Button]
    private void TestDistributionRandom()
    {
        List<int> unitProtections = _distributionRandom.GetDistributionRandom(VariableManager.Instance.variables.PopulationNumber, (int)VariableManager.Instance.variables.PopulationProtection);
        List<int> visualizer = new List<int>();
        float avg = 0;

        for (int i = 0; i < 10; i++)
        {
            visualizer.Add(0);
        }

        for (int i = 0; i < unitProtections.Count; i++)
        {
            int index = unitProtections[i] / 10;
            visualizer[index] = visualizer[index] + 1;
            avg += unitProtections[i];
        }

        for (int i = 0; i < visualizer.Count; i++)
        {
            string str = i + " - " + (i+1) + ": ";
            for (int j = 0; j < visualizer[i]; j++)
            {
                str += "*";
            }
            Debug.Log(str);
        }

        Debug.Log("AVG: " + avg / unitProtections.Count);

    }

    //IEnumerator WaitTo_AllUnitFindPath()
    //{
    //    bool pass = false;
    //    while (!pass)
    //    {
    //        bool pending = false;
    //        yield return new WaitForSecondsRealtime(1.0f);

    //        foreach (var unit in _units)
    //        {
    //            // still pending -> wait / check again
    //            if (unit.FirstPathPending) pending = true; break;
    //        }

    //        Debug.Log("PathPending: " + pending);
    //        if (!pending) pass = true;
    //    }

    //    foreach (var unit in _units)
    //        unit.StartNavMeshMoving();

    //    yield return null;
    //}

    // called by event-based trigger
    //private void InfCalculate(IEvent e)
    //{
    //    UnitEntity.OnInfect data = e as UnitEntity.OnInfect;
    //    if (data == null) { return; }

    //    Debug.Log("InfCalculate Trigger");

    //    if (Random.value >= 0.5f)
    //        _units[data.id].InfectionState = UnitEntity.InfState.Infectious;
    //}

    

    public void CalculateAllPlacePaths()
    {
        _placePaths.Clear();

        for (int from = 0; from < _places.Count; from++)
        {
            for (int to = 0; to < _places.Count; to++)
            {
                if (from == to) continue;

                NavMeshPath newPath = new NavMeshPath();
                NavMesh.CalculatePath(_places[from].transform.position, _places[to].transform.position, NavMesh.GetAreaFromName("Nav_Walkable"), newPath);
                _placePaths.Add(from, to, newPath);
            }
        }
    }

    public void SetDisplayMarker(bool state)
    {
        if (state == _displayMarker) return;
        _displayMarker = state;
        foreach (PlaceEntity place in _places)
            place.transform.GetChild(0).gameObject.SetActive(state);
    }

    IEnumerator displayStatusCoroutine;

    public void SetDisplayStatus(bool state)
    {
        if (displayStatusCoroutine != null)
            StopCoroutine(displayStatusCoroutine);

        if (state)
        {
            displayStatusCoroutine = DisplayingStatus();
            StartCoroutine(displayStatusCoroutine);
        }    
    }

    public void ResetDisplayStatus()
    {
        foreach (PlaceEntity place in _places)
            place.SetRedMarker(false);
    }

    IEnumerator DisplayingStatus()
    {
        while (true)
        {
            foreach (PlaceEntity place in _places) 
                place.SetRedMarker((place.infectCount > 0));

            yield return new WaitForSecondsRealtime(1.0f);
        }
    }

    //[Header("Nav Path Check")]
    //[SerializeField] int _testFrom = 0;
    //[SerializeField] int _testTo = 1;

    //[Button]
    //public void CheckPathData()
    //{
    //    //UnitEntity unit = Instantiate(_unitPrefab,_places[_testFrom].transform.position,_places[_testFrom].transform.rotation).GetComponent<UnitEntity>();
    //    //unit._navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
    //    //unit._navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    //    //unit._navMeshAgent.speed = 2.5f; // hard code

    //    //unit._navMeshAgent.SetPath(_placePaths.Get(_testFrom, _testTo));
    //}
}
