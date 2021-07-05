using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovement : MonoBehaviour, IProjectileHit
{
    Projectile projectile;

    // dampen missile movement towards target
    float dampening = 1.0f;
    // missile turn speed
    float turnSpeed = 120f;

    bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponent<Projectile>();
    }

    void FixedUpdate()
    {
        if (hit)
        {
            return;
        }
        Vector2 targetPos;
        // select target
        GameObject target = projectile.GetTarget();
        if (target != null)
        {
            targetPos = target.transform.position;
        }
        else
        {
            targetPos = transform.position + Vector3.up;
        }
        // move towards target
        Vector2 diff = targetPos - (Vector2)transform.position;
        float targetRotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90f;
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetRotation, turnSpeed / 50f));
        transform.position += projectile.shotSpeed * 0.35f * (transform.up + (Vector3)diff.normalized * dampening) / 50f;
    }

    // display effects
    void IProjectileHit.HitTarget()
    {
        hit = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Hit");
    }
    void Cleanup()
    {
        Destroy(gameObject);
    }
}
