using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
partial struct PlayerAnimationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }


    public void OnUpdate(ref SystemState state)
    {
        var buffer = new EntityCommandBuffer(Allocator.Temp);
        foreach((PlayerPrefabReference playerPrefab, Entity entity) 
            in SystemAPI.Query<PlayerPrefabReference>().WithNone<PlayerAnimatorReference>()
                .WithEntityAccess())
        {
            var newCompanionPlayerObject = Object.Instantiate(playerPrefab.PlayerPrefab);
            var newAnimatorRef = new PlayerAnimatorReference
            {
                PlayerAnimator = newCompanionPlayerObject.GetComponent<Animator>()
            };

            buffer.AddComponent(entity, newAnimatorRef);
        }

        foreach ((PlayerAnimatorReference animatorReference,
            RefRO<LocalTransform> transform,
            RefRO<NetcodePlayerInput> playerMovement)
            in SystemAPI.Query<PlayerAnimatorReference, RefRO<LocalTransform>, RefRO<NetcodePlayerInput>>())
        {
            animatorReference.PlayerAnimator.transform.position = transform.ValueRO.Position;
            animatorReference.PlayerAnimator.transform.rotation = transform.ValueRO.Rotation;
            animatorReference.PlayerAnimator.SetBool("IsMoving", math.length(playerMovement.ValueRO.moveInput) > 0f);
        }

        foreach ((PlayerAnimatorReference animatorReference, Entity entity)
            in SystemAPI.Query<PlayerAnimatorReference>().WithNone<PlayerPrefabReference, LocalTransform>()
            .WithEntityAccess())
        {
            Object.Destroy(animatorReference.PlayerAnimator.gameObject);
            buffer.RemoveComponent<PlayerAnimatorReference>(entity);
        }

        buffer.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public struct ChangeAnimationState : IRpcCommand
{
    public bool isMoving;
    ChangeAnimationState(bool condition)
    {
        isMoving = condition;
    }
    
}

