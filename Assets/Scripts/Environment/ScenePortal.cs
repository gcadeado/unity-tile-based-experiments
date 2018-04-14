using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox.Audio;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{

    // [SerializeField]
    // private ScenePortal exitPortal;

    private bool isTeleporting = false;

    [SerializeField]
    private float teleportDuration = 0.6f;


    [Header("Audio")]
    [SerializeField]
    private AudioManager audioManager = null;

    [SerializeField]
    private Sound teleportSound = null;

    private AudioSourcePlayer audioPlayer = null;

    private void Awake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            player.SetMoveStatus(false);
            StartCoroutine(Teleport(player, teleportDuration));
        }
    }

    public IEnumerator Teleport(Player player, float teleportationTime)
    {

        //If the teleporter is in use, abort
        if (isTeleporting) yield break;

        //We wait for any other movement coroutines to finish before starting this one.
        while (player.IsMoving)
            yield return null;

        //we prevent the player from moving while teleporting
        player.AddCooldown(teleportationTime);

        //Staircase sound!
        audioPlayer.PlaySound(teleportSound);

        //We set both teleporters as "In Use"
        isTeleporting = true;
        // exitPortal.isTeleporting = true;

        //Fade the player to transparency
        yield return StartCoroutine(player.FadePlayerTo(0f, teleportationTime / 2f));

        //Teleport!
        SceneManager.LoadSceneAsync("TestLevel2");

        //Fade them back to reality
        yield return StartCoroutine(player.FadePlayerTo(1f, teleportationTime / 2f));

        //We set both teleporter as "Available"
        isTeleporting = false;
        // exitPortal.isTeleporting = false;
    }


    private void OnDrawGizmos()
    {
        // if (exitPortal != null)
        // {

        // }
    }
}
