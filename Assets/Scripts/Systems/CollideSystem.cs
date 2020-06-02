using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Jobs;

public class CollideSystem : SystemBase
{
    private struct CollisionSystemJob : ICollisionEventsJob
    {
        public BufferFromEntity<CollisionBufferData> collisions;

        public void Execute(CollisionEvent collisionEvent)
        {
            if (collisions.Exists(collisionEvent.Entities.EntityA))
                collisions[collisionEvent.Entities.EntityA].Add(new CollisionBufferData()
                { entity = collisionEvent.Entities.EntityB });
            if (collisions.Exists(collisionEvent.Entities.EntityB))
                collisions[collisionEvent.Entities.EntityB].Add(new CollisionBufferData()
                { entity = collisionEvent.Entities.EntityA });
        }
    }

    private struct TriggerSystemJob : ITriggerEventsJob
    {
        public BufferFromEntity<TriggerBufferData> triggers;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (triggers.Exists(triggerEvent.Entities.EntityA))
                triggers[triggerEvent.Entities.EntityA].Add(new TriggerBufferData()
                { entity = triggerEvent.Entities.EntityB });
            if (triggers.Exists(triggerEvent.Entities.EntityB))
                triggers[triggerEvent.Entities.EntityB].Add(new TriggerBufferData()
                { entity = triggerEvent.Entities.EntityA });
        }
    }



    protected override void OnUpdate()
    {
        var physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld;
        var simulation = World.GetOrCreateSystem<StepPhysicsWorld>().Simulation;

        Entities.ForEach((DynamicBuffer<CollisionBufferData> collisions) =>
        {
            collisions.Clear();
        }).Run();

        var collisionsJobHandle = new CollisionSystemJob()
        {
            collisions = GetBufferFromEntity<CollisionBufferData>()
        }
            .Schedule(simulation, ref physicsWorld, this.Dependency);

        collisionsJobHandle.Complete();

        Entities.ForEach((DynamicBuffer<TriggerBufferData> triggers) =>
        {
            triggers.Clear();
        }).Run();

        var triggersJobHandler = new TriggerSystemJob()
        {
            triggers = GetBufferFromEntity<TriggerBufferData>()
        }
            .Schedule(simulation, ref physicsWorld, this.Dependency);

        triggersJobHandler.Complete();

    }
}