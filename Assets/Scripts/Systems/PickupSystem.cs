using UnityEngine;
using System.Collections;
using Unity.Entities;

public class PickupSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var entityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

        Entities
            .WithAll<PlayerData>()
            .ForEach((Entity playerEntity, DynamicBuffer<TriggerBufferData> triggerBuffer) =>
            {
                for (int i = 0; i < triggerBuffer.Length; i++)
                {
                    var entity = triggerBuffer[i].entity;
                    if (HasComponent<PointsData>(entity) && !HasComponent<KillData>(entity))
                    {
                        entityCommandBuffer.AddComponent(entity, new KillData() { timer = 0 });
                        GameManager.Instance.AddPoints(GetComponent<PointsData>(entity).points);
                    }

                    if (HasComponent<PickupPowerUpData>(entity) && !HasComponent<KillData>(entity))
                    {
                        entityCommandBuffer.AddComponent(playerEntity, GetComponent<PickupPowerUpData>(entity));
                        entityCommandBuffer.AddComponent(entity, new KillData() { timer = 0 });
                    }
                }
            }).WithoutBurst().Run();
    }
}
