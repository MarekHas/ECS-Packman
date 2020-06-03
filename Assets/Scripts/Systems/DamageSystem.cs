using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using System;

public class DamageSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;

        Entities.ForEach((DynamicBuffer<CollisionBufferData> collisions, ref HealthData healthData) =>
        {
            for (int i = 0; i < collisions.Length; i++)
            {
                if (healthData.healthTimer <= 0 && HasComponent<DamageData>(collisions[i].entity))
                {
                    healthData.healthValue -= GetComponent<DamageData>(collisions[i].entity).damgeValue;
                    healthData.healthTimer = 1;
                    AudioManager.Instance.PlaySfx(healthData.damageSfx.ToString());
                }
            }
        }).WithoutBurst().Run();

        Entities
            .WithNone<KillData>()
            .ForEach((Entity entity, ref HealthData healthData) =>
            {
                healthData.healthTimer -= deltaTime;
                if (healthData.healthValue <= 0)
                {
                    AudioManager.Instance.PlaySfx(healthData.deathSfx.ToString());
                    EntityManager.AddComponentData(entity, new KillData() { timer = healthData.deathTimer });
                }

            }).WithStructuralChanges().Run();

        var endSimulationEnityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        var entityCommandBufferSystem = endSimulationEnityCommandBufferSystem.CreateCommandBuffer();

        Entities.ForEach((Entity entity, ref KillData kill, in Translation translation, in Rotation rotation) =>
        {
            kill.timer -= deltaTime;
            if (kill.timer <= 0)
            {
                if (HasComponent<OnKillData>(entity))
                {
                    var onKill = GetComponent<OnKillData>(entity);
                    AudioManager.Instance.PlaySfx(onKill.SfxName.ToString());
                    GameManager.Instance.AddPoints(onKill.PointValue);

                    if (EntityManager.Exists(onKill.PrefabSpawned))
                    {
                        var spawnedEntity = entityCommandBufferSystem.Instantiate(onKill.PrefabSpawned);
                        entityCommandBufferSystem.AddComponent(spawnedEntity, translation);
                        entityCommandBufferSystem.AddComponent(spawnedEntity, rotation);
                    }

                }
                entityCommandBufferSystem.DestroyEntity(entity);
            }
        }).WithoutBurst().Run();
    }
}