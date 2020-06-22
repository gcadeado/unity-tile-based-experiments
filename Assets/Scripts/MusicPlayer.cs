using Cawotte.Toolbox;
using Cawotte.Toolbox.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : Singleton<MusicPlayer>
{

    [SerializeField]
    protected AudioManager audioManager;

    [SerializeField]
    private Sound mainMusic = null;

    [SerializeField]
    private Sound ambientMusic = null;

    private AudioSourcePlayer audioPlayer;

    protected override void OnAwake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
    }

    void Start()
    {
        if (mainMusic)
        {
            audioPlayer.PlaySound(mainMusic);
        }
        if (ambientMusic)
        {
            audioPlayer.PlaySound(ambientMusic);
        }
    }

}
