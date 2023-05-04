using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class PlaceEntity : MonoBehaviour
{
    [SerializeField]
    private List<UnitEntity> _placeContainer = new List<UnitEntity>();

    //[SerializeField]
    //private float _infectDelay = 1.0f;
    private float _infectDelayCounter = 0.0f;

    //[SerializeField]
    //private int _trafficCount = 0;
    [SerializeField]
    private int _infectCount = 0;

    public int infectCount
    {
        get { return _infectCount; }
        set { _infectCount = value; }
    }

    public void UnitArrive(UnitEntity unit)
    {
        _placeContainer.Add(unit);
        if (unit.InfectionState == UnitEntity.InfState.Infectious)
            _infectCount++;
    }

    public void UnitDepart(UnitEntity unit)
    {
        _placeContainer.Remove(unit);
        if (unit.InfectionState == UnitEntity.InfState.Infectious)
            _infectCount--;
    }

    private void FixedUpdate()
    {
        if (!EntityManager.Instance.infectByPlace) { return; }

        _infectDelayCounter += Time.deltaTime;
        if (VariableManager.Instance.variables.TransmissionRate <= 0) return;
        if (_infectDelayCounter >= 1.0f / VariableManager.Instance.variables.TransmissionRate)
        {
            _infectDelayCounter = 0;
            Infect();
        }
    }

    private void Infect()
    {
        if (_infectCount <= 0) return;

        foreach (UnitEntity unit in _placeContainer)
        {
            if (unit.InfectionState == UnitEntity.InfState.Infectious) continue;
            int result;
            
            if (EntityManager.Instance.enableFuzzyLogic)
            {
                //Debug.Log("Protection: " + unit.protectionValue + " Exposure: " + unit.exposureTime);
                result = FuzzyCalculator.Instance.GetHighestProb(unit.protectionValue, unit.exposureTime);
                //Debug.Log(result);
            }
            else 
            {
                result = Random.Range(0, 2); // 0 or 1
                if (result > 0)
                    result += Random.Range(0, 2); // 0 or 1
            }

            switch (result)
            {
                case 0: // non-infect
                    unit.IncreaseExposure();
                    //Debug.Log("non-infect");
                    break;
                case 1: // mild-infect
                    unit.SetInfectState((int)UnitEntity.InfState.Infectious);
                    unit.SetRecoveryDelay(EntityManager.Instance.recoverDelay_Mild);
                    _infectCount++;
                    //Debug.Log("mild-infect");
                    break;
                case 2: // severe-infect
                    unit.SetInfectState((int)UnitEntity.InfState.Infectious);
                    unit.SetRecoveryDelay(EntityManager.Instance.recoverDelay_Severe);
                    _infectCount++;
                    //Debug.Log("severe-infect");
                    break;
            }
        }
    }

    public void SetNavMeshActive(bool state)
    {
        GetComponent<NavMeshAgent>().enabled = state;
    }

    public void UnitRecoveredUpdate()
    {
        _infectCount--;
    }

    public void UnitInfectUpdate()
    {
        _infectCount++;
    }
}
