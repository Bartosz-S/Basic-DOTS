using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct ClientISystemScript : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Entity newRPC = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(newRPC, new TestRPC(11));
            state.EntityManager.AddComponentData(newRPC, new SendRpcCommandRequest());
            Debug.Log("Sending RPC...");
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
