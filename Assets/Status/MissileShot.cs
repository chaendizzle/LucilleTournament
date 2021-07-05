using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileShot : MonoBehaviour, IAttackEffect
{
    int IStatus.Order => 101;

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
        foreach (GameObject go in projectiles)
        {
            Destroy(go);
        }
        Vector2 baseOffset = new Vector2(0.05f, 0f);
        return new List<GameObject>()
        {
            attackSpawner.SpawnAttackObj(gameObject, "Missile", Vector3.up * baseOffset.x + Vector3.right * baseOffset.y),
        };
    }
}
