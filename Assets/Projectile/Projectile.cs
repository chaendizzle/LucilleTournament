using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject sourceEntity { get; private set; }
    public float shotSpeed { get; private set; } = 0;

    public CombatSide side { get; private set; } = CombatSide.NEUTRAL;

    // damage multiplier of this projectile
    public float BaseDamage = 1f;

    public void Initialize(GameObject sourceEntity)
    {
        this.sourceEntity = sourceEntity;
        side = sourceEntity.GetComponent<Entity>().side;
        if (sourceEntity.layer == LayerMask.NameToLayer("PlayerEntity"))
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        }
        if (sourceEntity.layer == LayerMask.NameToLayer("EnemyEntity"))
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
        shotSpeed = sourceEntity.GetComponent<Entity>().currentStat.ShotSpeed;
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
        // if too far away, destroy
        if (transform.position.magnitude > 11f)
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetTarget()
    {
        string sideTag = "";
        switch (side)
        {
            case CombatSide.PLAYER:
                sideTag = "Enemy";
                break;
            case CombatSide.ENEMY:
                sideTag = "Player";
                break;
        }
        float minDist = float.MaxValue;
        GameObject target = null;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(sideTag))
        {
            Vector2 dist = go.transform.position - transform.position;
            if (dist.magnitude < minDist)
            {
                minDist = dist.magnitude;
                target = go;
            }
        }
        return target;
    }
}

// receive event on a projectile when it hits
public interface IProjectileHit
{
    void HitTarget();
}