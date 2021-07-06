using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDisplayInfo", menuName = "ScriptableObjects/CardDisplayInfo", order = 2)]
public class CardDisplayInfo : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Sprite Image;
    public string Description;
    // prefab which handles the effects of the card
    public GameObject Handler;
}