using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using NaughtyAttributes;

public class UnitEntity : MonoBehaviour
{
    public enum InfState 
    { 
        Susceptible,
        Infectious,
        Recovered
    }

    public enum MoveState
    {
        Stay,
        Travel
    }

    [SerializeField, ReadOnly]
    private InfState _infectionState = InfState.Susceptible;
    public InfState InfectionState { get { return _infectionState; }}

    [SerializeField, ReadOnly]
    private MoveState _movementState = MoveState.Stay;
    public MoveState MovementState { get { return _movementState; }}

    [SerializeField]
    private List<int> _unitPath = new List<int>();
    [SerializeField, ReadOnly]
    private int _pathCounter = 0;
    [SerializeField, ReadOnly]
    private int _protectionValue;
    public int protectionValue => _protectionValue;
    [SerializeField, ReadOnly]
    private int _exposureTime;
    public int exposureTime => _exposureTime;

    [SerializeField]
    private Material _susceptibleMat;
    [SerializeField]
    private Material _infectiousMat;
    [SerializeField]
    private Material _recoveredMat;

    private NavMeshAgent _navMeshAgent;
    private Renderer _renderer;

    private PlaceEntity _moveToPlace;
    private float _stayDelay = 5.0f;
    private float _stayCounter = 0.0f;

    private IEnumerator _infectCoroutine;

    public void GenerateUnitPath(int pathLength, int placeCount) 
    {
        _unitPath.Clear();
        for (int i = 0; i < pathLength; i++)
        {
            int newPathIndex = Random.Range(0, placeCount);
            if (i > 0 && newPathIndex == _unitPath[i-1])
                newPathIndex = (newPathIndex+1) % placeCount;
            if(i == pathLength - 1)
                while (newPathIndex == _unitPath[0] || newPathIndex == _unitPath[i - 1])
                    newPathIndex = (newPathIndex + 1) % placeCount;
            _unitPath.Add(newPathIndex);
        }

        Vector3 initialPosition = EntityManager.Instance.Places[_unitPath[_pathCounter]].transform.position;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(initialPosition, out navHit, 1.0f, NavMesh.AllAreas))
            transform.position = navHit.position;
        else 
            Debug.LogWarning("Unit initialize: No navmesh found nearby.");

        _navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _navMeshAgent.speed = 2.5f; // hard code

        _renderer = gameObject.GetComponent<Renderer>();

        //_protectionValue = Random.Range(0, 100);

    }

    public void UpdateNextPath() 
    {
        _pathCounter = (_pathCounter + 1) % _unitPath.Count;
        _moveToPlace = EntityManager.Instance.Places[_unitPath[_pathCounter]];
        _movementState = MoveState.Travel;
        _renderer.enabled = true;

        _navMeshAgent.destination = _moveToPlace.transform.position;

        if (_infectCoroutine != null)
        {
            StopCoroutine(_infectCoroutine);
        }
    }

    public void SetInfectState(int stateIndex)
    {
        _infectionState = (InfState)stateIndex;

        if (_infectionState == InfState.Infectious)
            _renderer.material = _infectiousMat;
        else if(_infectionState == InfState.Recovered)
            _renderer.material = _recoveredMat;
    }

    private void FixedUpdate()
    {
        /*if (_movementState == MoveState.Travel)
            _navMeshAgent.destination = _moveToPlace.transform.position;
        else */if (_movementState == MoveState.Stay)
        {
            _stayCounter += Time.deltaTime;
            if (_stayCounter >= _stayDelay)
            {
                _stayCounter = 0;
                _moveToPlace.UnitDepart(this);
                UpdateNextPath();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "PlaceObject") return;
        if (ReferenceEquals(other.transform, _moveToPlace.transform))
        {
            _movementState = MoveState.Stay;
            _renderer.enabled = false;
            _moveToPlace.UnitArrive(this);
            _exposureTime = 0;

            if (EntityManager.Instance.infectByPlace) { return; }
            _infectCoroutine = InfectCoroutine(1.0f / VariableManager.Instance.variables.TransmissionRate);
            StartCoroutine(_infectCoroutine);
        }
    }

    public void IncreaseExposure() 
    {
        _exposureTime++;
    }

    public void SetRecoveryDelay(float time)
    {
        StartCoroutine(WaitTo_EndRecoveryDelay(time));
    }

    public void SetProtectionValue(int value)
    {
        _protectionValue = value;
    }

    IEnumerator WaitTo_EndRecoveryDelay(float time)
    {
        yield return new WaitForSeconds(time);
        SetInfectState((int)InfState.Recovered);

        if (_movementState == MoveState.Stay)
            _moveToPlace.UnitRecoveredUpdate();
    }

    IEnumerator InfectCoroutine(float delayTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);
            Infect();
        }
    }

    private void Infect()
    {

        EntityManager entityManager = EntityManager.Instance;
        int result;

        if (entityManager.Places[_unitPath[_pathCounter]].infectCount <= 0) return;

        if (entityManager.enableFuzzyLogic)
        {
            //Debug.Log("Protection: " + unit.protectionValue + " Exposure: " + unit.exposureTime);
            result = FuzzyCalculator.Instance.GetHighestProb(_protectionValue, _exposureTime);
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
                IncreaseExposure();
                //Debug.Log("non-infect");
                break;
            case 1: // mild-infect
                SetInfectState((int)UnitEntity.InfState.Infectious);
                SetRecoveryDelay(EntityManager.Instance.recoverDelay_Mild);
                entityManager.Places[_unitPath[_pathCounter]].UnitInfectUpdate();
                //Debug.Log("mild-infect");
                break;
            case 2: // severe-infect
                SetInfectState((int)UnitEntity.InfState.Infectious);
                SetRecoveryDelay(EntityManager.Instance.recoverDelay_Severe);
                entityManager.Places[_unitPath[_pathCounter]].UnitInfectUpdate();
                //Debug.Log("severe-infect");
                break;
        }
        
    }
}
