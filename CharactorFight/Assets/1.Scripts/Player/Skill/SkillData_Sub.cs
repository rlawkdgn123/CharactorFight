using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �ۼ��� : ������
// PlayerSkill�� ���� ���� ���� ���������� ������ 2������ ��ų �������� �����մϴ�.
[CreateAssetMenu(fileName = "SkillData_Sub", menuName = "Scriptable Object/SkillData_Sub", order = int.MaxValue)]
public class SkillData_Sub : ScriptableObject
{
    [Header("�⺻ ����")]
    public string skillName; // ��ų �̸�
    [Header("����")]
    public int damage; // ���ݷ�
    [Header("���� �� ����")]
    public float spawnDelay; // ���� ����
    public float duration; // ��ų ���ӽð�
    public Vector2 knockback; // �˹� �Ÿ�
    public float spawnPosx = 2; // ���� ��ġ X
    public float spawnPosy; // ���� ��ġ Y
    [Header("��ų �߻�")]
    public bool skillFire; // ��ų ������ �߻� ����
    public bool skillPenetrate; // �߻�ü ���� ����
    public float skillSpeed; // �߻�ü �̵��ӵ�
    [Header("��ų ����")]
    public bool skillAttach; // ��ų �������� ���� ������ ���� ����
    public Vector2 skillAttachPos; // ��ų ���� ��ġ (��� + ��ġ)
    [Header("���� ����")]
    public bool Stun;   // ���� ����
    public float StunTime;  // ���� �ð�
    public float stunTick;  // ���� ���� (����� ���)
    public int stunCycle;   // ���� ����Ŭ (����� ���)
    [Header("��Ÿ")]
    public GameObject projectilePrefab; // ������ ������ ���� ��ų ������
    public GameObject ExplosionPrefab; // ��ų ���� ������
    public AudioClip skillSpawnAudioSource; // ��ų ���� �� ȿ���� �迭
    public AudioClip skillDestroyAudioSource; // ��ų �Ҹ� �� ȿ���� �迭
}

/* �⺻������ projectilePrefab���� ���� ������ ��ų ���� �����տ��� �ڽ� �������� ������ �ѱ�� ���·� ����� ��
 */