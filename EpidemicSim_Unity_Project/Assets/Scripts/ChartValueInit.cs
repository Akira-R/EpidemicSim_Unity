using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using UnityEngine.InputSystem;

public class ChartValueInit : MonoBehaviour
{
    GraphChartBase graph;
    int idx = 30;

    // Start is called before the first frame update
    void Start()
    {
        graph = GetComponent<GraphChart>();
        if (graph != null)
        {
            Debug.Log("Graph is not NULL");
            graph.DataSource.StartBatch();  // start a new update batch
            graph.DataSource.ClearCategory("Susceptible");  // clear the categories we have created in the inspector
            graph.DataSource.ClearCategory("Infectious");
            for (int i = 0; i < 30; i++)
            {
                //add 30 random points , each with a category and an x,y value
                graph.DataSource.AddPointToCategory("Susceptible", i, Random.Range(1f, 50f) * 2f);
                graph.DataSource.AddPointToCategory("Infectious", i, Random.Range(1f, 50f) * 2f);
            }
            graph.DataSource.EndBatch(); // end the update batch . this call will render the graph
        }
    }

    // Update is called once per frame
    void Update()
    {
        graph.DataSource.AddPointToCategory("Susceptible", idx++, Random.Range(1f, 50f) * 2f);
    }
}
