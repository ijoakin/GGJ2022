using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageTarget
{
    public static Player Instance;
    public float JumpForce;
    public float MoveSpeed;
    public float bounceForce = 15f;
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

    private float invicibleLenght = 2f;
    private float invicibleCounter;

    private float chargeCount;
    private float chargeLenght = 2.5f;

    [Header("Attack")]
    [SerializeField] private PunchController punchController;
    [SerializeField] private GameObject punchPrefab;

    [SerializeField]
    private int damagePoints;

    private bool IsConverting = false;

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
        invicibleCounter = 0;
        this.playerMode = PlayerMode.PUNK;

        chargeCount = chargeLenght;
    }

    bool isIdle()
    {
        
        if (animator.GetBool("Monk_Idle"))
        {
            return true;
        }

        if (!animator.GetBool("Walk") && !animator.GetBool("Punch") && !animator.GetBool("Monk_Kick")
            && !animator.GetBool("Monk_Walk")
            && !animator.GetBool("Zen")
            && !animator.GetBool("ZenContinue")
            && !animator.GetBool("FireBall"))
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector2(MoveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);

        isGrounded = Physics2D.OverlapCircle(GroundCheckPoint.transform.position, .2f, WhatLayerIsGround);

        if ((chargeCount >= 0) && isIdle())
        {
            chargeCount -= Time.deltaTime;
            if (chargeCount <= 0)
            {
                Charge(10);
                chargeCount = chargeLenght;
            }
        }

        if (invicibleCounter >= 0)
        {
            invicibleCounter -= Time.deltaTime; //one second    

            if (invicibleCounter <= 0)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            }
            return;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            var audioId = Random.Range(15, 17);
            PlayerSounds.Instance.PlayJumpPunk();

            rigidbody.velocity = new Vector2(rigidbody.velocity.x, JumpForce);
        }

        //Remove this. the enemy will call TakeDamage
        if (Input.GetKeyDown(KeyCode.E))
        {
            Charge(10);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(10);
        }

        if (playerMode == PlayerMode.PUNK)
            UpdatePunk();
        else
            UpdateMonk();
    }
    public void Punch()
    {
        Charge(-5);
        float offsetX = 1;
        if (playerMode == PlayerMode.PUNK)
        {
            if (!spriteRenderer.flipX)
            {
                offsetX = offsetX  * -1;
            }
        }
        else
        {
            if (spriteRenderer.flipX)
            {
                offsetX = offsetX * -1;
            }
        }

        var punch = Instantiate(this.punchPrefab, new Vector2(this.transform.position.x + offsetX, this.transform.position.y) , this.transform.rotation);
        punch.GetComponent<PunchController>().Punch();
    }

    private void UpdateMonk()
    {

        if (Input.GetButtonDown("Fire1") && !animator.GetBool("Monk_Kick"))
        {
            animator.SetBool("Monk_Kick", true);
            Punch();
        }

        if (animator.GetBool("Monk_Kick"))
        {
            puncherCount -= Time.deltaTime;
            if (puncherCount <= 0)
            {
                puncherCount = puncherTotal;
                animator.SetBool("Monk_Kick", false);
                
                PlayerSounds.Instance.PlayJumpMonk();
            }
        }

        if (animator.GetBool("FireBall"))
        {
            fireBallCount -= Time.deltaTime;
            if (fireBallCount <= 0)
            {
                animator.SetBool("Monk_Idle", true);
                animator.SetBool("FireBall", false);
                IsConverting = false;
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
            Punch();
            PlayerSounds.Instance.PlayPunch();
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
        if (!animator.GetBool("ZenContinue"))
        {
            zenCount = zenTotal;

            //TODO: FireBall animation
            animator.SetBool("Zen", true);
            PlayerSounds.Instance.PlayTransformationZen();
        }
        
    }

    public void ConvertToMonk()
    {
        if (!IsConverting && playerMode != PlayerMode.MONK)
        {
            fireBallCount = fireBallTotal;

            PlayerSounds.Instance.PlayTransformation();

            //TODO: FireBall animation
            animator.SetBool("FireBall", true);
            animator.SetBool("ZenContinue", false);
            this.playerMode = PlayerMode.MONK;

            boxCollider.offset = new Vector2(boxCollider.offset.x, -0.05f);
            IsConverting = true;
        }
        
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
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (animator.GetBool("Punch") || animator.GetBool("Monk_Kick"))
            {
                var component = collision.gameObject.GetComponent<DamageController>();

                if (component != null)
                {
                    //take damage!!!
                    component.ApplyDamage(gameObject, 3);
                }
            }
        }
    }

    public void Charge(int value)
    {
        GameLogic.Instance.Charge(value);
    }

    public void TakeDamage(int damagePoints)
    {
        damagePoints *= -1;
        //throw new System.NotImplementedException();
        var isFigthing = (animator.GetBool("Punch") || animator.GetBool("Monk_Kick"));

        if (!isFigthing)
        {
            Bounce();
            invicibleCounter = invicibleLenght;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, .5f);
            GameLogic.Instance.Charge(damagePoints);
        }
    }
    public void Bounce()
    {
        var xbounceForce = bounceForce;
        if (this.rigidbody.velocity.x > 0)
            xbounceForce *= -1f;

        rigidbody.velocity = new Vector2(xbounceForce, bounceForce);

        //rigidbody.AddForce(new Vector2(100f, 5f));

        PlayerSounds.Instance.PlayPunch();
    }
}
