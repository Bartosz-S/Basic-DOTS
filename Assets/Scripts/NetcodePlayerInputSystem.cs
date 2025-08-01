using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine.InputSystem;
using UnityEngine;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial class NetcodePlayerInputSystem : SystemBase
{
    InputSystem_Actions input;
    InputSystem_Actions.PlayerActions actions;

    protected override void OnCreate()
    {
        input = new();
        input.Enable();
        actions = input.Player;
        RequireForUpdate<NetcodePlayerInput>();
        RequireForUpdate<NetworkStreamInGame>();
    }

    //[BurstCompile]
    protected override void OnUpdate()
    {
        InputSystem.Update();
        float2 inputVectorMove = actions.Move.ReadValue<Vector2>();
        float2 inputVectorLook = actions.Look.ReadValue<Vector2>();
        foreach (RefRW<NetcodePlayerInput> netcodePlayerInput
            in SystemAPI.Query<RefRW<NetcodePlayerInput>>()
            .WithAll<GhostOwnerIsLocal>())
        {
            netcodePlayerInput.ValueRW.moveInput = new float2(inputVectorMove.x, inputVectorMove.y);
            netcodePlayerInput.ValueRW.lookInput = new float2(inputVectorLook.x, inputVectorLook.y);
        }
    }

    protected override void OnDestroy()
    {
        input.Disable();
    }
}
