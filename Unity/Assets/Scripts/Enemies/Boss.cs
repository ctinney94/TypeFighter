﻿using UnityEngine;
using System.Collections;

public class Boss : Enemy 
{
    public Transform mouthShot;
    public Vector2 bossTarget = new Vector2(0,9.3f);
    public Transform[] eyeShot;
    public Enemy littleBastards;
    public Enemy rareBastards;
    float maxCool = 5;
    float minicool = 0.8f;
    float minicooler = 0.8f;
    float cool;
    int dosCount = 0;
    bool move = false;
	// Use this for initialization
	void Start ()
    {
        //SetActor(100, 1, 1, 0.8f);
        cool = maxCool;
        //safeHealth = health;
	
	}

    void SpawnBossEnemies(Vector2 _spawnPoint, Enemy _enemy)
    {
        Enemy e = EnemyManager.instance.EnemyPooling(_enemy);
        e.transform.position = _spawnPoint;
        e.gameObject.SetActive(true);

    }

    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    {
        if (shootCooldown >= shootRate)
        {
            for (int i = 0; i < _shootTransform.Length; i++)
            {
                Vector3 shootDir = new Vector3(Player.instance.transform.position.x,  Player.instance.transform.position.y )- _shootTransform[i].transform.position;
                if (shootDir.x > 0)
                    shootDir.x = Mathf.Clamp(shootDir.x, 0, 7.5f);
                else
                    shootDir.x = Mathf.Clamp(shootDir.x, -7.5f, 0);

                Projectile p = ProjectileManager.instance.PoolingProjectile(_shootTransform[i].transform);
                p.SetProjectile(_projData, shootDir);
                p.transform.position = _shootTransform[i].transform.position;
                p.gameObject.SetActive(true);
                shootCooldown = 0;
                p.GetComponentInChildren<ParticleSystem>().startLifetime = .35f;
            }
            return true;
        }
        return false;
    }

    // Update is called once per frame
    protected override void Update() 
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            MiniCool();

            if (cool <= maxCool)
            {
                cool += Time.deltaTime;
            }

            if (move)
            {
                EyeShooting();
            }


            //Debug.Log("count " + dosCount + " cool " + cool);

            base.Update();
        }
	}

    protected override void Movement()
    {
		transform.position = Vector2.MoveTowards(transform.position, bossTarget,speed*2* Time.deltaTime);
		if (Vector2.Distance(transform.position,bossTarget)<1)
        {
            move = true;
        }
    }

    void EyeShooting()
    {
        if (dosCount<10&&cool>=maxCool)
        {
            if (MiniCool())
            {
              SpawnBossEnemies(eyeShot[Random.Range(0,eyeShot.Length - 1)].position, littleBastards);
                minicooler = 0;
                dosCount++;
            }
        }
        else
        {
            dosCount = 0;
        }
        if (dosCount >= 10 && cool >= maxCool)
        {
            cool = 0;
        }
    }
   
  
   bool MiniCool()
    {
        if (minicooler <= minicool)
        {
            minicooler += Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
}
