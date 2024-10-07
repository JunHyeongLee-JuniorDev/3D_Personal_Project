using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour, IInitManager
{
    private AudioSource[] audioSources = new AudioSource[(int)Sound.MaxCount];
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        string[] soundNames = System.Enum.GetNames(typeof(Sound));

        for(int i = 0; i < soundNames.Length; i++)
        {
            GameObject newSpeaker = new GameObject(soundNames[i]);
            audioSources[i] = newSpeaker.AddComponent<AudioSource>();
            newSpeaker.transform.SetParent(transform);
        }

        audioSources[(int)Sound.Bgm].loop = true;
    }

    /// <summary>
    /// 계속 씬에 남아있기에 필요 없는 사운드를 비워줄 필요가 있다
    /// </summary>
    public void ClearSound()
    {
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        audioClips.Clear();
    }

    public void Clear()
    {
        ClearSound();
    }

    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        AudioSource audioSource = audioSources[(int)type];
        audioSource.pitch = pitch;

        if (type == Sound.Bgm)
        {
            if(audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
        }

        else
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Play3DSound(string path, Vector3 position , float pitch = 1.0f)
    {
        AudioSource.PlayClipAtPoint(GetOrAddAudioClip(path), position);
    }

    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
    }

    public AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Sound.Bgm)
        {
            audioClip = Resources.Load<AudioClip>(path);
        }

        else
        {
            if(audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                audioClips.Add(path, audioClip);
            }
        } 

        if(audioClip == null)
            Debug.LogWarning($"Audio Clip Missing : {path}");

        return audioClip;
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount
    }
}