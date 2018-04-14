﻿
using Cawotte.Toolbox.Audio;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interactable
{

    [SerializeField]
    private bool isSwitchedOn = false;

    [SerializeField]
    private List<LeveredDoor> doorList = new List<LeveredDoor>();

    [SerializeField]
    private Sprite switchedOnSprite = null;

    [SerializeField]
    private Sprite switchedOffSprite = null;

    [Header("Audio")]
    [SerializeField]
    private AudioManager audioManager = null;

    [SerializeField]
    private Sound leverSound = null;

    private AudioSourcePlayer audioPlayer = null;


    private void Awake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
    }

    //Update sprite when modifying the settings in EditMode
    private void OnValidate()
    {
        UpdateSprite();
    }
    // Use this for initialization
    void Start()
    {

        UpdateSprite();

    }

    public override bool CanMoveTo(Player player)
    {
        PressPlate(); //Switch the lever, then return false because a lever is an obstacle.
        return true;
    }

    public void PressPlate()
    {
        isSwitchedOn = !isSwitchedOn;

        UpdateSprite();

        //Switch levers
        foreach (LeveredDoor door in doorList)
        {
            door.SwitchOpenClose();
        }

        audioPlayer.PlaySound(leverSound);

    }

    private void UpdateSprite()
    {

        if (isSwitchedOn)
            GetComponent<SpriteRenderer>().sprite = switchedOnSprite;
        else
            GetComponent<SpriteRenderer>().sprite = switchedOffSprite;
    }

    //Draw line to all doors it triggers
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        foreach (LeveredDoor door in doorList)
        {
            Gizmos.DrawLine(transform.position, door.transform.position);
        }
    }

}