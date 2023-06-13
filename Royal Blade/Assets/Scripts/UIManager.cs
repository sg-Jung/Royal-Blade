using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [Header("Button")]
    public Button attackBtn;
    public Button shieldBtn;
    public Button runForwardBtn;
    public bool runBtnPressed;
    float runFillAmountSpeed;
    float runMinValue;
    float runMaxValue;


    [Header("Image")]
    public Image attackImage;
    public Image runImage;
    float runImgGage = 0f;


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
        runFillAmountSpeed = PlayerController.Instance.runMinPower * 5;
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
            targetValue = Mathf.Lerp(0, 1, runImgGage / runMaxValue);
            yield return null;
        }

        runImage.fillAmount += Mathf.Clamp01(targetValue);
        PlayerController.Instance.runPower = tempGage;
        Debug.Log(tempGage);
    }

    public void RunBtnReleased()
    {
        runBtnPressed = false;
        PlayerController.Instance.OnClickRunForwardkBtn();
    }

    public void AttackImageFill(float attackGageValue)
    {
        attackImage.fillAmount = Mathf.Clamp01(attackImage.fillAmount + attackGageValue);
    }

}
