using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    public enum InfState 
    { 
        Susceptible,
        Infectious,
        Recovered
    }

    public class OnInfect : IEvent {
        public int id;
    }

    [SerializeField]
    private InfState _infectionState = InfState.Susceptible;

    public InfState InfectionState { get { return _infectionState; } set { _infectionState = value; } }

    [SerializeField]
    private List<Transform> _unitPath = new List<Transform>();
    [SerializeField]
    private int _pathCounter = 0;

    private void Move() 
    {
        float distance = Vector3.Distance(_unitPath[_pathCounter].position, this.transform.position);
        // move long the path
    }

    //private void Infect()
    public void Infect()
    {
        // dispatch event
        EventManager.Instance.Dispatch(new OnInfect() { id = 0 });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Infect();
    }
}
