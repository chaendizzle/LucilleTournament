using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AttackSpawner attackSpawner;

    PlayerInput input;
    Entity entity;
    EntityStatuses statuses;

    float cooldown;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        entity = GetComponent<Entity>();
        statuses = GetComponent<EntityStatuses>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (cooldown <= 0 && input.inputState.Fire)
        {
            cooldown += 1;
            Attack();
        }
        if (cooldown > 0)
        {
            // 50 frames per second, goal is AttackSpeed shots per second
            cooldown -= entity.currentStat.AttackSpeed / 50f;
        }
    }

    void Attack()
    {
        Vector2 baseOffset = new Vector2(0.05f, 0.05f);
        List<GameObject> projectiles = new List<GameObject>()
        {
            attackSpawner.SpawnAttackObj(gameObject, "BaseBullet", Vector3.up * baseOffset.x + Vector3.right * baseOffset.y),
            attackSpawner.SpawnAttackObj(gameObject, "BaseBullet", Vector3.up * baseOffset.x + Vector3.right * -baseOffset.y)
        };
        foreach (IAttackEffect attackEffect in statuses.GetAttackEffectList())
        {
            projectiles = attackEffect.Apply(attackSpawner, projectiles);
        }
    }
}
