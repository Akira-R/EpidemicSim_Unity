using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial class AgentSpawnerSystem : SystemBase
{
    private Entity _agentPrefab;
    private Entity _agentSpawner;
    private Random _random;
    private BeginSimulationEntityCommandBufferSystem _ecbSystem;

    // Playable limit at 200 (40000 entities total)
    private int perRowColumn = 100;

    protected override void OnStartRunning()
    {
        _random = new Random(1234567890);

        // Get an instance of prefab and create Entity Buffer
        _agentPrefab = GetSingleton<AgentSpawnerData>().Prefab;
        _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

        // Add buffer to the spawner
        _agentSpawner = GetSingletonEntity<LastSpawnAgent>();
        EntityManager.AddBuffer<SpawnedAgentBufferElement>(_agentSpawner);

        SpawnAgents();
    }
    protected override void OnUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.E)) 
        //{
        //    var newAgent = EntityManager.Instantiate(_agentPrefab);

        //    SetSingleton(new LastSpawnAgent { Agent = newAgent });
        //}

        //var lastSpawned = GetSingleton<LastSpawnAgent>().Agent;
        //if (lastSpawned != Entity.Null) 
        //{
        //    var lastPos = GetComponent<Translation>(lastSpawned);
        //    var upPos = new float3(25, 25, 25);
        //    Debug.DrawLine(lastPos.Value, upPos, Color.green);
        //}
    }

    private void SpawnAgents() 
    {
        var ecb = _ecbSystem.CreateCommandBuffer();
        for (int i = 0; i < perRowColumn; i++)
        {
            for (int j = 0; j < perRowColumn; j++)
            {
                var newAgent = ecb.Instantiate(_agentPrefab);
                var newPos = new Translation { Value = new float3(i, 1, j) };
                ecb.SetComponent(newAgent, newPos);

                ecb.AppendToBuffer(_agentSpawner, new SpawnedAgentBufferElement { Agent = newAgent });
            }
        }
    }
}
