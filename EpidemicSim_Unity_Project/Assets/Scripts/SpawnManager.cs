using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject _prefab;

    [Range(10, 100)]
    public int rowLimit = 100;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rowLimit; i++) 
        {
            for (int j = 0; j < rowLimit; j++) 
            {
                Instantiate(_prefab, new Vector3(i, 0, j) , Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
