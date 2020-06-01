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
    }
}