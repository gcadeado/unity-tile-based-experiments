using Cawotte.Toolbox.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimpleDoor : Door
{
    [Header("Audio")]
    [SerializeField]
    private AudioManager audioManager = null;

    [SerializeField]
    private Sound openingDoor = null;

    private AudioSourcePlayer audioPlayer = null;

    private void Awake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
    }


    public override bool CanMoveTo(Player player)
    {
        if (isOpen)
        {
            return true;
        }
        else
        {
            Open();
            return true;
        }
    }

    private void Open()
    {
        IsOpen = true;

        audioPlayer.PlaySound(openingDoor);
    }

}
