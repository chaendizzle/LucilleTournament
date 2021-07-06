using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// card play handler for triple shot
public class StatusEffectCardHandler : MonoBehaviour, ICardHandler
{
    public string Name;
    public bool Stack = false;
    public float Duration = 0f;

    void ICardHandler.Play()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!Stack)
        {
            UnityEngine.Object old = player.GetComponent(Type.GetType(Name));
            if (old != null)
            {
                Destroy(old);
            }
        }
        UnityEngine.Object effect = player.AddComponent(Type.GetType(Name));
        Debug.Log(effect);
        if (Duration > 0)
        {
            StartCoroutine(Timer(effect));
        }
    }

    IEnumerator Timer(UnityEngine.Object effect)
    {
        float timer = 0f;
        // TODO: update some UI component with duration
        while (timer < Duration)
        {
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(effect);
    }
}
