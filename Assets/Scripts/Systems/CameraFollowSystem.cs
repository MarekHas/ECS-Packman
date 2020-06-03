using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class CameraFollowSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var playerQuery = GetEntityQuery(typeof(PlayerData), typeof(MoveData), typeof(Translation));

        if (playerQuery.CalculateEntityCount() == 0)
            return;

        var playerTranslation = GetComponent<Translation>(playerQuery.GetSingletonEntity());
        var minDistance = float.MaxValue;

        var cameraQuery = GetEntityQuery(typeof(CameraData), typeof(FollowData));

        if (cameraQuery.CalculateEntityCount() == 0)
            return;

        var cameraEntity = cameraQuery.GetSingletonEntity();
        var cameraFollow = GetComponent<FollowData>(cameraEntity);

        Entities
            .WithAll<CameraPointData>()
            .ForEach((Entity e, in Translation transalation) =>
            {
                var currentDistance = math.distance(transalation.Value, playerTranslation.Value);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    cameraFollow.TargetToFollow = e;
                    SetComponent(cameraEntity, cameraFollow);
                }

            }).Run();
    }
}
