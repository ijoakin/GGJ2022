using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    private float kickCount;
    private float kickTotal = 0.5f;


    private BoxCollider2D boxCollider;

    private float fireBallCount;
    private float fireBallTotal = 2.2f;

    private float zenCount;
    private float zenTotal = 0.75f;

    private float invicibleLenght = 2f;
    private float invicibleCounter;

    private float chargeCount;
    private float chargeLenght = 2.3f;

    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private PlayerState currentState;

    public bool stateFinished;

    [SerializeField] private string currentStateName = "";

    [Header("Animation")]
    [SerializeField] private AnimatorController animatorController;

    public enum PlayerStateId
    {
        Player_Idle,
        Player_Punch,
        Player_Walk,
        Player_Fireball,
        Player_Monk_Idle,
        Player_Monk_Run,
        Player_Monk_Kick,
        Player_Monk_Zen,
        Player_Monk_Zen_Continue
    }

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

    public void StateFinished()
    {
        stateFinished = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        animatorController = GetComponent<AnimatorController>(); 

        puncherCount = puncherTotal;
        kickCount = kickTotal;
        invicibleCounter = 0;
        this.playerMode = PlayerMode.PUNK;

        chargeCount = chargeLenght;

        ExecuteState<PunkIdleState>();
    }
    public void PlayAnimation()
    {
        PlayAnimation(currentState);
    }
    public void PlayAnimation(PlayerState stateId)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.Play(stateId.ToString());
    }

    public void PlayAnimation(AnimationClip animationClip)
    {
        animator.Play(animationClip.name.ToString());
    }

    bool isIdle()
    {
        //Debug.Log($"{animator.GetBool("Monk_Idle")}");

        if (current_animation == "Player_Monk_Idle")
        {
            return true;
        }

        if ((playerMode == PlayerMode.PUNK) && (!animator.GetBool("Walk") && !animator.GetBool("Punch")))
        {
            return true;
        }

        return false;
    }

    public void ExecuteState<T>() where T : Component
    {
        var componentInObject = GetComponent<T>();
        if (componentInObject != null)
        {
            if (currentState != null)
            {
                currentState.OnExitState();
            }
            stateFinished = false;

            currentState = componentInObject as PlayerState;
            currentState.SetEnemyGameObject(this);
            currentState.SetRigidBody2D(rigidbody);
            currentState.SetSpriteRenderer(spriteRenderer);

            if (currentState.GetAnimationClip() != null)
            {
                animatorController.Play(currentState.GetAnimationClip());
            }
            currentState.OnEnterState();
            currentStateName = typeof(T).Name;
        }
        else
        {
            Debug.LogError("COMPONENT " + typeof(T).Name + " IS NOT ATTACHED TO " + this.gameObject.name);
        }
    }
    protected bool CurrentStateIs<T>() where T : Component
    {
        return currentState != null && (currentState is T);
    }

    // Update is called once per frame
    void Update()
    {
        animatorinfo = this.animator.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        rigidbody.velocity = new Vector2(MoveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);

        isGrounded = Physics2D.OverlapCircle(GroundCheckPoint.transform.position, .2f, WhatLayerIsGround);

        if (playerMode == PlayerMode.PUNK)
            UpdatePunk();
        else
            UpdateMonk();


        if (currentState != null)
        {
            currentState.OnUpdateState();
        }

        /*

        if (stateFinished && CurrentStateIs<PunkIdleState>())
        {
            ExecuteState<PunkIdleState>();
        }

        if (stateFinished && CurrentStateIs<PunkPunchState>())
        {
            ExecuteState<PunkPunchState>();
        }

        if (stateFinished && CurrentStateIs<PunkWalkState>())
        {
            ExecuteState<PunkWalkState>();
        }

        */

        ////Debug.Log($"{chargeCount}: {isIdle()}");

        //if ((chargeCount >= 0) && isIdle())
        //{
        //    chargeCount -= Time.deltaTime;
        //    if (chargeCount <= 0)
        //    {
        //        Charge(10);
        //        chargeCount = chargeLenght;
        //    }
        //}

        if (invicibleCounter >= 0)
        {
            invicibleCounter -= Time.deltaTime; //one second    

            if (invicibleCounter <= 0)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            }
            return;
        }

        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    var audioId = Random.Range(15, 17);
        //    PlayerSounds.Instance.PlayJumpPunk();

        //    rigidbody.velocity = new Vector2(rigidbody.velocity.x, JumpForce);
        //}

        ////Remove this. the enemy will call TakeDamage
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Charge(10);
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    TakeDamage(10);
        //}


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
        if (!stateFinished && CurrentStateIs<FireballState>())
        {
            ExecuteState<FireballState>();
        }
        else
        {
            //El problema esta acá
            if (Input.GetAxis("Horizontal") != 0)
                ExecuteState<MonkRunState>();
            else
                ExecuteState<MonkIdleState>();
        }


        //WALK


        //if (Input.GetButtonDown("Fire1"))
        //{
        //    ExecuteState<MonkKickState>();
        //}

        //Remove
        if (Input.GetKeyDown(KeyCode.G))
        {
            this.ConvertToPunk();
        }

        //if (Input.GetButtonDown("Fire1") && !animator.GetBool("Monk_Kick"))
        //{
        //    animator.SetBool("Monk_Kick", true);
        //    PlayerSounds.Instance.PlayJumpMonk();
        //    Punch();
        //}

        //if (animator.GetBool("Monk_Kick"))
        //{
        //    kickCount -= Time.deltaTime;

        //    if (kickCount <= 0)
        //    {
        //        kickCount = kickTotal;
        //        animator.SetBool("Monk_Kick", false);
        //    }
        //}

        //if (animator.GetBool("FireBall"))
        //{
        //    fireBallCount -= Time.deltaTime;
        //    if (fireBallCount <= 0)
        //    {
        //        animator.SetBool("Monk_Idle", true);
        //        animator.SetBool("FireBall", false);
        //        IsConverting = false;
        //    }
        //}

        //if (animator.GetBool("Zen"))
        //{
        //    zenCount -= Time.deltaTime;
        //    if (zenCount <= 0)
        //    {
        //        animator.SetBool("Zen", false);
        //        animator.SetBool("ZenContinue", true);
        //    }
        //}

        //if (rigidbody.velocity.x > 0)
        //{
        //    spriteRenderer.flipX = false;
        //}
        //else if (this.rigidbody.velocity.x < 0)
        //{
        //    spriteRenderer.flipX = true;
        //}

        //if (rigidbody.velocity.x != 0)
        //{
        //    animator.SetBool("Monk_Walk", true);
        //}
        //else
        //{
        //    animator.SetBool("Monk_Walk", false);
        //}
    }

    private void UpdatePunk()
    {
        //WALK
        if (Input.GetAxis("Horizontal") != 0)
            ExecuteState<PunkWalkState>();
        else
            ExecuteState<PunkIdleState>();

        if (Input.GetButtonDown("Fire1"))
        {
            ExecuteState<PunkPunchState>();
        }

        //remove this.
        if (Input.GetKeyDown(KeyCode.G))
        {
            this.ConvertToMonk();
        }

        //if (Input.GetButtonDown("Fire1") && currentState != PlayerState.Player_Punch)
        //{
        //    currentState = PlayerState.Player_Punch;
        //    PlayAnimation();

        //    Punch();
        //    PlayerSounds.Instance.PlayPunch();
        //}

        //if (currentState == PlayerState.Player_Punch)
        //{
        //    puncherCount -= Time.deltaTime;
        //    if (puncherCount <= 0)
        //    {
        //        puncherCount = puncherTotal;
        //        currentState = PlayerState.Player_Idle;
        //        PlayAnimation();
        //    }
        //}

        //if (rigidbody.velocity.x != 0)
        //{
        //    animator.SetBool("Walk", true);
        //}
        //else
        //{
        //    animator.SetBool("Walk", false);
        //}

        //if (rigidbody.velocity.x > 0)
        //{
        //    spriteRenderer.flipX = true;
        //}
        //else if (this.rigidbody.velocity.x < 0)
        //{
        //    spriteRenderer.flipX = false;
        //}
    }

    public void ConvertToZen()
    {
        if (!animator.GetBool("ZenContinue"))
        {
            ExecuteState<MonkZenState>();
        }
    }

    public void ConvertToMonk()
    {
        if (playerMode != PlayerMode.MONK)
        {
            this.playerMode = PlayerMode.MONK;
            ExecuteState<FireballState>();

            boxCollider.offset = new Vector2(boxCollider.offset.x, -0.05f);
        }
    }

    public void ConvertToPunk()
    {
        this.playerMode = PlayerMode.PUNK;

        boxCollider.offset = new Vector2(boxCollider.offset.x, 0f);
    }
   

    public void Charge(int value)
    {
        GameLogic.Instance.Charge(value);
    }

    public void TakeDamage(int damagePoints)
    {
        damagePoints = -5;
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

        PlayerSounds.Instance.PlayPunch();
    }
}
