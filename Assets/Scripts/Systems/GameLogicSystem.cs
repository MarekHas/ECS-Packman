using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Collections;

[AlwaysUpdateSystem]
public class GameLogicSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var dotsQuery = GetEntityQuery(ComponentType.ReadOnly<PickupPointsData>());
        var playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerData>());
        var enemyQuery = GetEntityQuery(ComponentType.ReadOnly<EnemyData>());
        var spawnQuery = GetEntityQuery(typeof(SpawnData));

        var dotsCount = dotsQuery.CalculateEntityCount();

        if (playerQuery.CalculateEntityCount() > 0)
        {
            GameManager.Instance.SetDotsCount(dotsCount);
            if (dotsCount == 0)
            {
                Entities
                    .WithAll<PhysicsVelocity>()
                    .ForEach((Entity entity) =>
                    {
                        EntityManager.RemoveComponent<PickupPowerUpData>(entity);
                        EntityManager.RemoveComponent<PhysicsVelocity>(entity);
                    }).WithStructuralChanges().Run();
            }
        }

        Entities
            .WithAll<PlayerData, PhysicsVelocity>()
            .ForEach((Entity entity, in KillData killData) =>
            {
                EntityManager.RemoveComponent<PhysicsVelocity>(entity);
                EntityManager.RemoveComponent<MoveData>(entity);
                GameManager.Instance.SubtractLife();

                if (GameManager.Instance.CurrentLives < 0)
                {
                    var spawnArray = spawnQuery.ToEntityArray(Allocator.TempJob);

                    foreach (Entity spawnEntity in spawnArray)
                    {
                        EntityManager.DestroyEntity(spawnEntity);
                    }

                    spawnArray.Dispose();
                }

                var enemyArray = enemyQuery.ToEntityArray(Allocator.TempJob);

                foreach (Entity enemyEntity in enemyArray)
                {
                    EntityManager.AddComponentData(enemyEntity, killData);
                    EntityManager.RemoveComponent<PhysicsVelocity>(enemyEntity);
                    EntityManager.RemoveComponent<MoveData>(enemyEntity);
                    EntityManager.RemoveComponent<OnKillData>(enemyEntity);
                }

                enemyArray.Dispose();

            }).WithStructuralChanges().Run();
    }
}