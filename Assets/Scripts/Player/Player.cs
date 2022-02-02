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

    private float kickTotal = 0.5f;

    private BoxCollider2D boxCollider;
    
    private float invicibleLenght = 2f;
    private float invicibleCounter;

    private float chargeCount;
    private float chargeLenght = 2.3f;

    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private PlayerState currentState;

    public bool isIdle = false;
    public bool isWalking = false;
    public bool isPunching = false;
    public bool isFireball = false;
    public bool isConvertingToAang = false;
    public bool isAvatarMode = false;


    [SerializeField] private string currentStateName = "";

    [Header("Animation")]
    [SerializeField] private AnimatorController animatorController;

    [Header("Attack")]
    [SerializeField] private PunchController punchController;
    [SerializeField] private GameObject punchPrefab;

    [SerializeField]
    private int damagePoints;

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
        animatorController = GetComponent<AnimatorController>(); 

        puncherCount = puncherTotal;
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

    public void ExecuteState<T>() where T : Component
    {
        var componentInObject = GetComponent<T>();
        if (componentInObject != null)
        {
            if (currentState != null)
            {
                currentState.OnExitState();
            }

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

        if ((chargeCount >= 0) && isIdle)
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
        if (isFireball || isPunching || isConvertingToAang || isAvatarMode)
        {
            return;
        }
        else
        {
            //El problema esta acá
            if (Input.GetAxis("Horizontal") != 0)
            {
                isWalking = true;
                ExecuteState<MonkRunState>();
            }
            else
            {
                isIdle = true;
                ExecuteState<MonkIdleState>();
            }
        }


        if (Input.GetButtonDown("Fire1"))
        {
            isPunching = true;
            ExecuteState<MonkKickState>();
        }

        //Remove
        if (Input.GetKeyDown(KeyCode.G))
        {
            this.ConvertToPunk();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.ConvertToZen();
        }

    }

    private void UpdatePunk()
    {
        if(isPunching)
        {
            return;
        }

        //WALK
        if (Input.GetAxis("Horizontal") != 0)
            ExecuteState<PunkWalkState>();
        else
            ExecuteState<PunkIdleState>();

        if (Input.GetButtonDown("Fire1"))
        {
            isPunching = true;
            ExecuteState<PunkPunchState>();
        }

        //remove this.
        if (Input.GetKeyDown(KeyCode.G))
        {
            this.ConvertToMonk();
        }
    }

    public void ConvertToZen()
    {
        if (!animator.GetBool("ZenContinue"))
        {
            isConvertingToAang = true;
            ExecuteState<MonkZenState>();
        }
    }

    public void ConvertToMonk()
    {
        if (playerMode != PlayerMode.MONK)
        {
            this.playerMode = PlayerMode.MONK;
            ExecuteState<FireballState>();
            isFireball = true;
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
