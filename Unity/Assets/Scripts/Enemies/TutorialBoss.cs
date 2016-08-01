﻿using UnityEngine;
using System.Collections;

public class TutorialBoss : Boss
{
    [SerializeField] private SpriteRenderer bossRenderer = null;

    private bool movingLeft = false;
   

    protected override void Awake()
    {
        base.Awake();
        isTutorialBoss = true;
    }

    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform, bool b = false)
    {
        if (!bossDefeated)
        {
            if (shootCooldown >= shootRate)
            {
                for (int i = 0; i < _shootTransform.Length; i++)
                {
                    Vector3 shootDir = Vector3.down;
                    Projectile p = ProjectileManager.instance.PoolingProjectile(_shootTransform[i].transform);
                    p.SetProjectile(_projData, shootDir);
                    p.transform.position = _shootTransform[i].transform.position;
                    p.gameObject.SetActive(true);
                    shootCooldown = 0;
                    p.GetComponentInChildren<ParticleSystem>().startLifetime = .1f;
                }
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (!bossDefeated)
            {
                base.Update();
            }
        }
    }

    public override void TakeDamage(float _damage)
    {
        if (IsInvoking("revertColour"))
        {
            CancelInvoke("revertColour");
        }
        bossRenderer.color = Color.red;
        Invoke("revertColour", .1f);
        base.TakeDamage(_damage);
    }

    protected override IEnumerator bossDeath()
    {
        Explosion ex;
        ex = ExplosionManager.instance.PoolingExplosion(transform, 2);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        StartCoroutine(base.bossDeath());
        yield return new WaitForSeconds(0.1f);
    }

    protected override void Movement()
    {
        transform.position += Vector3.right * Time.deltaTime * speed * (movingLeft?-1:1);
        if(transform.position.x < -7.0f && movingLeft)
        {
            movingLeft = false;
        }
        else if(transform.position.x > 7.0f && !movingLeft)
        {
            movingLeft = true;
        }
        if(transform.position.y > 5.0f)
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
        }
    }
}