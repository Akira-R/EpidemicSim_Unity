using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using UnityEngine.InputSystem;
using TMPro;
using NaughtyAttributes;
using System;

public class ChartValueInit : MonoBehaviour
{
    GraphChartBase graph;
    int idx = 30;

    private int susCount = 0, infCount = 0, recCount = 0;

    public bool calEveryTimestep = false;

    [BoxGroup("Display Text")] public TMP_Text R0_Text;
    [BoxGroup("Display Text")] public TMP_Text R_Text;
    [BoxGroup("Display Text")] public TMP_Text HIT_Text;
    
    [BoxGroup("SIR Variables")]
    [SerializeField] private int previousInfectiousCount = 1;
    [BoxGroup("SIR Variables")]
    [SerializeField] private float basicReproductiveNumber = 0f;
    [BoxGroup("SIR Variables")]
    [SerializeField] private float effectiveReproductiveNumber = 0f;
    [BoxGroup("SIR Variables")]
    [SerializeField] private float herdImmunitythreshold = 0f;
    [BoxGroup("SIR Variables")]
    [SerializeField] private float totalBRNAverage = 0f;

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

        if (dayCount % 2 == 0 || calEveryTimestep) 
        {
            CalculateReproductiveNumber(susCount, infCount);
        }

        totalBRNAverage += basicReproductiveNumber;

        //Debug.Log("R0: " + basicReproductiveNumber);
        //Debug.Log("Avg.R0: " + totalBRNAverage / dayCount);
        //Debug.Log("R: " + effectiveReproductiveNumber);
        //Debug.Log("HIT: " + herdImmunitythreshold);

        R0_Text.text = "R0: " + Math.Round(basicReproductiveNumber, 2);
        R_Text.text = "R: " + Math.Round(effectiveReproductiveNumber, 2);
        HIT_Text.text = "HIT: " + Math.Round(herdImmunitythreshold * 100, 2) + "%";
    }

    public void CalculateReproductiveNumber(int suscount, int infCount)
    {
        //Debug.Log("InfectiousCount: " + infCount);
        //Debug.Log("previousInfectiousCount: " + previousInfectiousCount);

        float rNaught = ((float)infCount / (float)previousInfectiousCount);
        //Debug.Log("R0: " + rNaught);

        previousInfectiousCount = infCount;

        basicReproductiveNumber = rNaught;
        effectiveReproductiveNumber = basicReproductiveNumber * (float)(1f - (suscount / 1000f));
        herdImmunitythreshold = 1f - (float)(1f / basicReproductiveNumber);

        if (herdImmunitythreshold <= 0f)
            herdImmunitythreshold = 0f;
    }
}
