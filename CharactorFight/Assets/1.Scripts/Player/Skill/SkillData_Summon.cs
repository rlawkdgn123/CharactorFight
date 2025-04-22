using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataSummon", menuName = "Scriptable Object/SkillDataSummon", order = int.MaxValue)]
public class SkillData_Summon : ScriptableObject
{
    [Header("�⺻ ����")]
    public string skillName; // ��ų �̸�
    public int skillNum;     // ��ų ��ȣ
    public Sprite skillIcon; // ��ų ������
    [Header("�ڿ�")]
    public float cooldown; // ��ų ��ٿ� �ð�
    public int manaCost;    // �Ҹ� ������
    [Header("����")]
    public int damage; // ���ݷ�
    public float attackSpeed; // ���ݼӵ�
    public float range; // ���� ��Ÿ�
    public float movespeed; // �̵��ӵ�
    [Header("���� �� ����")]
    public float spawnDelay; // ���� ����
    public float duration; // ��ų ���ӽð�
    public Vector2 knockback; // �˹� �Ÿ�
    public float spawnPosx = 2; // ���� ��ġ X
    public float spawnPosy; // ���� ��ġ Y
    [Header("��Ÿ")]
    public GameObject sommonerPrefab; // ��ȯ�� ������
    public bool freezePlayerPos; // ��ų ��� �� �÷��̾� ���� ����
    public AudioClip skillSpawnAudioSource; // ��ų ���� �� ȿ���� �迭
    public AudioClip skillDestroyAudioSource; // ��ų �Ҹ� �� ȿ���� �迭
}
