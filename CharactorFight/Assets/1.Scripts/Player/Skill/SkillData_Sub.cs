using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 작성자 : 김장후
// PlayerSkill을 통해 폭발 등의 프리팹으로 생성된 2차적인 스킬 프리팹을 관리합니다.
[CreateAssetMenu(fileName = "SkillData_Sub", menuName = "Scriptable Object/SkillData_Sub", order = int.MaxValue)]
public class SkillData_Sub : ScriptableObject
{
    [Header("기본 세팅")]
    public string skillName; // 스킬 이름
    [Header("공격")]
    public int damage; // 공격력
    [Header("생성 및 지연")]
    public float spawnDelay; // 생성 지연
    public float duration; // 스킬 지속시간
    public Vector2 knockback; // 넉백 거리
    public float spawnPosx = 2; // 생성 위치 X
    public float spawnPosy; // 생성 위치 Y
    [Header("스킬 발사")]
    public bool skillFire; // 스킬 프리팹 발사 여부
    public bool skillPenetrate; // 발사체 관통 여부
    public float skillSpeed; // 발사체 이동속도
    [Header("스킬 부착")]
    public bool skillAttach; // 스킬 프리팹의 만난 적에게 부착 여부
    public Vector2 skillAttachPos; // 스킬 부착 위치 (대상 + 위치)
    [Header("군중 제어")]
    public bool Stun;   // 경직 여부
    public float StunTime;  // 경직 시간
    public float stunTick;  // 경직 간격 (사용할 경우)
    public int stunCycle;   // 경직 사이클 (사용할 경우)
    [Header("기타")]
    public GameObject projectilePrefab; // 정보를 가져올 원본 스킬 프리팹
    public GameObject ExplosionPrefab; // 스킬 폭발 프리팹
    public AudioClip skillSpawnAudioSource; // 스킬 생성 시 효과음 배열
    public AudioClip skillDestroyAudioSource; // 스킬 소멸 시 효과음 배열
}

/* 기본적으로 projectilePrefab에서 추후 생성된 스킬 폭발 프리팹에게 자신 데이터의 정보를 넘기는 형태로 진행될 것
 */