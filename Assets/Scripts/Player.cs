using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        puncherCount = puncherTotal;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector2(MoveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);

        isGrounded = Physics2D.OverlapCircle(GroundCheckPoint.transform.position, .2f, WhatLayerIsGround);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
             rigidbody.velocity = new Vector2(rigidbody.velocity.x, JumpForce);
        }

        if (Input.GetButtonDown("Fire1") && !animator.GetBool("Punch"))
        {
            animator.SetBool("Punch", true);
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

    public void TakeDamage(float damagePoints)
    {
        throw new System.NotImplementedException();
    }
}
