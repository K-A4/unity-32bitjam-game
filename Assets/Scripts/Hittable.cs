using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{
    public UnityEvent<int> OnGetHit;
    /// <summary>
    /// 1 float - health change value
    /// 2 float - current health
    /// </summary>
    public UnityEvent<float, float> OnHealthChange;
    public UnityEvent OnDie;

    public float MaxHealth => maxHealth;
    public float Health => health;

    public AudioSource HitSound { get { return hitSound; } set { hitSound = value; } }
    
    public ParticleSystem Particles { get { return hitParticles; } set { hitParticles = value; } }

    [SerializeField] protected Animator animator;

    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth = 20;
    [SerializeField] protected ParticleSystem hitParticles;
    [SerializeField] protected GameObject corpseItem;
    [SerializeField] protected AudioSource hitSound;
    
    public virtual void GetHit(float damage, Vector3 pos)
    {
        OnGetHit?.Invoke(Mathf.RoundToInt(health));
        OnHealthChange?.Invoke(-damage, health);
        health -= damage;
        if (health <= 0)
        {
            Die(pos);
        }

        if (hitSound)
        {
            hitSound.Play();
        }
        PlayHitParticles(pos);
    }

    public void AddHealth(float count)
    {
        OnHealthChange?.Invoke(count, health);
        health = Mathf.Clamp(health + count, 0, maxHealth);
    }

    protected void PlayHitParticles(Vector3 pos)
    {
        if (hitParticles)
        {
            hitParticles.transform.position = pos;
            hitParticles.Play();
        }
    }

    public virtual void Die(Vector3 pos)
    {
        //if (gameObject.layer == LayerMask.NameToLayer("Corpse"))
        //{
        //    //hitParticles.transform.SetParent(null);
        //    //var ps = hitParticles.main;
        //    //ps.stopAction = ParticleSystemStopAction.Destroy;
        //    if (corpseItem)
        //    {
        //        Instantiate(corpseItem, transform.position , Quaternion.identity);
        //    }
        //    OnDie?.Invoke();
        //    Destroy(gameObject);
        //}
        OnDie?.Invoke();
        //Destroy(gameObject);
        PlayHitParticles(pos);
        animator?.SetTrigger("Lay");
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        if (hitParticles)
        {
            CreateParticle();
        }
    }
    
    private void CreateParticle()
    {
        var particles = Instantiate(hitParticles, transform.position, Quaternion.identity);
        var hitsound = particles.gameObject.AddComponent<AudioSource>();
        hitsound.clip = hitSound.clip;
        var ps = particles.main;
        ps.stopAction = ParticleSystemStopAction.Destroy;
        particles.Play();
        hitsound.Play();
    }
}
