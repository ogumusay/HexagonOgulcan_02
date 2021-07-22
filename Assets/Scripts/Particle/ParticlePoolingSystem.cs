using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.GameObjects;

namespace Hexagon.Game
{
    public class ParticlePoolingSystem : MonoBehaviour
    {
        [SerializeField] private ParticlePoolingSystemSettings particlePoolingSystemSettings;
        private List<ParticleSystem> particlePoolList;
        private int index = 0;

        private void OnEnable()
        {
            AbstractSelectableGameObject.OnObjectDestroy += ShowParticleEffect;
        }

        private void Start()
        {
            particlePoolList = new List<ParticleSystem>(particlePoolingSystemSettings.PoolAmount);

            CreatePool();
        }

        private void ShowParticleEffect(AbstractSelectableGameObject gameObject, SelectableGameObjectData data)
        {
            ParticleSystem particleSystem = particlePoolList[index];
            ParticleSystem.MainModule settings = particleSystem.main;
            settings.startColor = data.GetColor(gameObject.Color);

            particleSystem.transform.position = gameObject.transform.position;
            particleSystem.Play();

            if (index >= particlePoolingSystemSettings.PoolAmount - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }

        }

        private void CreatePool()
        {
            for (int i = 0; i < particlePoolingSystemSettings.PoolAmount; i++)
            {
                ParticleSystem particleSystem = Instantiate(particlePoolingSystemSettings.ParticlePrefab, Vector3.zero, Quaternion.identity, this.transform);
                particlePoolList.Add(particleSystem);
            }
        }

        private void OnDisable()
        {
            AbstractSelectableGameObject.OnObjectDestroy -= ShowParticleEffect;
        }
    }
}