using UnityEngine;

public class Card : MonoBehaviour
{
    private bool facingUp = true;

    public void FlipCard()
    {

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