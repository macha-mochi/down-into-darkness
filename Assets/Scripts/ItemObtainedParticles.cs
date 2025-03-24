using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObtainedParticles : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material trailMaterial;
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        StartCoroutine(StartParticles());
    }
    public IEnumerator StartParticles()
    {
        ps.Play();
        yield return new WaitForSeconds(8);

        var trailModule = ps.trails;
        trailModule.enabled = true;

        ps.GetComponent<ParticleSystemRenderer>().trailMaterial = trailMaterial;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[30];
        ps.GetParticles(particles);

        var limitModule = ps.limitVelocityOverLifetime;
        limitModule.enabled = false;
        for(int i = 0;i < particles.Length;i++)
        {
            particles[i].velocity *= -8;
        }
        ps.SetParticles(particles);
    }
}
