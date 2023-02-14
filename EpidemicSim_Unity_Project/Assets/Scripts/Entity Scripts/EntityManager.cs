using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]
    private List<UnitEntity> _units = new List<UnitEntity>();
    [SerializeField]
    private List<PlaceEntity> _places = new List<PlaceEntity>();

    private void OnEnable()
    {
        EventManager.Instance.AddListener<UnitEntity.OnInfect>(InfCalculate);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<UnitEntity.OnInfect>(InfCalculate);
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
    private void InfCalculate(IEvent e)
    {
        UnitEntity.OnInfect data = e as UnitEntity.OnInfect;
        if (data == null) { return; }

        Debug.Log("InfCalculate Trigger");

        if (Random.value >= 0.5f)
            _units[data.id].InfectionState = UnitEntity.InfState.Infectious;
    }
}
