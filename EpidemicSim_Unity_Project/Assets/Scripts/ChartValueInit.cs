using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using UnityEngine.InputSystem;

public class ChartValueInit : MonoBehaviour
{
    
    GraphChartBase graph;
    int idx = 30;

    private int susCount, infCount, recCount;

    // Start is called before the first frame update
    void Start()
    {
        graph = GetComponent<GraphChart>();
        if (graph != null)
        {
            //Debug.Log("Graph is not NULL");
            graph.DataSource.StartBatch();  // start a new update batch
            graph.DataSource.ClearCategory("Susceptible");  // clear the categories we have created in the inspector
            graph.DataSource.ClearCategory("Infectious");
            graph.DataSource.ClearCategory("Recovered");
            graph.DataSource.EndBatch(); // end the update batch . this call will render the graph
        }

        //StartCoroutine(UpdateGraph(0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        EntityManager.Instance.GetUnitsStateCount(out susCount, out infCount, out recCount);
    }

    private IEnumerator UpdateGraph(float delay) 
    {
        while (true) 
        {
            //yield return new WaitForSeconds(delay);

            //graph.DataSource.StartBatch();  // start a new update batch
            //graph.DataSource.AddPointToCategory("Susceptible", idx++, Random.Range(1f, 50f) * 2f);
            //graph.DataSource.AddPointToCategory("Infectious", idx++, Random.Range(1f, 50f) * 2f);
            //graph.DataSource.AddPointToCategory("Recovered", idx++, Random.Range(1f, 50f) * 2f);
            //graph.DataSource.EndBatch(); // end the update batch . this call will render the graph

            yield return new WaitForSeconds(delay);

            graph.DataSource.StartBatch();  // start a new update batch
            graph.DataSource.AddPointToCategory("Susceptible", idx++, susCount);
            graph.DataSource.AddPointToCategory("Infectious", idx++, infCount);
            graph.DataSource.AddPointToCategory("Recovered", idx++, recCount);
            graph.DataSource.EndBatch(); // end the update batch . this call will render the graph
        }
    }
    public void UpdateGraphData(int dayCount) 
    {
        graph.DataSource.StartBatch();  // start a new update batch
        graph.DataSource.AddPointToCategory("Susceptible", dayCount, susCount);
        graph.DataSource.AddPointToCategory("Infectious", dayCount, infCount);
        graph.DataSource.AddPointToCategory("Recovered", dayCount, recCount);
        graph.DataSource.EndBatch(); // end the update batch . this call will render the graph
    }
}
