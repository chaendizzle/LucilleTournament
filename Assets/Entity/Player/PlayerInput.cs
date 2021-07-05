using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputState inputState { get; private set; } = new PlayerInputState();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputState.AggregatePressed(new PlayerInputState()
        {
            X = Input.GetAxis("Horizontal"),
            Y = Input.GetAxis("Vertical"),
            Fire = Input.GetButton("Fire"),
            Card = new bool[4] {
                Input.GetButtonDown("Card0"),
                Input.GetButtonDown("Card1"),
                Input.GetButtonDown("Card2"),
                Input.GetButtonDown("Card3"),
            },
        });
    }
    void FixedUpdate()
    {
        inputState.ResetPressed();
    }
}

public class PlayerInputState : ICloneable
{
    // whether a directional button is DOWN
    public float X;
    public float Y;
    // whether the shoot button is DOWN
    public bool Fire;
    // whether button is PRESSED this frame
    public bool[] Card = new bool[4];

    // Aggregate using || until next FixedUpdate
    public void AggregatePressed(PlayerInputState next)
    {
        X = next.X;
        Y = next.Y;
        Fire = next.Fire;
        for (int i = 0; i < 4; i++)
        {
            Card[i] = Card[i] || next.Card[i];
        }
    }

    // Once we hit FixedUpdate, reset to unpressed
    public void ResetPressed()
    {
        for (int i = 0; i < 4; i++)
        {
            Card[i] = false;
        }
    }

    public object Clone()
    {
        return new PlayerInputState()
        {
            X = X,
            Y = Y,
            Fire = Fire,
            Card = (bool[])Card.Clone(),
        };
    }
}