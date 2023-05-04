using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionRandom
{
    private int _minValue = 0;
    private int _maxValue = 100;

    private int _randomPartitionSetNumber = 6;
    private int[] _partitionSetValue = { 3, 16, 50, 84, 97, 100 };
    private int _partitionRandomRange = 10;

    public List<int> GetDistributionRandom(int number, int mean)
    {
        int startRandomValueOffset = mean - (_randomPartitionSetNumber * _partitionRandomRange / 2);
        List<int> valueList = new List<int>();

        for (int i = 0; i < number; i++)
        {
            // partition random value
            int prv = Random.Range(_minValue, _maxValue);

            // check Partition
            int p_index = 0;
            for (int p_counter = 0; p_counter < _randomPartitionSetNumber; p_counter++)
            {
                if (prv < _partitionSetValue[p_counter])
                {
                    p_index = p_counter;
                    break;
                }
            }

            // get random value inside partition range
            int from = startRandomValueOffset + (_partitionRandomRange * p_index);
            int to = startRandomValueOffset + (_partitionRandomRange * (p_index + 1));

            //Debug.Log("p_index: " + p_index);
            //Debug.Log(from + " to " + to);

            valueList.Add(Mathf.Clamp(Random.Range(from, to), _minValue, _maxValue));
        }
        return valueList;
    }
}
