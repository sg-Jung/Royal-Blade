using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Header("Game Info")]
    public int curScore;
    public int curMoney;
    public float stopTime; // 스킬 사용 시 게임이 잠깐 멈추는 시간

    [Header("Health")]
    public List<Image> hearts;
    public int health;
    public int maxHealth;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for(int i = 0; i < health; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }

    }

    public void AddHeart()
    {
        if (health >= maxHealth) return;

        health++;
        hearts[health - 1].gameObject.SetActive(true);
    }

    public void LossHeart()
    {
        health--;
        hearts[health].gameObject.SetActive(false);

        if(health <= 0)
            GameEnd();
    }

    void GameEnd()
    {
        UIManager.Instance.SettingBtnClicked();
    }

    public void AddScore(int score)
    {
        curScore += score;
        UIManager.Instance.ChangeScoreText();
    }

    public void AddMoney(int money)
    {
        curMoney += money;
        UIManager.Instance.ChangeMoneyText();
    }

    public void StopGameForSkill()
    {
        StartCoroutine(StopGameForSkillCor());
    }

    IEnumerator StopGameForSkillCor()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(stopTime);
        Time.timeScale = 1f;
    }
}
