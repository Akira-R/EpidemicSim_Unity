using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class EntityManager : MonoSingleton<EntityManager>
{
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
    [SerializeField]
    private int _unitCount = 0;
    [SerializeField]
    private List<GameObject> _placeObjs;

    [Header("Unit Properties")]
    [SerializeField]
    private float _moveSpeed = 0;
    [SerializeField]
    private int _pathLength = 0;
    [SerializeField]
    private float _stayDelay = 0;


    [Button]
    public void TestEntitySetup()
    {
        // places
        foreach (GameObject placeObj in _placeObjs)
            _places.Add(placeObj.GetComponent<PlaceEntity>());

        // units
        for (int i = 0; i < _unitCount; i++)
        {
            UnitEntity unit = Instantiate(_unitPrefab,transform).GetComponent<UnitEntity>();
            unit.GenerateUnitPath(_pathLength, _places.Count);
            _units.Add(unit);
        }

        // first Infectious
        _units[0].SetInfectState((int)UnitEntity.InfState.Infectious);

        TestNextPath();
    }

    [Button]
    public void TestNextPath()
    {
        foreach (UnitEntity unit in _units)
            unit.UpdateNextPath();
    }


    private void OnEnable()
    {
        //EventManager.Instance.AddListener<UnitEntity.OnInfect>(InfCalculate);
    }

    private void OnDisable()
    {
        //EventManager.Instance.RemoveListener<UnitEntity.OnInfect>(InfCalculate);
    }

    private void Add() 
    {
        
    }

    private void Delete()
    {

    }

    private void Clear()
    {

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
