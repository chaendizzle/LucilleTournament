using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletMovement : MonoBehaviour, IProjectileHit
{
    Projectile projectile;

    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponent<Projectile>();
    }

    void FixedUpdate()
    {
        transform.position += transform.up * projectile.shotSpeed / 50f;
    }

    void IProjectileHit.HitTarget()
    {
        Destroy(gameObject);
    }
}
