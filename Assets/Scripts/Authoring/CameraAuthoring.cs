using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class CameraAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public AudioListener AudioListener;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddComponentData(entity, new CameraData() { });

        conversionSystem.AddHybridComponent(AudioListener);
    }
}
