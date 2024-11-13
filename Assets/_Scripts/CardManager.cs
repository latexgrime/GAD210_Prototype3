using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Transform[] cardPositions;
    [SerializeField] private GameObject[] cards;

    // Pool that will contain all cards (will contain double the amount of the cards array).
    private readonly List<GameObject> cardPool = new();

    #region Game Preparation
    private void Start()
    {
        // Just to check if the length of the transforms is not pair (which makes the game literally unplayable),
        // and to check if the amount of the transforms is double of the amount of cards.
        // (Since a pair of cards is needed to play the memory game).
        if (cardPositions.Length % 2 != 0 || cards.Length * 2 != cardPositions.Length)
            Debug.LogError(
                "CardManager.cs: the length of the cardPositions array is not pair or the amount of the cards array is not half of the cardsPosition array.");

        DuplicateCards();
        ShuffleCards();
        InstantiateCardsInShuffledPosition();
    }

    private IEnumerator FlipCardAfterDelay(Card card, float delay)
    {
        yield return new WaitForSeconds(delay);
        card.FlipCard();
    }
    
    private void InstantiateCardsInShuffledPosition()
    {
        for (var i = 0; i < cardPositions.Length; i++)
        {
            GameObject cardInstance = Instantiate(cardPool[i], cardPositions[i].position, Quaternion.Euler(0,0,0));
            StartCoroutine(FlipCardAfterDelay(cardInstance.GetComponent<Card>(), 5f));
        }
    }

    private void ShuffleCards()
    {
        for (var i = 0; i < cardPool.Count; i++)
        {
            // Hold the current card in this position.
            var holder = cardPool[i];
            // Select a random index.
            var randomIndex = Random.Range(i, cardPool.Count);
            cardPool[i] = cardPool[randomIndex];
            // Move it to that index.
            cardPool[randomIndex] = holder;
        }
    }

    private void DuplicateCards()
    {
        foreach (var card in cards)
        {
            // Add twice because two of them are needed for a memory game.
            cardPool.Add(card);
            cardPool.Add(card);
        }
    }
    #endregion

    #region Game Manager

    public List<Card> selectedCards = new List<Card>();
    // If the counter is the same as the amount of cards in the GameObject array of cards (line 10), then the game is won. (because all pairs are facing up)
    private int correctGuessesCounter;
    public bool isResetting;

    private void Update()
    {
        if (selectedCards.Count >= 2)
        {
            Debug.Log($"The selected cards are: {selectedCards}");
            if (selectedCards[0].GetCardType() == selectedCards[1].GetCardType())
            {
                Debug.Log("Match.");
                correctGuessesCounter++;
                selectedCards.Clear();
            }
            else
            {
                Debug.Log("Not a match.");
                if (!isResetting)
                {
                    StartCoroutine(ResetCard());
                }
            }
        }

        CheckIfWon();
    }

    private IEnumerator ResetCard()
    {
        isResetting = true;
        yield return new WaitForSeconds(1f);
        foreach (var card in selectedCards)
        {
            card.FlipCard();
        }
        selectedCards.Clear();
        isResetting = false;
    }

    
    // Checks if the game is won.
    private void CheckIfWon()
    {
        if (correctGuessesCounter == cards.Length)
        {
            Debug.Log("Game won.");
        }
    }
    
    #endregion
}