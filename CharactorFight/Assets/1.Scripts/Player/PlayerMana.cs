using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static PlayerBase;
// �ۼ��� : ������
public class PlayerMana : MonoBehaviour
{
    [SerializeField] private int curMana;
    [SerializeField] private int maxMana;
    [SerializeField] private Slider mpBar;                // ���� ������ UI
    [SerializeField] private PlayerBase player;        // �÷��̾� ������Ʈ
    [SerializeField] private GameObject ui;
    PlayerBase playerBase;  // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ
    private void Start()
    {
        PlayerInitialize(); // ������Ʈ ����
    }

    private void PlayerInitialize()
    {
        player = gameObject.GetComponent<PlayerBase>();
        ui = player.GetPlayerUI();

        string barName = "";
        if (player.GetCurChoice(PlayerBase.PlayerChoice.P1)) { barName = "P1_MPBar"; }
        else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2)) { barName = "P2_MPBar"; }

        Transform tr = ui.transform.Find(barName);
        if (tr != null)
        {
            mpBar = tr.gameObject.GetComponentInChildren<Slider>();
            if (mpBar == null) { print("�����̴��� �������� �ʽ��ϴ�."); }
        }
        else { print(barName + "�� ã�� �� �����ϴ�."); }

    }

    private void Update()  // �÷��̾��� �������� ������ �������� ���� ����UI�� ä��Ⱚ ����
    {
        if (mpBar != null) // ���� hpBar �̹����� �Ҵ�Ǿ��ٸ�
            mpBar.value = ((float)curMana / (float)maxMana); // hpBar fillAmount�� ����

        if (curMana < 0) { curMana = 0; }
        else if (curMana > maxMana) { curMana = maxMana; }

    }
    public void MpCharge(int chargeMana)
    {
        Debug.Log("MP ����, ���� ����: " + curMana);
        curMana += chargeMana; // �÷��̾� ��ũ��Ʈ�� ������ �߰�
    }
    public void SetCurMana(int mana) { curMana = mana; }
    public void AddCurMana(int mana) { curMana += mana; }
    public void SubCurMana(int mana) { curMana -= mana; }
    public int GetCurMana() { return curMana; }
    public int GetMaxMana() { return maxMana; }
}
