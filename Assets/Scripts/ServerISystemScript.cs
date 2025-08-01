using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct ServerISystemScript : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer buffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach ((RefRO<TestRPC> rpc, RefRO<ReceiveRpcCommandRequest> request, Entity entity) 
            in SystemAPI.Query<RefRO<TestRPC>, RefRO<ReceiveRpcCommandRequest>>().WithEntityAccess()){
            Debug.Log("RPC received: " + rpc.ValueRO.value + " :: " + request.ValueRO.SourceConnection);
            buffer.DestroyEntity(entity);
        }
        buffer.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
