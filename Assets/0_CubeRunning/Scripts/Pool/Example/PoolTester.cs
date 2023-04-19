using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTester : MonoBehaviour
{
    [SerializeField] private ParticlePoolSO pool = default;

    private void Start()
    {
        List<ParticleSystem> particles = pool.Request(10) as List<ParticleSystem>;
        foreach (ParticleSystem particle in particles)
        {
            StartCoroutine(DoParticleBehaviour(particle));
        }
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