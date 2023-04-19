using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LocalPoolTester : MonoBehaviour
{
    [SerializeField] private int initialPoolSize = 5;

    private ParticlePoolSO pool;
    private ParticleFactorySO factory;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private IEnumerator Start()
    {
        factory = ScriptableObject.CreateInstance<ParticleFactorySO>();
        pool = ScriptableObject.CreateInstance<ParticlePoolSO>();
        pool.name = gameObject.name;
        pool.Factory = factory;
        pool.SetParent(this.transform);
        pool.Prewarm(initialPoolSize);
        List<ParticleSystem> particles = pool.Request(2) as List<ParticleSystem>;
        foreach (ParticleSystem particle in particles)
        {
            StartCoroutine(DoParticleBehaviour(particle));
        }

        yield return new WaitForSeconds(2);
        pool.SetParent(null);
        yield return new WaitForSeconds(2);
        pool.SetParent(this.transform);
    }

    private IEnumerator DoParticleBehaviour(ParticleSystem particle)
    {
        particle.transform.position = Random.insideUnitSphere * 5f;
        particle.Play();
        yield return new WaitForSeconds(particle.main.duration);
        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        yield return new WaitUntil(() => particle.particleCount == 0);
        pool.Return(particle);
    }
}