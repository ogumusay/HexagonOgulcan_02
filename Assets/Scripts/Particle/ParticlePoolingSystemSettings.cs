using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexagon.Game
{
    [CreateAssetMenu(menuName = "Hexagon/Game/ParticlePoolingSystemSettings")]
    public class ParticlePoolingSystemSettings : ScriptableObject
    {
        [SerializeField] public int PoolAmount;
        [SerializeField] public ParticleSystem ParticlePrefab;
    }
}