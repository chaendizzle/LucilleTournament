using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IOnDamageEffect
{
    int IStatus.Order => 1001;
    SpriteRenderer sr;
    // how much to tint due to damage
    float damageTint = 0f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        damageTint -= Mathf.Min(0.5f, damageTint * 5f / 50f);
        if (damageTint < 0.005f)
        {
            damageTint = 0f;
        }
        sr.color = Color.Lerp(Color.white, Color.red, damageTint);
    }

    float IOnDamageEffect.Apply(GameObject entity, GameObject sourceEntity, GameObject projectile, float damage)
    {
        if (damage <= 0)
        {
            return damage;
        }
        // flash more red with more damage
        damageTint += Mathf.Min(0.5f, 0.06f + damage / 80f);
        return damage;
    }
}
