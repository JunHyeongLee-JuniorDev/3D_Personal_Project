using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IInitManager
{
    private AudioSource[] audioSources = new AudioSource[(int)Sound.MaxCount];
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        string[] soundNames = System.Enum.GetNames(typeof(Sound));

        Debug.Log(soundNames[0]);
        Debug.Log(soundNames[1]);

        for(int i = 0; i < (int)Sound.MaxCount; i++)
        {
            GameObject newSpeaker = new GameObject(soundNames[i]);
            audioSources[i] = newSpeaker.AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
            newSpeaker.transform.SetParent(transform);
        }

            audioSources[(int)Sound.Effect].outputAudioMixerGroup = Managers.Instance.Data.audioMixer.FindMatchingGroups("SFX")[0];
            audioSources[(int)Sound.Bgm].outputAudioMixerGroup = Managers.Instance.Data.audioMixer.FindMatchingGroups("Music")[0];



        audioSources[(int)Sound.Bgm].loop = true;

        //player foot sound
        GetOrAddAudioClip("Player/FootStep1");
        GetOrAddAudioClip("Player/FootStep2");
        GetOrAddAudioClip("Player/FootStep3");
        GetOrAddAudioClip("Player/FootStep4");

        //player state sound
        GetOrAddAudioClip("Player/PlayerDead");
        GetOrAddAudioClip("Player/Thunder");
        GetOrAddAudioClip("Player/PowerUp");
        GetOrAddAudioClip("Player/SwordSlash");

        //Monsters
        GetOrAddAudioClip("Monster/BookHeadSkill");
        GetOrAddAudioClip("Monster/BossStep");
        GetOrAddAudioClip("Monster/BossRunCrash");
        GetOrAddAudioClip("Monster/CrabStep");
        GetOrAddAudioClip("Monster/MinoDead");

        //UI
        GetOrAddAudioClip("UI/MenuClick");
        GetOrAddAudioClip("UI/MenuOpen");
        GetOrAddAudioClip("UI/MenuClose");

        //ETC
        GetOrAddAudioClip("HitSound");
        GetOrAddAudioClip("Environment/Door");
        GetOrAddAudioClip("Environment/FireLight");

        //BGM
        GetOrAddAudioClip("Bgm/Title");
        GetOrAddAudioClip("Bgm/Game");
        GetOrAddAudioClip("Bgm/Boss");
        GetOrAddAudioClip("Bgm/YouDied");

        Play(GetOrAddAudioClip("Bgm/Title", Sound.Bgm), Sound.Bgm);
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
        //audioClips.Clear();
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

        Debug.Log("오디오 이름 : " + audioSource.gameObject.name);
    }

    public void StopSFXSound()
    {
        audioSources[(int)Sound.Effect].Stop();
    }

    public void Play3DSound(string path, Vector3 position , float pitch = 1.0f)
    {
        AudioSource.PlayClipAtPoint(GetOrAddAudioClip(path), position);
    }

    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
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