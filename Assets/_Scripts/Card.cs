using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool facingUp = true;
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
        FlipAnimation();
    }

    // FUTURE - Make the animation slower
    private void FlipAnimation()
    {
        facingUp = !facingUp;

        if (facingUp)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }
}