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

    protected override void OnStartRunning()
    {
        Application.targetFrameRate = 60;
        _random = new Random(1234567890);

        _agentPrefab = GetSingleton<AgentSpawnerData>().Prefab;
        _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

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
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                //var newAgent = EntityManager.Instantiate(_agentPrefab);
                //SetSingleton(new LastSpawnAgent { Agent = newAgent });

                //var newPos = new Translation { Value = new float3(i, 1, j)};
                //EntityManager.SetComponentData(newAgent, newPos);

                var ecb = _ecbSystem.CreateCommandBuffer();

                var newAgent = ecb.Instantiate(_agentPrefab);
                var newPos = new Translation { Value = new float3(i, 1, j) };
                ecb.SetComponent(newAgent, newPos);

                ecb.AppendToBuffer(_agentSpawner, new SpawnedAgentBufferElement { Agent = newAgent });
            }
        }
    }
}
