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
    private Sound mainMusic;

    [SerializeField]
    private Sound ambientMusic;

    private AudioSourcePlayer audioPlayer;

    protected override void OnAwake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
    }

    void Start()
    {
        audioPlayer.PlaySound(mainMusic);
        audioPlayer.PlaySound(ambientMusic);
    }

}
