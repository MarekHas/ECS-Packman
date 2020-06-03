using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Transforms;

public class SpawnSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref SpawnData spawnData, in Translation translation, in Rotation rotation) =>
        {
            if (!EntityManager.Exists(spawnData.ObjectSpawned))
            {
                spawnData.ObjectSpawned = EntityManager.Instantiate(spawnData.PrefabSpawned);
                EntityManager.SetComponentData(spawnData.ObjectSpawned, translation);
                EntityManager.SetComponentData(spawnData.ObjectSpawned, rotation);
            }

        }).WithStructuralChanges().Run();
    }
}