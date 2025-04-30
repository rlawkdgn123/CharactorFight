using UnityEngine;

public class PlatformSensor : MonoBehaviour
{
    PlayerBase player;
    [SerializeField] private float groundActiveDis = 5f; // 바닥 활성화 거리
    [SerializeField] private float groundRayWidth = 5f; // 바닥감지 레이 길이
    [SerializeField] private float groundRayStartPosition = 5f; // 바닥감지 레이 길이

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
        
        // 레이 시작 위치 설정
        Vector2 rayStartPosition = new Vector2(transform.position.x - groundRayStartPosition, transform.position.y - groundActiveDis);

        // 저장된 오브젝트를 임시로 복사 (과거 오브젝트 백업)
        GameObject[] oldHitPlatformObjs = new GameObject[hitPlatformObjs.Length];
        for (int i = 0; i < hitPlatformObjs.Length; i++)
            oldHitPlatformObjs[i] = hitPlatformObjs[i];

        // 원래 hitPlatformObjs 비우기
        for (int i = 0; i < hitPlatformObjs.Length; i++)
            hitPlatformObjs[i] = null;

        // 오른쪽으로 레이 발사
        RaycastHit2D[] hitPlatforms = Physics2D.RaycastAll(rayStartPosition, Vector2.right, groundRayWidth, LayerMask.GetMask("Ground"));

        // 이번 프레임 새로 맞은 오브젝트 저장
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

        // 과거 오브젝트가 이번 레이에 없으면 includeLayers 끄기
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
                    boxCol.includeLayers &= ~layerNumPlayer; // includeLayers 끄기
                }
            }



            // boxCol.includeLayers &= ~LayerMask.GetMask("P1");
            // 레이 경로를 씬 뷰에서 그리기
            Debug.DrawRay(rayStartPosition, Vector2.right * groundRayWidth, Color.red);
        }
        /*
        foreach (RaycastHit2D hit in groundHits)
        {
            // 예시로 "Ground" 태그를 가진 오브젝트만 배열에 추가
            if (hit.collider.CompareTag("Ground"))
            {
                // 이미 배열에 포함되어 있는지 확인하고 추가
                if (!hitGroundObjs.Contains(hit.collider.gameObject))
                {
                    hitGroundObjs.Add(hit.collider.gameObject);
                    hit.collider.gameObject.GetComponent<Platform>().ColOn(GetCurChoice());
                    Debug.Log("레이캐스트가 'Ground' 오브젝트에 닿았습니다: " + hit.collider.gameObject.name);
                }
            }
        }
        for (int i = hitGroundObjs.Count - 1; i >= 0; i--)
        {
            // 배열의 오브젝트가 더 이상 충돌하지 않으면 배열에서 제거
            if (!hitGroundObjs[i].GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Debug.Log("레이캐스트가 'Ground' 오브젝트에서 벗어났습니다: " + hitGroundObjs[i].name);
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
