using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls playback of particles connected to movement. Methods invoked by the StateMachine StateActions
/// </summary>
public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem walkingParticles = default;
    [SerializeField] private ParticleSystem landParticles = default;
    [SerializeField] private ParticleSystem jumpParticles = default;

    public void EnableWalkParticles() => walkingParticles.Play();

    public void DisableWalkParticles() => walkingParticles.Stop();

    public void PlayJumpParticles() => jumpParticles.Play();

    public void PlayLandParticles(float intensity)
    {
        // make sure intensity is always between 0 and 1
        intensity = Mathf.Clamp01(intensity);

        ParticleSystem.MainModule main = landParticles.main;
        ParticleSystem.MinMaxCurve
            origCurve = main.startSize; //save original curve to be assigned back to particle system
        ParticleSystem.MinMaxCurve
            newCurve = main.startSize; //Make a new minMax curve and make our changes to the new copy

        float minSize = newCurve.constantMin;
        float maxSize = newCurve.constantMax;

        newCurve.constantMax = Mathf.Lerp(minSize, maxSize, intensity);
        main.startSize = newCurve;

        landParticles.Play();

        StartCoroutine(ResetMinMaxCurve(landParticles, origCurve));
    }

    private IEnumerator ResetMinMaxCurve(ParticleSystem particleSystem, ParticleSystem.MinMaxCurve curve)
    {
        while (particleSystem.isEmitting)
        {
            yield return null;
        }

        ParticleSystem.MainModule main = particleSystem.main;
        main.startSize = curve;
    }
}