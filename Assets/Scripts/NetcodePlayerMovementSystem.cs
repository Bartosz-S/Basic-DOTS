using System.Numerics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.NetCode.Hybrid;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct NetcodePlayerMovementSystem : ISystem
{
    //[BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((RefRO<NetcodePlayerInput> input, RefRW<LocalTransform> transform) 
            in SystemAPI.Query<RefRO<NetcodePlayerInput>, RefRW<LocalTransform>>()
            .WithAll<Simulate>()){
            transform.ValueRW.Position = transform.ValueRO.Translate(
                new float3(input.ValueRO.moveInput.x,0,input.ValueRO.moveInput.y) * 10f * SystemAPI.Time.DeltaTime)
                .Position;
            transform.ValueRW.Rotation = transform.ValueRO.Rotate(
                quaternion.EulerZXY(new float3(0, input.ValueRO.lookInput.x, 0)* 5f * SystemAPI.Time.DeltaTime))
                .Rotation;
        }
    }

    //[BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
