using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject _prefab;

    [Range(10, 400)]
    public int rowLimit = 100;

    [Header("Agents List")]
    [SerializeField] private List<GameObject> agents;

    // Start is called before the first frame update
    void Start()
    {
        agents = new List<GameObject>();

        for (int i = 0; i < rowLimit; i++) 
        {
            for (int j = 0; j < rowLimit; j++) 
            {
                agents.Add(Instantiate(_prefab, new Vector3(i, 1, j) , Quaternion.identity));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var agent in agents) 
        {
            Vector3 agentPos = agent.transform.position;
            agent.transform.position = new Vector3(agentPos.x, Mathf.Abs(Mathf.Sin(Time.time)), agentPos.z);
        }
    }
}
