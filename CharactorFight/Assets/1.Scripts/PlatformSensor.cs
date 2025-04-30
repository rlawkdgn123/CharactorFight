using UnityEngine;

public class PlatformSensor : MonoBehaviour
{
    PlayerBase player;
    [SerializeField] private float groundActiveDis = 5f; // �ٴ� Ȱ��ȭ �Ÿ�
    [SerializeField] private float groundRayWidth = 5f; // �ٴڰ��� ���� ����
    [SerializeField] private float groundRayStartPosition = 5f; // �ٴڰ��� ���� ����

    [SerializeField] public GameObject[] hitPlatformObjs = new GameObject[5];
    [SerializeField] private LayerMask layerNumPlayer;
    private void Start()
    {
        PlayerInitialize();
    }
    private void Update()
    {
        GroundActive();
    }
    private void GroundActive()
    {
        
        // ���� ���� ��ġ ����
        Vector2 rayStartPosition = new Vector2(transform.position.x - groundRayStartPosition, transform.position.y - groundActiveDis);

        // ����� ������Ʈ�� �ӽ÷� ���� (���� ������Ʈ ���)
        GameObject[] oldHitPlatformObjs = new GameObject[hitPlatformObjs.Length];
        for (int i = 0; i < hitPlatformObjs.Length; i++)
            oldHitPlatformObjs[i] = hitPlatformObjs[i];

        // ���� hitPlatformObjs ����
        for (int i = 0; i < hitPlatformObjs.Length; i++)
            hitPlatformObjs[i] = null;

        // ���������� ���� �߻�
        RaycastHit2D[] hitPlatforms = Physics2D.RaycastAll(rayStartPosition, Vector2.right, groundRayWidth, LayerMask.GetMask("Ground"));

        // �̹� ������ ���� ���� ������Ʈ ����
        for (int i = 0; i < hitPlatforms.Length; i++)
        {
            if (hitPlatforms[i].collider != null)
            {
                hitPlatformObjs[i] = hitPlatforms[i].collider.gameObject;
                BoxCollider2D boxCol = hitPlatformObjs[i].GetComponent<BoxCollider2D>();
                if (boxCol != null)
                {
                    boxCol.includeLayers = layerNumPlayer;
                }
            }
        }

        // ���� ������Ʈ�� �̹� ���̿� ������ includeLayers ����
        for (int i = 0; i < oldHitPlatformObjs.Length; i++)
        {
            GameObject oldObj = oldHitPlatformObjs[i];
            if (oldObj == null)
                continue;

            bool stillHit = false;
            for (int j = 0; j < hitPlatformObjs.Length; j++)
            {
                if (hitPlatformObjs[j] == oldObj)
                {
                    stillHit = true;
                    break;
                }
            }

            if (!stillHit)
            {
                BoxCollider2D boxCol = oldObj.GetComponent<BoxCollider2D>();
                if (boxCol != null)
                {
                    boxCol.includeLayers &= ~layerNumPlayer; // includeLayers ����
                }
            }



            // boxCol.includeLayers &= ~LayerMask.GetMask("P1");
            // ���� ��θ� �� �信�� �׸���
            Debug.DrawRay(rayStartPosition, Vector2.right * groundRayWidth, Color.red);
        }
        /*
        foreach (RaycastHit2D hit in groundHits)
        {
            // ���÷� "Ground" �±׸� ���� ������Ʈ�� �迭�� �߰�
            if (hit.collider.CompareTag("Ground"))
            {
                // �̹� �迭�� ���ԵǾ� �ִ��� Ȯ���ϰ� �߰�
                if (!hitGroundObjs.Contains(hit.collider.gameObject))
                {
                    hitGroundObjs.Add(hit.collider.gameObject);
                    hit.collider.gameObject.GetComponent<Platform>().ColOn(GetCurChoice());
                    Debug.Log("����ĳ��Ʈ�� 'Ground' ������Ʈ�� ��ҽ��ϴ�: " + hit.collider.gameObject.name);
                }
            }
        }
        for (int i = hitGroundObjs.Count - 1; i >= 0; i--)
        {
            // �迭�� ������Ʈ�� �� �̻� �浹���� ������ �迭���� ����
            if (!hitGroundObjs[i].GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Debug.Log("����ĳ��Ʈ�� 'Ground' ������Ʈ���� ������ϴ�: " + hitGroundObjs[i].name);
                hitGroundObjs[i].GetComponent<Platform>().ColOff(GetCurChoice());
                hitGroundObjs.RemoveAt(i);
            }
        }
        */
    }
    public void PlayerInitialize()
    {
        player = gameObject.GetComponentInParent<PlayerBase>();
        switch (player.GetCurChoice())
        {
            case PlayerBase.PlayerChoice.P1:
                layerNumPlayer = LayerMask.GetMask("P1");
                break;
            case PlayerBase.PlayerChoice.P2:
                layerNumPlayer = LayerMask.GetMask("P2");
                break;
            default:
                layerNumPlayer = LayerMask.GetMask("Default");
                break;
        }
    }
}
