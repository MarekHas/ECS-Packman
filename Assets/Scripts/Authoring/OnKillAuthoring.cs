using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class OnKillAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public string SfxName;
    public GameObject prefabSpawned;
    public int PointValue;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddComponentData(entity, new OnKillData()
        {
            SfxName = new Unity.Collections.NativeString64(SfxName),
            PointValue = PointValue,
            PrefabSpawned = conversionSystem.GetPrimaryEntity(prefabSpawned)
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefabSpawned);
    }
}