using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimationAuthoring : MonoBehaviour
{
    public GameObject PlayerPrefab1;
    public GameObject PlayerPrefab2;

    public class Baker : Baker<PlayerAnimationAuthoring>
    {
        public override void Bake(PlayerAnimationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new PlayerPrefabReference
            {
                PlayerPrefab = authoring.PlayerPrefab1
            });
        }
    }
}

public class PlayerPrefabReference : IComponentData
{
    public GameObject PlayerPrefab;
}

public class PlayerAnimatorReference : ICleanupComponentData
{
    public Animator PlayerAnimator;
}
