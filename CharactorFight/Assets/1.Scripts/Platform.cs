using UnityEngine;
using static PlayerBase;
[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour
{
    BoxCollider2D col;
    int layer;

    private void Awake()
    {
        col = gameObject.GetComponent<BoxCollider2D>();
        layer = gameObject.layer;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>() is PlayerBase player)
        {
            PlatformSensor ps = player.gameObject.GetComponentInChildren<PlatformSensor>();
            for (int i = 0; i < ps.hitPlatformObjs.Length; i++)
            {
                if (ps.hitPlatformObjs[i] == gameObject)
                {
                    if (player.GetCurChoice() == PlayerChoice.P1)
                        Physics2D.IgnoreLayerCollision(layer, LayerMask.NameToLayer("P1"), true);
                    else if (player.GetCurChoice() == PlayerChoice.P2)
                        Physics2D.IgnoreLayerCollision(layer, LayerMask.NameToLayer("P2"), true);

                    break;
                }
            }
        }
    
        //col.enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>() is PlayerBase player)
        {
            PlatformSensor ps = player.gameObject.GetComponentInChildren<PlatformSensor>();
            for (int i = 0; i < ps.hitPlatformObjs.Length; i++)
            {
                if (ps.hitPlatformObjs[i] == gameObject)
                {
                    if (player.GetCurChoice() == PlayerChoice.P1)
                        Physics2D.IgnoreLayerCollision(layer, LayerMask.NameToLayer("P1"), false);
                    else if (player.GetCurChoice() == PlayerChoice.P2)
                        Physics2D.IgnoreLayerCollision(layer, LayerMask.NameToLayer("P2"), false);
                    break;
                }
            }
        }
    }
}
