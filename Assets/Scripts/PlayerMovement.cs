using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    //character tuning;
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashLength;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float attackAnimLength = 0.05f;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float dashCooldown;
    [SerializeField] private ParticleSystem dashParticles;
    [SerializeField] private GameObject lanternPrefab;
    [SerializeField] private GameObject groundChannelVFXPrefab;
    [SerializeField] private CanvasGroup deathScreen;
    [SerializeField] private AudioSource attackSFX;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource dashSFX;
    [SerializeField] private AudioSource walkSFX;
    [SerializeField] private AudioSource bgmSFX;
    [SerializeField] private AudioSource bossMusic;
    //power up variables
    private float numberOfJumps = 1; //temporary for testing
    private bool hasDash;
    private bool hasGroundChannel;
    private bool canMove = true;

    //components to manipulate
    private Rigidbody2D rb;
    private Animator animator;

    //utility variables
    private float axis;
    private float axisLast;
    private bool isAttacking;
    private bool isGrounded;
    private bool isDashing;
    private float coyoteTimer;
    private int currConsJumps;
    private float gravity;
    private Health health;
    private bool inDialogue;
    private bool isTakingKnockback = false;
    private float dashcooldownCounter = 0;
    private bool isJumping;


    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        gravity = rb.gravityScale;
        transform.position = new Vector2(PlayerPrefs.GetFloat("X Location"), PlayerPrefs.GetFloat("Y Location"));
        hasDash = PlayerPrefs.GetInt("Dash") == 1;
        hasGroundChannel = PlayerPrefs.GetInt("Ground Channel") == 1;
        bgmSFX.Play();
        if(PlayerPrefs.GetInt("Lantern") == 1)
        {
            var obj = Instantiate(lanternPrefab, transform);
            obj.transform.parent = null;
            obj.GetComponent<Lantern>().SetPickedUp(true);
        }
    }

    private void Update()
    {
        isTakingKnockback = health.IsTakingKnockback();

        //ground check
        isGrounded = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayer);

        //flip
        if (canMove && !inDialogue)
        {
            axis = Input.GetAxis("Horizontal");
        }
        else
        {
            axis = 0;
        }

        if (axis > 0 && !isDashing && !isAttacking && !isTakingKnockback && !inDialogue) transform.eulerAngles = new Vector3(0, 0, 0);
        else if (axis < 0 && !isDashing && !isAttacking && !isTakingKnockback && !inDialogue) transform.eulerAngles = new Vector3(0, 180, 0);

        axisLast = axis;

        //move
        if (!isDashing && !isAttacking && !isTakingKnockback && !inDialogue)
        {
            rb.velocity = new Vector2(axis * moveSpeed, rb.velocity.y);
        }

        if (!walkSFX.isPlaying && isGrounded && axis != 0 && !isDashing && !isAttacking && !isTakingKnockback && !inDialogue)
        {
            walkSFX.Play();
        }
        else
        {
            walkSFX.Stop();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && hasDash && !isTakingKnockback && !inDialogue && dashcooldownCounter < 0)
        {
            isDashing = true;
            StartCoroutine(dash());
            dashParticles.Play();
            dashcooldownCounter = dashCooldown;
        }
        dashcooldownCounter -= Time.deltaTime;
        //Attack
        if (Input.GetKeyDown(KeyCode.E) && !isDashing && !isAttacking && !isTakingKnockback && !inDialogue)
        {
            Debug.Log("E PRESSED");
            isAttacking = true;
            StartCoroutine(Attack());
        }
        //jump
        if (isGrounded)
        {
            coyoteTimer = 0;
            if(rb.velocity.y < 1) isJumping = false;
            rb.gravityScale = gravity;
        }
        else coyoteTimer += Time.deltaTime;
        bool canJump = (coyoteTimer <= coyoteTime);
        if (!isJumping && !isDashing && !isAttacking && !isTakingKnockback && !inDialogue && Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jumpSFX.Play();
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            animator.CrossFade("PlayerMidairJump", 0, 0);
        }
        if(isJumping && (rb.velocity.y < 0)){
            rb.gravityScale = gravity * 1.5f;
        }
        if(isJumping && rb.velocity.y > 0 && Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
        }
        //Handle animations
        HandleAnimations();
    }

    public void ToggleBossfightMusic(bool on)
    {
        if (on)
        {
            bgmSFX.Stop();
            bossMusic.Play();
        } else
        {
            bossMusic.Stop();
            bgmSFX.Play();
        }
        
    }


    private IEnumerator dash()
    {
        dashSFX.Play();
        rb.gravityScale = 0;
        int dir = 1;
        if (transform.eulerAngles.y == 180) dir = -1;
        rb.velocity = new Vector2(dir * dashSpeed, 0);
        yield return new WaitForSeconds(dashLength);
        rb.velocity = Vector2.zero;
        rb.gravityScale = gravity;
        isDashing = false;
    }
    private IEnumerator Attack()
    {
        rb.velocity = Vector2.zero;
        attackSFX.Play();
        if(hasGroundChannel && isGrounded && Input.GetAxis("Vertical") < 0)
        {
            animator.CrossFade("PlayerGroundChannel", 0, 0);
            StartCoroutine(GroundChannelAnimationVFX());
        }
        else {
            animator.CrossFade("PlayerSlice", 0, 0);
        }

        //===========================
        //DEAL DAMAGE TO ENEMIES HERE
        //===========================

        yield return new WaitForSeconds(attackAnimLength);
        isAttacking = false;
    }
    private IEnumerator GroundChannelAnimationVFX()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject vfx = Instantiate(groundChannelVFXPrefab, transform.position, transform.rotation);
        Destroy(vfx, 0.5f);
    }
    private void HandleAnimations()
    {
        animator.SetBool("Landed", isGrounded && rb.velocity.y <= 0.5f);
        animator.SetBool("Run", axis != 0);
    }
    public void ObtainDash()
    {
        hasDash = true;
    }
    public void ObtainGroundChannel()
    {
        hasGroundChannel = true;
    }
    public void DisablePlayer(bool state)
    {
        canMove = !state;
    }

    public bool getHasDash()
    {
        return hasDash;
    }

    public bool getHasGroundChannel()
    {
        return hasGroundChannel;
    }
    public void SetInDialogue(bool b)
    {
        inDialogue = b;
    }
    public bool GetInDialogue()
    {
        return inDialogue;
    }
    public void OnPlayerDeath()
    {
        StartCoroutine(PlayerDeathCoro());
    }
    public IEnumerator PlayerDeathCoro()
    {
        while (deathScreen.alpha < 0.999f)
        {
            yield return new WaitForSecondsRealtime(.1f);
            deathScreen.alpha = Mathf.Lerp(deathScreen.alpha, 1, 0.2f);
        }
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

