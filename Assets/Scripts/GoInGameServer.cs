using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct GoInGameServer : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
        state.RequireForUpdate<NetworkId>();
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences references = SystemAPI.GetSingleton<EntitiesReferences>();
        EntityCommandBuffer buffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach ((RefRO<ReceiveRpcCommandRequest> rpcRequest, Entity entity)
            in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRPC>().WithEntityAccess())
        {
            Debug.Log("Client connected to the server!");
            buffer.AddComponent<NetworkStreamInGame>(rpcRequest.ValueRO.SourceConnection);

            buffer.DestroyEntity(entity);

            Entity playerEntity = buffer.Instantiate(references.playerPrefabEntity);
            
            buffer.SetComponent(playerEntity, LocalTransform.FromPosition(
                new float3(Random.Range(-5,5),0,0)
            ));
            buffer.AddComponent<GhostOwner>(playerEntity, new GhostOwner
            {
                NetworkId = SystemAPI.GetComponent<NetworkId>(rpcRequest.ValueRO.SourceConnection).Value
            });

            buffer.AppendToBuffer(rpcRequest.ValueRO.SourceConnection, new LinkedEntityGroup
            {
                Value = playerEntity
            });

        }
        buffer.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
