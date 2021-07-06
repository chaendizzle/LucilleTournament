using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class CardBackend : MonoBehaviour
{
    public CardDisplayInfoList CardTypes;

    List<Card> Deck = new List<Card>();

    // top of the pile is at index 0, bottom of pile is at pile.Count - 1
    List<Card> DrawPile = new List<Card>();
    List<Card> DiscardPile = new List<Card>();
    List<Card> Hand = new List<Card>();

    int MaxHandSize = 8;
    int StartingHand = 2;

    static System.Random random;
    int Seed = 0;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random(Seed);
        Deck.Add(new Card(CardTypes.Cards["TripleShot"]));
        Deck.Add(new Card(CardTypes.Cards["MissileShot"]));
        StartCombat();
        PlayCard(Hand[0]);
        PlayCard(Hand[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToDeck(Card card)
    {
        Deck.Add(card);
        Deck.Sort();
    }

    public void StartCombat()
    {
        // deck becomes draw pile
        DrawPile.AddRange(Deck.Select(x => (Card)x.Clone()));
        Shuffle(DrawPile);
        // draw starting hand
        for (int i = 0; i < StartingHand; i++)
        {
            DrawCard();
        }
    }

    void Shuffle(List<Card> cards)
    {
        // Fisher-Yates shuffle algorithm
        for (int n = cards.Count - 1; n > 0; --n)
        {
            int k = random.Next(n + 1);
            Card temp = cards[n];
            cards[n] = cards[k];
            cards[k] = temp;
        }
    }

    public void EndCombat()
    {
        // clear draw/discard/hand
        DrawPile.Clear();
        DiscardPile.Clear();
        Hand.Clear();
    }

    public bool DrawCard()
    {
        // check that hand size will not be exceeded
        if (Hand.Count >= MaxHandSize)
        {
            return false;
        }
        // if draw pile is empty, shuffle discard pile back into draw pile
        if (DrawPile.Count == 0)
        {
            DrawPile.AddRange(DiscardPile);
            DiscardPile.Clear();
        }
        // if draw pile is still empty, fail
        if (DrawPile.Count == 0)
        {
            return false;
        }
        // take card from draw pile, place into hand
        Hand.Add(DrawPile[0]);
        DrawPile.RemoveAt(0);
        return true;
    }

    public bool DiscardCard(Card card)
    {
        // move from hand into discard pile
        if (Hand.Contains(card))
        {
            Hand.Remove(card);
            DiscardPile.Add(card);
            return true;
        }
        // or move from draw pile into discard pile
        else if (DrawPile.Contains(card))
        {
            Hand.Remove(card);
            DiscardPile.Add(card);
            return true;
        }
        return false;
    }

    public bool PlayCard(Card card)
    {
        if (!Hand.Contains(card))
        {
            return false;
        }
        foreach (ICardHandler handler in Instantiate(card.info.Handler).GetComponents<ICardHandler>())
        {
            if (!(handler is MonoBehaviour) || (handler as MonoBehaviour).enabled)
            {
                handler.Play();
            }
        }
        DiscardCard(card);
        return true;
    }

    // packages up current state as a copy to give to UI for displaying
    void ToUIState()
    {

    }
}

public class Card : ICloneable
{
    // generates IDs starting at 1
    static int _nextID = 0;
    static int NextID { get { _nextID++; return _nextID; } }

    public CardDisplayInfo info;
    // unique card identifier
    public int ID;

    // TODO: support for card modifiers like upgrades and whatnot

    public Card(CardDisplayInfo info)
    {
        this.info = info;
        ID = NextID;
    }

    public static string Format(string input, object p)
    {
        foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(p))
        {
            input = input.Replace("{" + prop.Name + "}", (prop.GetValue(p) ?? "(null)").ToString());
        }
        return input;
    }

    public object Clone()
    {
        return new Card(info);
    }
}

public interface ICardHandler
{
    // TODO: support for card modifiers
    void Play();
}