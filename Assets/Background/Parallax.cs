using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// parallax background via cloning
public class Parallax : MonoBehaviour
{
    public Vector2 speed;

    GameObject[,] objs;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Vector2 camSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) - Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector2 spriteSize = new Vector2(sr.bounds.extents.x * 2f, sr.bounds.extents.y * 2f);
        Vector2Int objCount = new Vector2Int(Mathf.CeilToInt(camSize.x / spriteSize.x) + 1, Mathf.CeilToInt(camSize.y / spriteSize.y) + 1);
        objs = new GameObject[objCount.x, objCount.y];
        objs[0, 0] = gameObject;
        Vector3 pos = transform.position;
        for (int i = 0; i < objs.GetLength(0); i++)
        {
            for (int j = 0; j < objs.GetLength(1); j++)
            {
                if (objs[i, j] == null)
                {
                    objs[i, j] = new GameObject($"{gameObject.name}-{i}-{j}");
                    objs[i, j].transform.parent = transform.parent;
                    SpriteRenderer objSr = objs[i, j].AddComponent<SpriteRenderer>();
                    objSr.sprite = sr.sprite;
                    objSr.sortingLayerID = sr.sortingLayerID;
                    objSr.sortingOrder = sr.sortingOrder;
                }
                objs[i, j].transform.position = new Vector3(pos.x + i * spriteSize.x, pos.y + j * spriteSize.y, pos.z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // move and wrap around
        Vector2 min = Camera.main.ScreenToWorldPoint(Vector3.zero) - sr.bounds.extents;
        Vector2 max = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) + sr.bounds.extents + new Vector3((objs.GetLength(0) - 1) * sr.bounds.extents.x, (objs.GetLength(1) - 1) * sr.bounds.extents.y);
        for (int i = 0; i < objs.GetLength(0); i++)
        {
            for (int j = 0; j < objs.GetLength(1); j++)
            {
                objs[i, j].transform.position += (Vector3)speed * Time.deltaTime;
                if (objs[i, j].transform.position.x < min.x)
                {
                    objs[i, j].transform.position += objs.GetLength(0) * sr.bounds.extents.x * 2f * Vector3.right;
                }
                if (objs[i, j].transform.position.x > max.x)
                {
                    objs[i, j].transform.position += objs.GetLength(0) * sr.bounds.extents.x * 2f * Vector3.left;
                }
                if (objs[i, j].transform.position.y < min.y)
                {
                    objs[i, j].transform.position += objs.GetLength(1) * sr.bounds.extents.y * 2f * Vector3.up;
                }
                if (objs[i, j].transform.position.y > max.y)
                {
                    objs[i, j].transform.position += objs.GetLength(1) * sr.bounds.extents.y * 2f * Vector3.down;
                }
            }
        }
    }
}
