using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;

public class SoundManager : Singleton<SoundManager>
{
    public List<AudioClip> bgmList;
    public List<AudioClip> seList;

    public AudioSource bgmPlayer;
    public AudioSource sePlayer = null;

    private Dictionary<string, AudioClip> bgm;
    private Dictionary<string, AudioClip> se;

    public bool muteBgm;
    public bool muteSe;

    private void Awake()
    {
        bgm = new Dictionary<string, AudioClip>();

        foreach (var item in bgmList)
        {
            bgm.Add(item.name, item);
        }

        se = new Dictionary<string, AudioClip>();

        foreach (var item in seList)
        {
            se.Add(item.name, item);
        }
    }

    public void MuteBgm()
    {
        muteBgm = !muteBgm;

        if(bgmPlayer != null)
        {
            bgmPlayer.mute = muteBgm;
        }
    }

    public void MuteSe()
    {
        muteSe = !muteSe;
    }

    public void PlayBgm(string name)
    {
        if(bgm.ContainsKey(name) == false)
        {
            return;
        }

        if(bgmPlayer != null)
        {
            bgmPlayer.mute = muteBgm;

            if (bgmPlayer.isPlaying)
            {
                bgmPlayer.Stop();
            }

            bgmPlayer.clip = bgm[name];
            bgmPlayer.Play();
        }
    }

    public void PlayBgm(int index)
    {
        PlayBgm(bgmList[index].name);
    }

    public void PlaySe(string name)
    {
        if (se == null || sePlayer == null || se.ContainsKey(name) == false || muteSe)
        {
            return;
        }

        sePlayer.PlayOneShot(se[name]);
    }

    public void PlaySe(int index)
    {
        PlaySe(seList[index].name);
    }
}
