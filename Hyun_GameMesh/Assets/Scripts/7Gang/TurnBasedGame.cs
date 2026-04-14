using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnBasedGame : MonoBehaviour
{
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;
    [SerializeField] float baseRareChance = 0.2f; 
    [SerializeField] float rareChanceIncreasePerTurn = 0.05f; 

    [SerializeField] TextMeshProUGUI resultUI; 

    // 통계 변수
    int turn = 0;
    bool rareItemObtained = false;
    float currentRareChance;

    int totalEnemiesSpawned = 0;
    int totalEnemiesKilled = 0;
    int totalAttackAttempts = 0;
    int totalSuccessfulHits = 0;
    int totalCriticalHits = 0;

    float maxDamageInfo = float.MinValue;
    float minDamageInfo = float.MaxValue;

    // 아이템 카운트
    int countPotion = 0;
    int countGold = 0;
    int countNormalWeapon = 0;
    int countRareWeapon = 0;
    int countNormalArmor = 0;
    int countRareArmor = 0;

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    void Start()
    {
        ResetStatistics();
        UpdateUI();
    }


    public void StartSimulation()   // 레어 아이템이 나올 때까지 한 번에 시뮬레이션
    {
        ResetStatistics();

        // 기하분포 샘플링: 레어 아이템이 나올 때까지 반복
        while (!rareItemObtained)
        {
            turn++;
            SimulateTurn();
            currentRareChance = Mathf.Min(1f, currentRareChance + rareChanceIncreasePerTurn);
        }

        Debug.Log($"자동 시뮬레이션 종료: 레어 아이템을 {turn} 턴에 획득했습니다.");
        UpdateUI();
    }

    
    public void SimulateSingleTurn()    // 한 번 클릭할 때마다 1턴씩만 진행
    {
        if (rareItemObtained || turn == 0)
        {
            ResetStatistics();
        }

        turn++;
        SimulateTurn();

        // 확률 100%까지만 제한
        currentRareChance = Mathf.Min(1f, currentRareChance + rareChanceIncreasePerTurn);

        UpdateUI();

        if (rareItemObtained)
        {
            Debug.Log($"축하합니다! {turn} 턴 만에 레어 아이템을 획득했습니다. 다음 클릭 시 처음부터 다시 시작합니다.");
        }
    }

    void ResetStatistics()
    {
        rareItemObtained = false;
        turn = 0;
        currentRareChance = baseRareChance;

        totalEnemiesSpawned = 0;
        totalEnemiesKilled = 0;
        totalAttackAttempts = 0;
        totalSuccessfulHits = 0;
        totalCriticalHits = 0;

        maxDamageInfo = float.MinValue;
        minDamageInfo = float.MaxValue;

        countPotion = 0;
        countGold = 0;
        countNormalWeapon = 0;
        countRareWeapon = 0;
        countNormalArmor = 0;
        countRareArmor = 0;
    }

    void SimulateTurn()
    {
        // 푸아송 분포: 적 등장 수
        int enemyCount = SamplePoisson(poissonLambda);
        totalEnemiesSpawned += enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            // 이항 분포: 명중 횟수 (최대 5번 시도 중 성공 횟수)
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            totalAttackAttempts += maxHitsPerTurn; // 공격 시도 횟수 누적
            totalSuccessfulHits += hits;           // 명중 횟수 누적

            float totalDamage = 0f;

            for (int j = 0; j < hits; j++)
            {
                // 정규 분포: 데미지
                float damage = SampleNormal(meanDamage, stdDevDamage);

                // 베르누이 분포: 치명타 발생 여부
                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    totalCriticalHits++; // 치명타 횟수 누적
                }

                // 최대/최소 데미지 갱신
                if (damage > maxDamageInfo) maxDamageInfo = damage;
                if (damage < minDamageInfo) minDamageInfo = damage;

                totalDamage += damage;
            }

            // 적 처치 여부 확인
            if (totalDamage >= enemyHP)
            {
                totalEnemiesKilled++;
                DistributeReward();
            }
        }
    }

    void DistributeReward()
    {
        // 균등 분포: 보상 결정
        string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];

        switch (reward)
        {
            case "Potion":
                countPotion++;
                break;
            case "Gold":
                countGold++;
                break;
            case "Weapon":
                if (Random.value < currentRareChance)
                {
                    countRareWeapon++;
                    rareItemObtained = true;
                }
                else countNormalWeapon++;
                break;
            case "Armor":
                if (Random.value < currentRareChance)
                {
                    countRareArmor++;
                    rareItemObtained = true;
                }
                else countNormalArmor++;
                break;
        }
    }

    void UpdateUI()
    {
        if (resultUI == null)
        {
            return;
        }

        // 확률 계산
        float hitPercentage = (totalAttackAttempts > 0) ? ((float)totalSuccessfulHits / totalAttackAttempts) * 100f : 0f;
        float critPercentage = (totalSuccessfulHits > 0) ? ((float)totalCriticalHits / totalSuccessfulHits) * 100f : 0f;

        // 데미지 출력 예외 처리 (공격을 한 번도 맞추지 못한 경우)
        float displayMinDmg = minDamageInfo == float.MaxValue ? 0f : minDamageInfo;
        float displayMaxDmg = maxDamageInfo == float.MinValue ? 0f : maxDamageInfo;

        // 상태 메시지 결정
        string statusMessage = rareItemObtained ? "레어 아이템 획득! (다음 클릭 시 초기화)</color>" : "탐험 진행 중...";

        string resultText =
            $"전투 결과\n" +
            $"상태: {statusMessage}\n\n" +
            $"총 진행 턴 수 : {turn}\n" +
            $"현재 레어 획득 확률 : {(currentRareChance * 100f):F0}%\n" +
            $"발생한 적 : {totalEnemiesSpawned}\n" +
            $"처치한 적 : {totalEnemiesKilled}\n" +
            $"공격 명중 결과 : {hitPercentage:F2}%\n" +
            $"발생한 치명타율 결과 : {critPercentage:F2}%\n" +
            $"최대 데미지 : {displayMaxDmg:F2}\n" +
            $"최소 데미지 : {displayMinDmg:F2}\n\n\n" +

            $"획득한 아이템\n\n" +
            $"포션 : {countPotion}개\n" +
            $"골드 : {countGold}개\n" +
            $"무기 - 일반 : {countNormalWeapon}개\n" +
            $"무기 - 레어 : {countRareWeapon}개\n" +
            $"방어구 - 일반 : {countNormalArmor}개\n" +
            $"방어구 - 레어 : {countRareArmor}개";

        resultUI.text = resultText;
    }

    // --- 분포 샘플 함수들 ---
    int SamplePoisson(float lambda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++)
            if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }
}