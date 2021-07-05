using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput input;
    Entity entity;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        entity = GetComponent<Entity>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        if (entity.currentStat == null)
        {
            return;
        }
        rb.velocity = ((transform.right * input.inputState.X) + (transform.up * input.inputState.Y)).normalized * entity.currentStat.Speed;
    }
}
