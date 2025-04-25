using UnityEngine;
using static PlayerBase;
[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour
{
    BoxCollider2D col;

    private void Awake()
    {
        col = gameObject.GetComponent<BoxCollider2D>();
    }
    public void ColOn(PlayerChoice player)
    {
        if (player == PlayerChoice.P1)
        {

        }
        else if (player == PlayerChoice.P2)
        {

        }
            col.enabled = true;
    }
    public void ColOff(PlayerChoice player)
    {
        col.enabled = false;
    }
}
