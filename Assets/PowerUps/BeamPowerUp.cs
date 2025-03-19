using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPowerUp : PowerUpBase
{

    [SerializeField] ParticleSystem beam;
    [SerializeField] CapsuleCollider2D capsuleCollider2D;

    [SerializeField] float colliderSpeed = 50f;
    ParticleSystem.MainModule main;
    private float colliderWidth;
    private float colliderHeight;

    [SerializeField] float volume = 0.5f;

    

    void Awake()
    {
        capsuleCollider2D = transform.GetComponentInChildren<CapsuleCollider2D>();
        beam = transform.GetComponentInChildren<ParticleSystem>();
        colliderWidth = capsuleCollider2D.size.x;
        colliderHeight = capsuleCollider2D.size.y;

        beam.Stop();
        main = beam.main;
        main.duration = duration;


        capsuleCollider2D.enabled = false;
    }



        protected override void Update()
    {
        base.Update();

        if (cooldownReady)
        {
            cooldownReady = false;

            ActivatePowerUp();
            ExtendCollider();
        }
        else
        {
            CheckCooldown();
        }
    }

    private void ExtendCollider()
    {
        capsuleCollider2D.enabled = true;
        StartCoroutine(DisableCollider());
    }

    private IEnumerator DisableCollider()
    {
        capsuleCollider2D.enabled = true;

        for(float size_Y = 0; size_Y < colliderHeight; size_Y += Time.deltaTime * colliderSpeed)
        {
            capsuleCollider2D.size = new Vector2(colliderWidth, size_Y);
            capsuleCollider2D.offset = new Vector2(0, (size_Y / 2) + 2);
            yield return null;
        }
    }

    private void ActivatePowerUp()
    {
        StartCoroutine(FireBeam());
    }

    private IEnumerator FireBeam()
    {
        beam.Play();

        audioSource.Play();


        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, volume, t / 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(duration - .5f);

        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(volume, 0, t / 0.5f);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = volume;

        capsuleCollider2D.enabled = false;

    }



}
