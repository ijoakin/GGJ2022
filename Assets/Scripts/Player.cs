using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class Player : MonoBehaviour, IDamageTarget
{
    public static Player Instance;
    public float JumpForce;
    public float MoveSpeed;
    public GameObject GroundCheckPoint;
    public LayerMask WhatLayerIsGround;

    private Rigidbody2D rigidbody;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float puncherCount;
    private float puncherTotal = 0.5f;
    private BoxCollider2D boxCollider;

    private float fireBallCount;
    private float fireBallTotal = 2.2f;

    private float zenCount;
    private float zenTotal = 0.75f;


    public enum PlayerMode
    {
        PUNK = 0,
        MONK = 1
    }

    private PlayerMode playerMode;

    private void Awake()
    {
        Instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        puncherCount = puncherTotal;

        this.playerMode = PlayerMode.PUNK;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector2(MoveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);

        isGrounded = Physics2D.OverlapCircle(GroundCheckPoint.transform.position, .2f, WhatLayerIsGround);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            var audioId = Random.Range(15, 17);
            AudioManager.instance.PlaySFX(audioId);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, JumpForce);
        }

        ////TODO: Remove this -> testing monk
        //if (Input.GetButtonDown("Fire2"))
        //{
        //    if (playerMode == PlayerMode.PUNK)
        //        ConvertToMonk();
        //    else
        //        ConvertToPunk();
        //}
        ////TODO: Remove this -> testing monk zen
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    if (playerMode == PlayerMode.MONK)
        //        ConvertToZen();
        //    else
        //        ConvertToMonk();
        //}
        //TODO: Remove this-> testing charge

        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(-10);
        }

        if (playerMode == PlayerMode.PUNK)
            UpdatePunk();
        else
            UpdateMonk();
    }
    private void UpdateMonk()
    {

        if (Input.GetButtonDown("Fire1") && !animator.GetBool("Monk_Kick"))
        {
            animator.SetBool("Monk_Kick", true);
        }

        if (animator.GetBool("Monk_Kick"))
        {
            puncherCount -= Time.deltaTime;
            if (puncherCount <= 0)
            {
                puncherCount = puncherTotal;
                animator.SetBool("Monk_Kick", false);
                AudioManager.instance.PlaySFX((int)AudioId.MONJESALTA2);
            }
        }

        if (animator.GetBool("FireBall"))
        {
            fireBallCount -= Time.deltaTime;
            if (fireBallCount <= 0)
            {
                animator.SetBool("Monk_Idle", true);
                animator.SetBool("FireBall", false);
            }
        }

        if (animator.GetBool("Zen"))
        {
            zenCount -= Time.deltaTime;
            if (zenCount <= 0)
            {
                animator.SetBool("Zen", false);
                animator.SetBool("ZenContinue", true);
            }
        }

        if (rigidbody.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (this.rigidbody.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (rigidbody.velocity.x != 0)
        {
            animator.SetBool("Monk_Walk", true);
        }
        else
        {
            animator.SetBool("Monk_Walk", false);
        }
    }

    private void UpdatePunk()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("Punch"))
        {
            animator.SetBool("Punch", true);

            var audioId = Random.Range(10, 14);
            AudioManager.instance.PlaySFX(audioId);
        }

        if (animator.GetBool("Punch"))
        {
            puncherCount -= Time.deltaTime;
            if (puncherCount <= 0)
            {
                puncherCount = puncherTotal;
                animator.SetBool("Punch", false);
            }
        }

        if (rigidbody.velocity.x != 0)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        if (rigidbody.velocity.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (this.rigidbody.velocity.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void ConvertToZen()
    {
        zenCount = zenTotal;

        //TODO: FireBall animation
        animator.SetBool("Zen", true);
    }

    public void ConvertToMonk()
    {

        fireBallCount = fireBallTotal;
        AudioManager.instance.PlaySFX((int) AudioId.MONJETRANS);
        //TODO: FireBall animation
        animator.SetBool("FireBall", true);
        animator.SetBool("ZenContinue", false);
        this.playerMode = PlayerMode.MONK;

        boxCollider.offset = new Vector2(boxCollider.offset.x, -0.05f);
    }

    public void ConvertToPunk()
    {
        animator.SetBool("Monk_Idle", false);
        animator.SetBool("Monk_Walk", false);
        animator.SetBool("FireBall", false);
        this.playerMode = PlayerMode.PUNK;

        boxCollider.offset = new Vector2(boxCollider.offset.x, 0f);
    }


    public void TakeDamage(int damagePoints)
    {
        //throw new System.NotImplementedException();
        
        GameLogic.Instance.Charge(damagePoints);

    }
}
