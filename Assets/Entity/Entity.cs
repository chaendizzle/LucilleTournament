using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityStat baseStat;
    public EntityStat currentStat { get; private set; }

    public CombatSide side = CombatSide.NEUTRAL;

    EntityStatuses statuses;

    void Awake()
    {
        statuses = GetComponent<EntityStatuses>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EntityStat stat = statuses.GetCurrentStat(baseStat);
        if (currentStat != null)
        {
            stat.CurrentHP = currentStat.CurrentHP;
        }
        currentStat = stat;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();
        if (projectile == null || projectile.side == side)
        {
            return;
        }
        float damage = statuses.GetDamage(gameObject, projectile.sourceEntity, projectile.gameObject);
        currentStat.CurrentHP = Mathf.Max(0f, currentStat.CurrentHP - damage);
        if (currentStat.CurrentHP == 0)
        {
            // die
        }
        // process projectile hit
        foreach (IProjectileHit projHit in projectile.GetComponents<IProjectileHit>())
        {
            if (!(projHit is MonoBehaviour) || (projHit as MonoBehaviour).enabled)
            {
                projHit.HitTarget();
            }
        }
    }
}

[Serializable]
public class EntityStat : ICloneable
{
    public float MaxHP;
    public float CurrentHP;
    public float Damage;
    public float Speed;
    public float AttackSpeed;
    public float ShotSpeed;

    public object Clone()
    {
        return new EntityStat()
        {
            MaxHP = MaxHP,
            CurrentHP = CurrentHP,
            Damage = Damage,
            Speed = Speed,
            AttackSpeed = AttackSpeed,
            ShotSpeed = ShotSpeed,
        };
    }
}

public enum CombatSide
{
    PLAYER, ENEMY, NEUTRAL
}