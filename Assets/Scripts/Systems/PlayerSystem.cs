using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var xAxis = Input.GetAxis("Horizontal");
        var yAxis = Input.GetAxis("Vertical");
        var delataTime = Time.DeltaTime;


        Entities
            .WithAll<PlayerData>()
            .ForEach((ref MoveData move) =>
            {
                move.movingDirection = new float3(xAxis, 0, yAxis);
            }).Schedule();

        var entityComponentBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

        Entities
            .WithAll<PlayerData>()
            .ForEach((Entity entity, ref HealthData health, ref PickupPowerUpData powerPill, ref DamageData damge) =>
            {
                damge.damgeValue = 100;
                powerPill.powerUpTimer -= delataTime;
                health.healthTimer = powerPill.powerUpTimer;
              
                if (powerPill.powerUpTimer <= 0)
                {
                    
                    entityComponentBuffer.RemoveComponent<PickupPowerUpData>(entity);
                    damge.damgeValue = 0;
                }
            }).WithoutBurst().Run();
    }
}