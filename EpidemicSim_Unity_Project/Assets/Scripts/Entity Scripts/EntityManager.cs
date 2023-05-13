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

    [Header("Test Value")]
    //[SerializeField]
    //private int _unitCount = 0;
    //[SerializeField]
    //private int _placeCount = 0;
    [SerializeField]
    private List<GameObject> _placeObjs;

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
    //private GameObject _mapObj;

    [Header("Unit State Count")]
    [SerializeField] private int _susceptibleCount = 0;
    [SerializeField] private int _infectiousCount = 0;
    [SerializeField] private int _recoveredCount = 0;

    [Header("Basic Reproductive Number")]
    [SerializeField] private int previousInfectiousCount = 0;
    [SerializeField] private float rNaught = 0f;

    private DistributionRandom _distributionRandom = new DistributionRandom();

    [SerializeField]
    public bool infectByPlace = false;
    [SerializeField]
    public bool enableFuzzyLogic = false;

    [Button]
    public void TestEntitySetup()
    {
        // places
        foreach (GameObject placeObj in _placeObjs) 
            _places.Add(placeObj.GetComponent<PlaceEntity>());

        // units
        for (int i = 0; i < VariableManager.Instance.variables.PopulationNumber; i++)
        {
            UnitEntity unit = Instantiate(_unitPrefab,transform).GetComponent<UnitEntity>();
            unit.GenerateUnitPath(_pathLength, _places.Count);
            _units.Add(unit);
        }

        // first Infectious
        _units[0].SetInfectState((int)UnitEntity.InfState.Infectious);
    }

    public void PlaceEntitySetup() 
    {
        foreach (GameObject placeObj in _placeObjs)
            _places.Add(placeObj.GetComponent<PlaceEntity>());

        foreach (UnitEntity unit in _units)
            unit.GenerateUnitPath(_pathLength, _places.Count);

        List<int> unitProtections = _distributionRandom.GetDistributionRandom(_units.Count, (int)VariableManager.Instance.variables.PopulationProtection);
        for (int i = 0; i < _units.Count; i++)
            _units[i].SetProtectionValue(unitProtections[i]);
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
    public void TestNextPath()
    {
        foreach (UnitEntity unit in _units)
            unit.UpdateNextPath();
    }

    [Button]
    public void ClearEntity()
    {
        foreach (UnitEntity unit in _units)
            unit.gameObject.Destroy();
        
        _units.Clear();
        _places.Clear();
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
        _placeObjs.Add(placeObj);

        ResetPlaceRef();
    }

    public void RemovePlaceRequest(GameObject placeObj)
    {
        _placeObjs.Remove(placeObj);
        placeObj.Destroy();

        ResetPlaceRef();   
    }

    private void ResetPlaceRef()
    {
        _places.Clear();
        EventManager.Instance.Dispatch<OnPlaceModified>();
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
    public void CalculateBasicReproductiveNumber(int infCount, out float r0) 
    {
        Debug.Log("InfectiousCount: " + infCount);
        Debug.Log("previousInfectiousCount: " + previousInfectiousCount);

        if (previousInfectiousCount <= 0) 
        {
            previousInfectiousCount = infCount;
            r0 = 0f;
            return;
        }

        rNaught = ((float)infCount / (float)previousInfectiousCount);
        Debug.Log("R0: " + rNaught);

        previousInfectiousCount = infCount;
        r0 = rNaught;
    }

    [Button]
    private void TestDistributionRandom()
    {
        List<int> unitProtections = _distributionRandom.GetDistributionRandom(VariableManager.Instance.variables.PopulationNumber, (int)VariableManager.Instance.variables.PopulationProtection);
        float avg = 0;

        for (int i = 0; i < _units.Count; i++)
        {
            Debug.Log(unitProtections[i]);
            avg += unitProtections[i];
        }

        Debug.Log("AVG: " + avg / _units.Count);
        
    }

    // called by event-based trigger
    //private void InfCalculate(IEvent e)
    //{
    //    UnitEntity.OnInfect data = e as UnitEntity.OnInfect;
    //    if (data == null) { return; }

    //    Debug.Log("InfCalculate Trigger");

    //    if (Random.value >= 0.5f)
    //        _units[data.id].InfectionState = UnitEntity.InfState.Infectious;
    //}
}
