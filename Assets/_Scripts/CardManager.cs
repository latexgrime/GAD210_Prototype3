using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [Header("Game settings.")]

    // Pool that will contain all cards (will contain double the amount of the cards array).
    private readonly List<GameObject> cardPool = new();

    /// <summary>
    ///     The time the cards are going to be show to the player at the beginning.
    /// </summary>
    [SerializeField] private float hintTime;

    /// <summary>
    ///     The interval between shuffles.
    /// </summary>
    [SerializeField] private float shuffleInterval = 5f;

    /// <summary>
    ///     The duration that will take two cards to change location.
    /// </summary>
    [SerializeField] private float movingDuration = 1.5f;

    /// <summary>
    ///     The transforms of the location in which the cards are going to be instantiated. Needs to be a pair number.
    /// </summary>
    [SerializeField] private Transform[] cardPositions;

    /// <summary>
    ///     Prefabs of the cards game objects.
    /// </summary>
    [SerializeField] private GameObject[] cards;

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
        StartCoroutine(ShuffleCardsPeriodically());
    }

    private readonly List<GameObject> instantiatedCards = new();

    private void InstantiateCardsInShuffledPosition()
    {
        for (var i = 0; i < cardPositions.Length; i++)
        {
            var cardInstance = Instantiate(cardPool[i], cardPositions[i].position, Quaternion.Euler(0, 0, 0));

            // Set the instanced card as a child of the position object, to be able to access the card just by using the position.
            cardInstance.transform.SetParent(cardPositions[i]);

            // DEBUG - Delete later.
            instantiatedCards.Add(cardInstance);

            var cardScript = cardInstance.GetComponentInParent<Card>();
            if (cardScript != null)
                // Flips the card after showing a hint to the player.
                StartCoroutine(EndShowingHint(cardScript, hintTime));
        }
    }

    private IEnumerator EndShowingHint(Card card, float delay)
    {
        yield return new WaitForSeconds(delay);
        card.FlipCard();
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

    public List<Card> selectedCards = new();

    // If the counter is the same as the amount of cards in the GameObject array of cards (line 10), then the game is won. (because all pairs are facing up)
    private int correctGuessesCounter;
    public bool isResetting;

    private void Update()
    {
        SelectedCardsPairChecker();
        CheckIfWon();
    }

    // Checks for two selected cards.
    private void SelectedCardsPairChecker()
    {
        if (selectedCards.Count >= 2)
        {
            if (selectedCards[0].GetCardType() == selectedCards[1].GetCardType())
            {
                Debug.Log("Match.");
                correctGuessesCounter++;
                selectedCards.Clear();
            }
            else
            {
                Debug.Log("Not a match.");
                if (!isResetting) StartCoroutine(ResetCard());
            }
        }
    }

    // Reset the cards after a small period of time to make tell the player they didn't get a match.
    private IEnumerator ResetCard()
    {
        isResetting = true;
        yield return new WaitForSeconds(1f);
        foreach (var card in selectedCards)
        {
            yield return new WaitForSeconds(0.2f);
            card.FlipCard();
        }

        selectedCards.Clear();
        isResetting = false;
    }


    // Checks if the game is won.
    private void CheckIfWon()
    {
        if (correctGuessesCounter == cards.Length) Debug.Log("Game won.");
    }

    private IEnumerator ShuffleCardsPeriodically()
    {
        // The true condition can be changed to a timer if it gets implemented.
        while (true)
        {
            // If two cards are being reset, wait until they're done resetting.
            yield return new WaitUntil(() => !isResetting);

            // Start the shuffling after the shuffling interval.
            yield return new WaitForSeconds(Random.Range(shuffleInterval, shuffleInterval + 4));

            // The two positions that are going to be changing places.
            var index1 = Random.Range(0, instantiatedCards.Count);
            var index2 = Random.Range(0, instantiatedCards.Count);

            // This is to make sure that both positions are not the same.
            while (index1 == index2) index2 = Random.Range(0, instantiatedCards.Count);

            var card1 = instantiatedCards[index1]; // Gets the first child which is the card occupying this space.
            var card2 = instantiatedCards[index2];

            // Correct the indexes of the cards in the list so it doesn't de-synchronize in the future.
            // (This was causing the bug of the cards being overlapped)
            instantiatedCards[index1] = card2;
            instantiatedCards[index2] = card1;

            // Animate the shuffle of the cards.
            StartCoroutine(AnimateCardShuffle(card1.transform, cardPositions[index2].position, movingDuration));
            StartCoroutine(AnimateCardShuffle(card2.transform, cardPositions[index1].position, movingDuration));

            yield return new WaitForSeconds(movingDuration);
        }
    }


    private IEnumerator AnimateCardShuffle(Transform card, Vector3 targetPosition, float animDuration)
    {
        var startPosition = card.position;
        var timeElapsed = 0f;

        // To prevent sudden shifts (quick workaround for overlapping cards when the player interacts with cards)
        card.position = startPosition;

        while (timeElapsed < animDuration)
        {
            card.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / animDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // To prevent bugs.
        card.position = targetPosition;
    }

    #endregion
}