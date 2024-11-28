using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool facingUp = true;


    public enum cardType
    {
        catCry,
        catEatPaw,
        catGigaChad,
        christianHamster,
        dogLol,
        ebilCat
    }

    // Select the type of card it is in the inspector with this drop list.
    [SerializeField] private cardType _cardType;

    public cardType GetCardType()
    {
        return _cardType;
    }

    // Flip the current card.
    public void FlipCard()
    {
        if (facingUp)
            StartCoroutine(FlipAnimation(new Vector3(0, 0, 180)));
        else
            StartCoroutine(FlipAnimation(new Vector3(0, 0, 0)));

        facingUp = !facingUp;
    }

    // This is for the animation of the flip.
    [SerializeField] private float flipDuration = 0.5f;

    // Flip animation for the cards.
    private IEnumerator FlipAnimation(Vector3 targetRotation)
    {
        var timeElapsed = 0f;
        var startRotation = transform.rotation;
        var endRotation = Quaternion.Euler(targetRotation);

        while (timeElapsed < flipDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / flipDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        // Makes sure it gets to the correct end rotation (since its time dependant and not frame-rate dependant).
        transform.rotation = endRotation;
    }
}