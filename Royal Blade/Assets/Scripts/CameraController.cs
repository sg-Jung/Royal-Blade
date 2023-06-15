using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public static CameraController Instance { get { return instance; } }

    [Header("Camera Setting")]
    public Camera mainCamera;
    public Color skillBGColor;
    public float magnitude;
    public bool isChangeBG;

    private float duration;
    Vector3 startPos;

    private void Awake()
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

    void Start()
    {
        startPos = transform.localPosition;
    }

   public void CameraShake(float duration)
    {
        StartCoroutine(Shake(duration));
    }

    IEnumerator Shake(float duration)
    {
        float timer = 0;
        while (duration >= timer)
        {
            transform.localPosition = (Vector3)Random.insideUnitSphere * magnitude + startPos;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startPos;
    }

    void StartCameraBackGroundForSkill(float skillDuration)
    {
        duration = skillDuration;
        mainCamera.backgroundColor = skillBGColor;
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        isChangeBG = true;
    }

    void EndCameraBackGroundForSkill()
    {
        mainCamera.clearFlags = CameraClearFlags.Skybox;
        isChangeBG = false;
    }

    IEnumerator ChangeCameraBackGroundForSkillCor()
    {
        yield return new WaitForSeconds(duration);
        EndCameraBackGroundForSkill();
    }

    public void ChangeCameraBackGroundForSkill(float skillDuration)
    {
        StartCameraBackGroundForSkill(skillDuration);
        StartCoroutine(ChangeCameraBackGroundForSkillCor());
    }

}
