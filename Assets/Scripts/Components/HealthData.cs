using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Collections;
[GenerateAuthoringComponent]
public struct HealthData : IComponentData
{
    public float healthValue, healthTimer, deathTimer;
    public NativeString64 damageSfx, deathSfx;
}
