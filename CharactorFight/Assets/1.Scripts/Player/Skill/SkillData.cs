using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 작성자 : 김장후
[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData", order = int.MaxValue)]
public class SkillData : ScriptableObject
{
    [Header("기본 세팅")]
    public string skillName; // 스킬 이름
    public int skillNum;     // 스킬 번호
    public Sprite skillIcon; // 스킬 아이콘
    [Header("자원")]
    public float cooldown; // 스킬 쿨다운 시간
    public int manaCost;    // 소모 마나량
    [Header("공격")]
    public int damage; // 공격력
    public int attackSpeed; // 공격속도
    public float range; // 공격 사거리
    [Header("생성 및 지연")]
    public bool spawnEffect;
    public float spawnDelay; // 생성 지연
    public float duration; // 스킬 지속시간
    public Vector2 knockback; // 넉백 거리
    public float spawnPosx = 2; // 생성 위치 X
    public float spawnPosy; // 생성 위치 Y
    public float motionTime; // 스킬 모션 시간
    [Header("스킬 발사")]
    public bool skillFire; // 스킬 프리팹 발사 여부
    public bool skillPenetrate; // 발사체 관통 여부
    public bool skillGrowing; // 지속시간 따라 스킬 크기 커지는 여부
    public float skillGrowingSpeed; // 커지는 속도
    public bool skillFaster; // 지속시간 따라 스킬 속도 빨라지는 여부
    public float skillSpeed; // 발사체 및 소환체 이동속도
    public bool skillAddSpeed; // 지속시간 따라 빨라지는 이동속도 더하기값
    public Vector3 skillMaxSize; // 스킬 커질 때 최대 사이즈
    [Header("기타")]
    public GameObject spawnEffectPrefab; // 스폰 프리팹
    public GameObject projectilePrefab; // 스킬 프리팹
    public GameObject explosionPrefab; // 스킬 폭발 프리팹
    public bool freezePlayerPos; // 스킬 사용 시 플레이어 정지 여부
    public AudioClip skillSpawnAudioSource; // 스킬 생성 시 효과음 배열
    public AudioClip skillDestroyAudioSource; // 스킬 소멸 시 효과음 배열
}
