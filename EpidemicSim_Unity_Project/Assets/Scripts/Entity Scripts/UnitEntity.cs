using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [SerializeField]
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    private InfState _infectionState = InfState.Susceptible;
    public InfState InfectionState { get { return _infectionState; }}

    [SerializeField]
    private MoveState _movementState = MoveState.Stay;
    public MoveState MovementState { get { return _movementState; }}

    [SerializeField]
    private List<int> _unitPath = new List<int>();
    [SerializeField]
    private int _pathCounter = 0;

    [SerializeField]
    private Material _susceptibleMat;
    [SerializeField]
    private Material _infectiousMat;

    Transform _moveTo;

    public void GenerateUnitPath(int pathLength, int placeCount) 
    {
        _unitPath.Clear();
        for (int i = 0; i < pathLength; i++)
        {
            int newPathIndex = Random.Range(0, placeCount);
            if (i > 0 && newPathIndex == _unitPath[i-1])
                newPathIndex = (newPathIndex+1) % placeCount;
            _unitPath.Add(newPathIndex);
        }

        //transform.position = EntityManager.Instance.Places[_unitPath[_pathCounter]].transform.position;
        Vector3 initialPosition = EntityManager.Instance.Places[_unitPath[_pathCounter]].transform.position;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(initialPosition, out navHit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = navHit.position;

            Debug.Log("Initial: " + initialPosition);
            Debug.Log("Nav Hit: " + navHit.position);
        }
        else 
        {
            Debug.Log("No Nav hit found.");
        }

        _navMeshAgent = gameObject.AddComponent<NavMeshAgent>();

    }

    public void UpdateNextPath() 
    {
        _pathCounter = (_pathCounter + 1) % _unitPath.Count;
        _moveTo = EntityManager.Instance.Places[_unitPath[_pathCounter]].transform;
        _movementState = MoveState.Travel;
    }

    private void FixedUpdate()
    {
        if (_movementState == MoveState.Travel)
            _navMeshAgent.destination = _moveTo.position;
    }

}
