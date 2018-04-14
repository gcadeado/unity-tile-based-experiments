using Cawotte.Toolbox.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public GameObject gameOverCanvas;

    [Header("Audio")]
    [SerializeField]
    private AudioManager audioManager = null;

    [SerializeField]
    private Sound endLevel = null;

    [SerializeField]
    private float timeBeforeStartNextLevel = 0.5f;

    private AudioSourcePlayer audioPlayer = null;
    private void Awake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
    }

    private void Update()
    {
        //Cheat code previous/next levels
        if (Input.GetKeyDown(KeyCode.P))
        {
            NextLevel();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadSceneAsync(scene.name);
            StartCoroutine(ToggleGameOver());
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            doExitGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {

            audioPlayer.PlaySound(endLevel);

            float totalWaitTime = endLevel.clip.length + timeBeforeStartNextLevel;

            player.AddCooldown(totalWaitTime);
            Invoke("NextLevel", totalWaitTime);
        }
    }

    private void NextLevel()
    {
        gameOverCanvas.SetActive(true);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        // SceneManager.LoadSceneAsync("NameOfScene");
    }

    private void doExitGame()
    {
        Debug.Log("Quiting game...");
        Application.Quit();
    }

    IEnumerator ToggleGameOver()
    {
        yield return new WaitForSeconds(1);
        gameOverCanvas.SetActive(false);
    }
}
