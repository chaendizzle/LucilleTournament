using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot : MonoBehaviour, IAttackEffect
{
    int IStatus.Order => 201;

    List<float> angles = new List<float>() { -30f, 30f };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<GameObject> IAttackEffect.Apply(AttackSpawner attackSpawner, List<GameObject> projectiles)
    {
        List<GameObject> tripleShot = new List<GameObject>(projectiles);
        foreach (float angle in angles)
        {
            foreach (GameObject go in projectiles)
            {
                GameObject proj = Instantiate(go, go.transform.position, go.transform.rotation * Quaternion.Euler(0f, 0f, angle));
                proj.GetComponent<Projectile>().Initialize(go.GetComponent<Projectile>().sourceEntity);
                tripleShot.Add(proj);
            }
        }
        return tripleShot;
    }
}
