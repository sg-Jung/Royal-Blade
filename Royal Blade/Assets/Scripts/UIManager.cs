using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [Header("Top")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI infoText;

    [Header("Canvas")]
    public GameObject gamePlayCanvas;
    public GameObject settingCanvas;

    [Header("Button")]
    public Button attackBtn;
    public Button attackSkillBtn;
    public Button shieldBtn;
    public Button runForwardBtn;
    public Button runSkillBtn;
    public bool runBtnPressed;
    float runFillAmountSpeed;
    float runMinValue;
    float runMaxValue;

    [Header("Text")]
    public TextMeshProUGUI settingScoreText;

    [Header("Image")]
    public Image shieldImage;
    public Image attackSkillImage;
    public Image runSkillImage;
    float runImgGage = 0f;

    [Header("Setting")]
    public float runFillSpeed;
    public float runFillRatio;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        runMinValue = PlayerController.Instance.runMinPower;
        runMaxValue = PlayerController.Instance.runMaxPower;
        runFillAmountSpeed = PlayerController.Instance.runMinPower * runFillSpeed;
        
    }

    private void Update()
    {
        runForwardBtn.interactable = !PlayerController.Instance.isRun;

        ManageSkillBtn();
    }

    void ManageSkillBtn()
    {
        if (attackSkillImage.fillAmount == 1)
        {
            attackSkillBtn.interactable = true;
        }
        else
        {
            attackSkillBtn.interactable = false;
        }

        if (runSkillImage.fillAmount == 1)
        {
            runSkillBtn.interactable = true;
        }
        else
        {
            runSkillBtn.interactable = false;
        }
    }


    public void ReStartBtnClicked()
    {
        Time.timeScale = 1f;
        Destroy(Manager.Instance.gameObject);
        Destroy(PlayerController.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SettingBtnClicked()
    {
        Time.timeScale = 0f;
        settingScoreText.text = GameManager.Instance.curScore.ToString();
        gamePlayCanvas.SetActive(false);
        settingCanvas.SetActive(true);
    }

    public void QuitBtnClicked()
    {
        Application.Quit();
    }

    public void RunBtnPressed()
    {
        runBtnPressed = true;
        StartCoroutine(RunBtnPressedCor());
    }

    IEnumerator RunBtnPressedCor()
    {
        float targetValue = 0f;
        float tempGage = 0f;
        while (runBtnPressed)
        {
            tempGage += runFillAmountSpeed * Time.deltaTime;
            runImgGage = Mathf.Clamp(tempGage, runMinValue, runMaxValue);
            targetValue = Mathf.Lerp(0, 1, (runImgGage / runMaxValue) / runFillRatio);
            yield return null;
        }
        if (PlayerController.Instance.isGround)
            runSkillImage.fillAmount += Mathf.Clamp01(targetValue);
    }

    public void RunBtnReleased()
    {
        runBtnPressed = false;

        if (PlayerController.Instance.isGround)
        {
            PlayerController.Instance.runPower = runImgGage;
            PlayerController.Instance.OnClickRunForwardkBtn();
        }
    }

    public void RunSkillBtnClicked()
    {
        PlayerController.Instance.OnClickRunSkillBtn();
        runSkillImage.fillAmount = 0f;
    }

    public void AttackSkillBtnClicked()
    {
        PlayerController.Instance.OnClickAttackSkillBtn();
        StartCoroutine(AttackSkillLossAmount());
        attackSkillBtn.interactable = false;
    }

    IEnumerator AttackSkillLossAmount()
    {
        float wholeDuration = PlayerController.Instance.wholeAttackSkillDuration;
        float time = wholeDuration;

        Image attackImg = attackBtn.GetComponent<Image>();
        
        Color orginColor = attackImg.color;
        attackImg.color = Color.blue;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            float fillAmount = Mathf.Lerp(0f, 1f, time / wholeDuration);
            attackSkillImage.fillAmount = fillAmount;

            yield return null;
        }

        attackImg.color = orginColor;
    }

    public void ShieldBtnClicked()
    {
        float coolTime = PlayerController.Instance.shieldCoolTime;
        shieldImage.fillAmount = 0f;
        shieldBtn.interactable = false;

        StartCoroutine(ShieldBtnFillCor(coolTime));
        PlayerController.Instance.OnClickShieldkBtn();
    }

    IEnumerator ShieldBtnFillCor(float coolTime)
    {
        float time = 0f;

        while(coolTime > time)
        {
            time += Time.deltaTime;
            float fillAmount = Mathf.Lerp(0f, 1f, time / coolTime);
            shieldImage.fillAmount = fillAmount;

            yield return null;
        }

        shieldBtn.interactable = true;
    }

    public void AttackSkillImageFill(float attackGageValue)
    {
        attackSkillImage.fillAmount = Mathf.Clamp01(attackSkillImage.fillAmount + attackGageValue);
        
        if (attackSkillImage.fillAmount == 1) attackSkillBtn.interactable = true;
    }

    public void ChangeScoreText()
    {
        scoreText.text = GameManager.Instance.curScore.ToString();
    }

    public void ChangeMoneyText()
    {
        moneyText.text = GameManager.Instance.curMoney.ToString();
    }
}