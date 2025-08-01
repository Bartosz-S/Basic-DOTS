using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct NewISystemScript : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkId>();
        state.RequireForUpdate<EntitiesReferences>();
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer buffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach ((RefRO<NetworkId> id, Entity entity) 
            in SystemAPI.Query<RefRO<NetworkId>>().WithNone<NetworkStreamInGame>().WithEntityAccess())
        {
            buffer.AddComponent<NetworkStreamInGame>(entity);

            Entity rpcEntity = buffer.CreateEntity();
            buffer.AddComponent(rpcEntity, new GoInGameRPC());
            buffer.AddComponent(rpcEntity, new SendRpcCommandRequest());
            Debug.Log("Connecting to the server");
        }
        buffer.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public struct GoInGameRPC : IRpcCommand
{

}
