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
    [SerializeField] private Image mpBar;                // ���� ������ UI
    [SerializeField] private PlayerBase player;        // �÷��̾� ������Ʈ
    PlayerBase playerBase;  // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ
    private void Awake() {
        PlayerInitialize(); // ������Ʈ ����
        if (player.GetCurChoice(PlayerChoice.P1)) 
        { 
            GameObject obj = GameObject.Find("P1_ManaGauge");
            if (obj) mpBar = obj.GetComponent<Image>();
        }
        else if (player.GetCurChoice(PlayerChoice.P2))
        {
            GameObject obj = GameObject.Find("P2_ManaGauge");
            if (obj) mpBar = obj.GetComponent<Image>();
        }
        else { print(gameObject + "�� Player�Ҵ��� �Ǿ����� �ʽ��ϴ�."); }
    }

    private void PlayerInitialize()
    {
        player = gameObject.GetComponent<PlayerBase>();
    }

    private void Update()  // �÷��̾��� �������� ������ �������� ���� ����UI�� ä��Ⱚ ����
    {
        if (mpBar != null) // ���� mpBar �̹����� �Ҵ�Ǿ��ٸ�
            mpBar.fillAmount = ((float)curMana / (float)maxMana); // hpBar fillAmount�� ����
        
        if (curMana < 0) { curMana = 0; }
        else if (curMana > maxMana) { curMana = maxMana; }

    }
    public void MpCharge(int chargeMana)
    {
        Debug.Log("MP ����, ���� ����: " + curMana);
        curMana += chargeMana; // �÷��̾� ��ũ��Ʈ�� ������ �߰�
    }
    public int GetCurMana() { return curMana; }
    public int GetMaxMana() { return maxMana; }
}
