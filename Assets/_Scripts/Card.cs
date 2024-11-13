using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool facingUp = true;
    public bool cardSelected = false;
    


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
    
    public void FlipCard()
    {
        if (facingUp)
        {
            StartCoroutine(FlipAnimation(new Vector3(0,0,180)));
        }
        else
        {
            StartCoroutine(FlipAnimation(new Vector3(0,0,0)));
        }

        facingUp = !facingUp;
    }

    // This is for the animation of the flip.
    [SerializeField] private float flipDuration = 0.5f;
    // FUTURE - Make the animation slower
    private IEnumerator FlipAnimation(Vector3 targetRotation)
    {
        float timeElapsed = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(targetRotation);

        while (timeElapsed < flipDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / flipDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
}