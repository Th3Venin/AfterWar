using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public AudioClip clip;

    AudioSource audio;

    public float range;

    public ParticleSystem[] muzzleFlash;
    public GameObject bullethole;
    public Transform fpsCam;
    // Start is called before the first frame update
    void Start()
    {
        fpsCam = RecursiveFindChild(transform.root, "MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateAudio(AudioClip clip)
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.clip = clip;
    }

    public void PlaySound()
    {
        audio.playOnAwake = false;
        audio.time = 0.3f;
        audio.volume = 0.3f;
        if (audio.isPlaying)
            audio.Stop();
        audio.Play();
    }

    public void Shoot()
    {
        if (transform.root.GetComponent<PlayerStats>().magazine != 0)
        {
            PlaySound();
            foreach (var particle in muzzleFlash)
            {
                particle.Emit(1);
                particle.Play();
            }
        }
    }

}
