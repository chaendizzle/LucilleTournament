using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDisplayInfoList", menuName = "ScriptableObjects/CardDisplayInfoList", order = 1)]
public class CardDisplayInfoList : ScriptableObject
{
    public List<CardDisplayInfo> CardList;

    private Dictionary<string, CardDisplayInfo> cardDict;
    public Dictionary<string, CardDisplayInfo> Cards
    {
        get
        {
            if (cardDict == null)
            {
                cardDict = new Dictionary<string, CardDisplayInfo>();
                foreach (CardDisplayInfo c in CardList)
                {
                    cardDict[c.name] = c;
                }
            }
            return cardDict;
        }
    }

    public CardDisplayInfo SpawnAttackObj(string name)
    {
        if (!Cards.ContainsKey(name))
        {
            throw new KeyNotFoundException($"No CardDisplayInfo {name} is registered with CardDisplayInfoList.");
        }
        return Cards[name];
    }
}