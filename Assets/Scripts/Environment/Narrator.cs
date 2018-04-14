
using Cawotte.Toolbox.Audio;
using System.Collections.Generic;
using UnityEngine;

public class Narrator : Interactable
{

    [SerializeField]
    private bool isSwitchedOn = false;

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


    DialogueTrigger dialogTrigger;

    private void Awake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
        dialogTrigger = gameObject.GetComponent<DialogueTrigger>();

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
        OpenDialogBox(); //Open dialog, then return false because a lever is an obstacle.
        // DO STUFF
        return false;
    }

    public void OpenDialogBox()
    {

        isSwitchedOn = !isSwitchedOn;

        UpdateSprite();

        audioPlayer.PlaySound(leverSound);

        dialogTrigger.TriggerDialogue();

    }

    private void UpdateSprite()
    {

        if (isSwitchedOn)
            GetComponent<SpriteRenderer>().sprite = switchedOnSprite;
        else
            GetComponent<SpriteRenderer>().sprite = switchedOffSprite;
    }

}
