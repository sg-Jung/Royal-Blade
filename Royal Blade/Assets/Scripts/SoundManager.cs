using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public float masterVolume = 1f; // ��ü ���� ���� ����

    public AudioSource[] audioSources; // ����� �ҽ� �迭

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

    // ��� AudioSource�� �Ͻ� ����
    public void PauseAllAudio()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Pause();
        }
    }

    // ��� AudioSource�� ���
    public void PlayAllAudio()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Play();
        }
    }

    // ��� AudioSource�� ���� ����
    public void SetAllVolume(float volume)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = volume;
        }
    }

    // ��� AudioSource�� ���Ұ� ����
    public void MuteAllAudio(bool isMuted)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].mute = isMuted;
        }
    }

    // Ư�� AudioSource�� Audio Clip�� ��ü
    public void ChangeAudioClip(AudioSource audioSource, AudioClip newClip)
    {
        audioSource.clip = newClip;
    }

    // Ư�� AudioSource�� AudioClip�� ����
    public void StopAudioClip(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    // Ư�� AudioSource�� AudioClip�� ���
    public void PlayAudioClip(AudioSource audioSource)
    {
        audioSource.Play();
    }

    private AudioSource FindAudioSourceByClip(AudioClip clip)
    {
        // �ش� ����� Ŭ���� ������ �ִ� ����� �ҽ� ã��
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
