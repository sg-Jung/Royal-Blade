using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

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

    [Header("Image")]
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
        shieldBtn.interactable = !PlayerController.Instance.isShield;

        ManageSkillBtn();
    }

    void ManageSkillBtn()
    {
        if (attackSkillImage.fillAmount == 1) attackSkillBtn.interactable = true;
        else attackSkillBtn.interactable = false;

        if (runSkillImage.fillAmount == 1) runSkillBtn.interactable = true;
        else runSkillBtn.interactable = false;
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

    }

    public void AttackImageFill(float attackGageValue)
    {
        attackSkillImage.fillAmount = Mathf.Clamp01(attackSkillImage.fillAmount + attackGageValue);
    }

}