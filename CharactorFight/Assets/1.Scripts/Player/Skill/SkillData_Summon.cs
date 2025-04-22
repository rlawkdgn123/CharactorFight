using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataSummon", menuName = "Scriptable Object/SkillDataSummon", order = int.MaxValue)]
public class SkillData_Summon : ScriptableObject
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
    public float attackSpeed; // 공격속도
    public float range; // 공격 사거리
    public float movespeed; // 이동속도
    [Header("생성 및 지연")]
    public float spawnDelay; // 생성 지연
    public float duration; // 스킬 지속시간
    public Vector2 knockback; // 넉백 거리
    public float spawnPosx = 2; // 생성 위치 X
    public float spawnPosy; // 생성 위치 Y
    [Header("기타")]
    public GameObject sommonerPrefab; // 소환수 프리팹
    public bool freezePlayerPos; // 스킬 사용 시 플레이어 정지 여부
    public AudioClip skillSpawnAudioSource; // 스킬 생성 시 효과음 배열
    public AudioClip skillDestroyAudioSource; // 스킬 소멸 시 효과음 배열
}
