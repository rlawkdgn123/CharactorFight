using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �ۼ��� : ������
[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData", order = int.MaxValue)]
public class SkillData : ScriptableObject
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
    public int attackSpeed; // ���ݼӵ�
    public float range; // ���� ��Ÿ�
    [Header("���� �� ����")]
    public bool spawnEffect;
    public float spawnDelay; // ���� ����
    public float duration; // ��ų ���ӽð�
    public Vector2 knockback; // �˹� �Ÿ�
    public float spawnPosx = 2; // ���� ��ġ X
    public float spawnPosy; // ���� ��ġ Y
    public float motionTime; // ��ų ��� �ð�
    [Header("��ų �߻�")]
    public bool skillFire; // ��ų ������ �߻� ����
    public bool skillPenetrate; // �߻�ü ���� ����
    public bool skillGrowing; // ���ӽð� ���� ��ų ũ�� Ŀ���� ����
    public float skillGrowingSpeed; // Ŀ���� �ӵ�
    public bool skillFaster; // ���ӽð� ���� ��ų �ӵ� �������� ����
    public float skillSpeed; // �߻�ü �� ��ȯü �̵��ӵ�
    public bool skillAddSpeed; // ���ӽð� ���� �������� �̵��ӵ� ���ϱⰪ
    public Vector3 skillMaxSize; // ��ų Ŀ�� �� �ִ� ������
    [Header("��Ÿ")]
    public GameObject spawnEffectPrefab; // ���� ������
    public GameObject projectilePrefab; // ��ų ������
    public GameObject explosionPrefab; // ��ų ���� ������
    public bool freezePlayerPos; // ��ų ��� �� �÷��̾� ���� ����
    public AudioClip skillSpawnAudioSource; // ��ų ���� �� ȿ���� �迭
    public AudioClip skillDestroyAudioSource; // ��ų �Ҹ� �� ȿ���� �迭
}
