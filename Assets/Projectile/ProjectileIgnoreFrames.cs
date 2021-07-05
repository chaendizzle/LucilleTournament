using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileIgnoreFrames : MonoBehaviour
{
    int ready = 0;
    public int Frames = 1;

    void FixedUpdate()
    {
        if (ready >= Frames)
        {
            GetComponent<Collider2D>().enabled = true;
            Destroy(this);
        }
        ready++;
    }
}
