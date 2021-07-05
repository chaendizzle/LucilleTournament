using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpawner", menuName = "ScriptableObjects/AttackSpawner", order = 1)]
public class AttackSpawner : ScriptableObject
{
    public List<GameObject> PrefabList;

    private Dictionary<string, GameObject> prefabDict;
    public Dictionary<string, GameObject> Prefabs
    {
        get
        {
            if (prefabDict == null)
            {
                prefabDict = new Dictionary<string, GameObject>();
                foreach (GameObject go in PrefabList)
                {
                    prefabDict[go.name] = go;
                }
            }
            return prefabDict;
        }
    }

    public GameObject SpawnAttackObj(GameObject entity, string name, Vector3 position = default)
    {
        if (!Prefabs.ContainsKey(name))
        {
            throw new KeyNotFoundException($"No prefab with name {name} is registered with {entity.name} AttackSpawner.");
        }
        GameObject proj = Instantiate(Prefabs[name], entity.transform.position + position, entity.transform.rotation);
        proj.GetComponent<Projectile>().Initialize(entity);
        return proj;
    }
}