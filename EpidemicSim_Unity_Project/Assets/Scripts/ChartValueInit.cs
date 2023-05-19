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
    [SerializeField] private int previousInfectiousCount = 1;
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
            graph.DataSource.ClearCategory("Recovered");
            graph.DataSource.ClearCategory("Infectious");
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
        graph.DataSource.AddPointToCategory("Susceptible", dayCount, 1000);
        graph.DataSource.AddPointToCategory("Recovered", dayCount, recCount + infCount);
        graph.DataSource.AddPointToCategory("Infectious", dayCount, infCount);
        graph.DataSource.EndBatch(); // end the update batch . this call will render the graph

        if (dayCount % 4 == 0 && susCount > 0) 
        {
            CalculateReproductiveNumber(susCount, infCount, out basicReproductiveNumber, out effectiveReproductiveNumber, out heardImmunitythreshold);
        }

        Debug.Log("R0: " + basicReproductiveNumber);
        Debug.Log("R: " + effectiveReproductiveNumber);
        Debug.Log("HIT: " + heardImmunitythreshold);
    }

    public void CalculateReproductiveNumber(int suscount, int infCount, out float R0, out float R, out float HIT)
    {
        Debug.Log("InfectiousCount: " + infCount);
        Debug.Log("previousInfectiousCount: " + previousInfectiousCount);

        float rNaught = ((float)infCount / (float)previousInfectiousCount);
        //Debug.Log("R0: " + rNaught);

        previousInfectiousCount = infCount;

        R0 = rNaught;
        R = R0 * (float)(suscount / 1000f);
        HIT = 1f - (float)(1f / R0);
    }
}
