using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public float masterVolume = 1f; // 전체 음량 조절 변수

    public AudioSource[] audioSources; // 오디오 소스 배열

    [Header("Player Audio Clip")]
    public AudioClip[] p_ac;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 모든 AudioSource를 일시 정지
    public void PauseAllAudio()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Pause();
        }
    }

    // 모든 AudioSource를 재생
    public void PlayAllAudio()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Play();
        }
    }

    // 모든 AudioSource의 볼륨 설정
    public void SetAllVolume(float volume)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = volume;
        }
    }

    // 모든 AudioSource의 음소거 설정
    public void MuteAllAudio(bool isMuted)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].mute = isMuted;
        }
    }

    // 특정 AudioSource의 Audio Clip을 교체
    public void ChangeAudioClip(AudioSource audioSource, AudioClip newClip)
    {
        audioSource.clip = newClip;
    }

    // 특정 AudioSource의 AudioClip을 멈춤
    public void StopAudioClip(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    // 특정 AudioSource의 AudioClip을 재생
    public void PlayAudioClip(AudioSource audioSource)
    {
        audioSource.Play();
    }

    private AudioSource FindAudioSourceByClip(AudioClip clip)
    {
        // 해당 오디오 클립을 가지고 있는 오디오 소스 찾기
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].clip == clip)
            {
                return audioSources[i];
            }
        }
        return null;
    }

}
