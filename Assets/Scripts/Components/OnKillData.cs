using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Collections;

public struct OnKillData : IComponentData
{
    public NativeString64 sfxName;
    public Entity spawnPrefab;
    public int pointValue;
}
