using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox.Audio;

/// <summary>
/// Scripts that handle the Player character
/// </summary>

public class Player : MonoBehaviour
{


    [Header("References")]

    [SerializeField]
    private MapGlobalReference mapGlobalReference = null;

    private Map map = null;

    private Coroutine bubbleRoutine;

    private AudioSourcePlayer audioPlayer = null;

    [Header("ScriptableObject Variables")]

    [SerializeField]
    private IntVariable keyCount = null;


    [Header("Movements")]

    [SerializeField]
    [ReadOnly]
    private bool isMoving = false;

    [SerializeField]
    [ReadOnly]
    /* Has long as cooldown is not equal to 0, the player movements are disabled.
    This allow to keep movements disabled if several function triggers a cooldown
    that would end on different times*/
    private int cooldownCount = 0;

    [SerializeField]
    private bool canMove = true;

    [SerializeField]
    private float moveTime = 0.1f;

    [SerializeField]
    private float cooldownMovement = 0.1f;

    [Header("Audio")]
    [SerializeField]
    private AudioManager audioManager = null;

    [SerializeField]
    private Sound grassStep = null;

    [SerializeField]
    private Sound blockedStep = null;

    private Animator animator = null;

    private SpriteRenderer sr = null;

    public GameObject speechBubble = null;

    public int KeyCount
    {
        get
        {
            return keyCount.Value;
        }
        set
        {
            keyCount.Value = value;
        }
    }

    public bool HasCooldown
    {
        get
        {
            return cooldownCount > 0;
        }
    }

    public bool IsMoving { get => isMoving; }

    //Screen.SetResolution(1280, 600, false);

    private void Awake()
    {
        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        keyCount.Value = 0;
    }

    private void Start()
    {
        map = mapGlobalReference.Map;
    }

    // Update is called once per frame
    void Update()
    {
        //We do nothing if the player is still moving.
        if (isMoving || HasCooldown || !canMove) return;


        //To get move directions
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //We can't go in both directions at the same time
        animator.SetBool("isMovingUp", false);
        if (horizontal != 0f)
        {
            vertical = 0f;
            if (horizontal < 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
        else if (vertical > 0)
        {
            animator.SetBool("isMovingUp", true);
        }

        //If there's a direction, we try to move.
        if (horizontal != 0 || vertical != 0)
        {
            Move(new Vector2(horizontal, vertical));
        }

    }

    public void AddCooldown(float duration)
    {
        StartCoroutine(_AddCooldown(duration));
    }

    public void SetMoveStatus(bool canMoveStatus)
    {
        canMove = canMoveStatus;
    }

    private void Move(Vector2 direction)
    {


        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + direction.normalized;

        //Huge function that verify if the player can move to the next tile.
        bool canWalk = map.CanMoveFromTo(this, transform.position, endPos);

        // bool hasBridgeTile = map.IsBridge(endPos); //if target Tile has a bridge (plank)

        //If the player is allowed to move to the next tile.
        if (canWalk)
        {
            //Play sound depending on the kind of ground
            // if (hasBridgeTile)
            // {
            //     audioPlayer.PlaySound(bridgeStep);
            // } else
            // {
            audioPlayer.PlaySound(grassStep);
            // }

            animator.SetBool("isRunning", true);
            StartCoroutine(MoveTo(endPos, moveTime, cooldownMovement));
        }
        else
        {

            StartCoroutine(BlockedMovement(endPos));
        }

    }

    private IEnumerator MoveTo(Vector3 end, float moveTime, float cooldown)
    {
        //Can't take another action while moving
        isMoving = true;

        float timer = 0f;
        Vector3 startingPos = transform.position;

        while (timer < moveTime)
        {
            yield return null;

            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startingPos, end, timer / moveTime);

        }

        //Be sure that the player snaps to the end position.
        transform.position = end;

        isMoving = false;

        animator.SetBool("isRunning", false);

        //If there's a cooldown, we keep movement disabled for an extra time.
        if (cooldown > 0f)
        {
            AddCooldown(cooldownMovement);
        }

        if (map.IsVoid(end) && !map.IsGround(end))
        {
            if (this.bubbleRoutine != null)
                StopCoroutine(this.bubbleRoutine);

            this.bubbleRoutine = StartCoroutine(ShowBubble(2));
        }

    }

    private IEnumerator ShowBubble(float time)
    {
        var instruction = new WaitForEndOfFrame();
        speechBubble.SetActive(true);
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return instruction;
        }
        speechBubble.SetActive(false);
    }

    //Blocked animation
    private IEnumerator BlockedMovement(Vector3 end)
    {

        //Play the blocked sound only if there's no lever, no negative feedback.
        if (!map.HasLever(end))
        {
            audioPlayer.PlaySound(blockedStep);
        }

        Vector3 originalPos = transform.position;
        Vector3 blockedEnd = transform.position + ((end - transform.position) / 3);

        //We move normally to one-third of the way to the next tile, then return to our original pos.

        yield return MoveTo(blockedEnd, moveTime / 2f, 0f);

        //The next yield won't start before the previous coroutine has ended.

        yield return MoveTo(originalPos, moveTime / 2f, cooldownMovement);

    }

    public IEnumerator FadePlayerTo(float alpha, float durationFading)
    {

        float startingAlpha = sr.color.a;
        float timer = 0f;

        while (timer < durationFading)
        {
            float newAlpha = Mathf.Lerp(startingAlpha, alpha, timer / durationFading);

            //Set new alpha
            SetAlphaSprite(newAlpha);

            timer += Time.deltaTime;
            yield return null;
        }

        //Snap to final alpha
        SetAlphaSprite(alpha);
    }

    /// <summary>
    /// Add a cooldown for the given duration. As long as cooldown is not equal to 0,
    /// the player can't take a new action.
    /// Useful because several function may trigger cooldowns at different times, and it
    /// allows to avoid race errors. (EndMovement + Teleporting in this case)
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator _AddCooldown(float duration)
    {
        cooldownCount++;

        yield return new WaitForSeconds(duration);

        cooldownCount--;
    }

    private void SetAlphaSprite(float alpha)
    {
        Color newColor = sr.color;
        newColor.a = alpha;
        sr.color = newColor;
    }

}
