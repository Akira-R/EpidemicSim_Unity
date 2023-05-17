using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using UnityEngine.InputSystem;

public class ChartValueInit : MonoBehaviour
{  
    GraphChartBase graph;
    int idx = 30;

    private int susCount = 0, infCount = 0, recCount = 0;

    [Header("SIR Related")]
    [SerializeField] private float basicReproductiveNumber = 0f;
    [SerializeField] private float effectiveReproductiveNumber = 0f;
    [SerializeField] private float heardImmunitythreshold = 0f;

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
            graph.DataSource.ClearCategory("Predicted Infected");
            graph.DataSource.EndBatch(); // end the update batch . this call will render the graph
        }

        //StartCoroutine(UpdateGraph(0.5f));
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private IEnumerator UpdateGraph(float delay) 
    {
        while (true) 
        {
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
        EntityManager.Instance.GetUnitsStateCount(out susCount, out infCount, out recCount);
        graph.DataSource.StartBatch();  // start a new update batch
        graph.DataSource.AddPointToCategory("Susceptible", dayCount, susCount);
        graph.DataSource.AddPointToCategory("Infectious", dayCount, infCount);
        graph.DataSource.AddPointToCategory("Recovered", dayCount, recCount);
        graph.DataSource.EndBatch(); // end the update batch . this call will render the graph

        if (dayCount % 4 == 0 && susCount > 0) 
            EntityManager.Instance.CalculateReproductiveNumber(susCount, infCount, out basicReproductiveNumber, out effectiveReproductiveNumber, out heardImmunitythreshold);

        Debug.Log("R0: " + basicReproductiveNumber);
        Debug.Log("R: " + effectiveReproductiveNumber);
        Debug.Log("HIT: " + heardImmunitythreshold);
    }
}
