using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;

public class SoundManager : Singleton<SoundManager>
{
    public List<ISoundOptionButton> optionButtons;

    public List<AudioClip> bgmList;
    public List<AudioClip> seList;

    public AudioSource bgmPlayer;

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
    }

    public void PlayBgm(string name)
    {
        if(bgm.ContainsKey(name) == false)
        {
            return;
        }

        if(bgmPlayer != null)
        {
            if(bgmPlayer.isPlaying)
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

    public void PlaySe(AudioSource player, string name)
    {
        if(se.ContainsKey(name) == false)
        {
            return;
        }

        player.PlayOneShot(se[name]);
    }

    public void PlaySe(AudioSource player, int index)
    {
        PlaySe(player, seList[index].name);
    }
}
