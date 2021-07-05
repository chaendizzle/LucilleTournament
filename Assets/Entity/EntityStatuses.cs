using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityStatuses : MonoBehaviour
{
    Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EntityStat GetCurrentStat(EntityStat baseStat)
    {
        List<IStatEffect> statEffects = GetComponents<IStatEffect>().ToList();
        statEffects.OrderBy(x => x.Order);
        EntityStat stat = (EntityStat)baseStat.Clone();
        foreach (IStatEffect statEffect in statEffects)
        {
            if (!(statEffect is MonoBehaviour) || (statEffect as MonoBehaviour).enabled)
            {
                stat = statEffect.Apply(stat);
            }
        }
        return stat;
    }

    public List<IAttackEffect> GetAttackEffectList()
    {
        List<IAttackEffect> attackEffects = GetComponents<IAttackEffect>().Where(x => !(x is MonoBehaviour) || (x as MonoBehaviour).enabled).ToList();
        return attackEffects.OrderBy(x => x.Order).ToList();
    }

    public float GetDamage(GameObject entity, GameObject sourceEntity, GameObject projectile)
    {
        float damage = sourceEntity.GetComponent<Entity>().currentStat.Damage * projectile.GetComponent<Projectile>().BaseDamage;
        List<IOnHitEffect> onHitEffects = sourceEntity.GetComponents<IOnHitEffect>().ToList();
        onHitEffects.OrderBy(x => x.Order);
        foreach (IOnHitEffect onHitEffect in onHitEffects)
        {
            if (!(onHitEffect is MonoBehaviour) || (onHitEffect as MonoBehaviour).enabled)
            {
                damage = onHitEffect.Apply(entity, sourceEntity, projectile, damage);
            }
        }
        List<IOnDamageEffect> onDamageEffects = entity.GetComponents<IOnDamageEffect>().ToList();
        onDamageEffects.OrderBy(x => x.Order);
        foreach (IOnDamageEffect onDamageEffect in onDamageEffects)
        {
            if (!(onDamageEffect is MonoBehaviour) || (onDamageEffect as MonoBehaviour).enabled)
            {
                damage = onDamageEffect.Apply(entity, sourceEntity, projectile, damage);
            }
        }
        return damage;
    }
}

// tag for game status components
public interface IStatus {
    // Example:
    // Order 1: Damage x2 
    // Order 2: Damage +4
    // Result: ((10 damage base) * 2) + 4 = 24 damage
    // Status effects at the same order level are processed in order of first applied.
    int Order { get; }
}
public interface IBuff : IStatus { }
public interface IDebuff : IStatus { }
public interface IStatEffect : IStatus {
    EntityStat Apply(EntityStat stats);
}
public interface IAttackEffect : IStatus {
    // Modify the player's attack.
    // Example: Triple attack would clone the current projectiles amd change their direction.
    // Example: Missile attack would destroy the player's projectiles and replace them with a missile.
    List<GameObject> Apply(AttackSpawner attackSpawner, List<GameObject> projectiles);
}
// called when entity damages another entity
public interface IOnHitEffect : IStatus {
    float Apply(GameObject entity, GameObject sourceEntity, GameObject projectile, float damage);
}
// called when entity takes damage. Applied after IOnHitEffect
public interface IOnDamageEffect : IStatus
{
    float Apply(GameObject entity, GameObject sourceEntity, GameObject projectile, float damage);
}