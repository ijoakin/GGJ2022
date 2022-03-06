using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageTarget
{
    public static Player Instance;
    public float JumpForce;
    public float MoveSpeed;
    public float bounceForce = 15f;
    public GameObject GroundCheckPoint;
    public LayerMask WhatLayerIsGround;


    [Header("Player States")]
    public bool isGrounded;
    public bool playerIsJumping;
    public bool playerIsDead = false;
    public bool playerIsRecovering;

    private Rigidbody2D playerRigidbody;
    private Animator animator;
    internal SpriteRenderer spriteRenderer;
    
   // private float invicibleLenght = 2f;
    private float invicibleCounter;

    public float chargeCount;
    public float chargeLenght = 2.3f;

    private AnimatorClipInfo[] animatorinfo;
    private PlayerState currentState;

    // Control Variables
    private Vector2 movementDirection;
    public float nonZeroMovementX;
    public string currentStateName { get; private set; } = "";

    [Header("Animation")]
    [SerializeField] private AnimatorController animatorController;

    [Header("Attack")]
    [SerializeField] private PunchController punchController;
    [SerializeField] private GameObject punchPrefab;

    [Header("Health Variables")]
    [SerializeField] private SpriteBlinkEffect spriteBlinkEffect;

    [Header("Player interruptions")]
    public bool canMove;


    [SerializeField]
    private int damagePoints;

    public Vector2 rawInputMovement;
    public bool Attack = false;
    public bool Jump = false;

    public enum PlayerMode
    {
        PUNK = 0,
        MONK = 1,
        ZEN = 2
    }

    private PlayerMode playerMode;

    private void Awake()
    {
        Instance = this;
        canMove =true;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animatorController = GetComponent<AnimatorController>(); 

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
            currentState.SetRigidBody2D(playerRigidbody);
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
        currentState.OnUpdateState();
        
        //TODO: create a new state for jump???
        /*
        isGrounded = Physics2D.OverlapCircle(GroundCheckPoint.transform.position, .2f, WhatLayerIsGround);

        if (Input.GetButtonDown("Jump")) // && isGrounded)
        {
            var audioId = Random.Range(15, 17);
            PlayerSounds.Instance.PlayJumpPunk();

            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, JumpForce);
        }
        */


        //TODO: should we need to delete this part
        if (invicibleCounter >= 0)
        {
            invicibleCounter -= Time.deltaTime; //one second    

            if (invicibleCounter <= 0)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            }
            return;
        }
    }
    public void Punch()
    {
        Charge(-5);
        float offsetX = 1;
        Vector2 playerVector = new Vector2();
        if (playerMode == PlayerMode.PUNK)
        {
            if (!spriteRenderer.flipX)
            {
                offsetX = offsetX  * -1;
            }
            playerVector = new Vector2(this.transform.position.x + offsetX * -1.75f, this.transform.position.y + 2f);
        }
        else
        {
            if (spriteRenderer.flipX)
            {
                offsetX = offsetX * -1;
            }
            playerVector = new Vector2(this.transform.position.x + offsetX * 1.75f, this.transform.position.y + 2f);
        }

        var punch = Instantiate(this.punchPrefab, playerVector, this.transform.rotation);
        punch.GetComponent<PunchController>().Punch();
    }

    public void ConvertToZen()
    {
        if (playerMode != PlayerMode.ZEN)
        {
            this.playerMode = PlayerMode.ZEN;
            ExecuteState<MonkZenTransformationState>();
        }
    }

    public void ConvertToMonk()
    {
        if (playerMode != PlayerMode.MONK)
        {
            this.playerMode = PlayerMode.MONK;
            ExecuteState<FireballState>();
        }
    }

    public void ConvertToPunk()
    {
        if (playerMode != PlayerMode.PUNK)
        {
            this.playerMode = PlayerMode.PUNK;
            ExecuteState<PunkIdleState>();
        }
    }

    public void Charge(int value)
    {
        GameLogic.Instance.Charge(value);
    }

    public void TakeDamage(int damagePoints)
    {
        if (playerIsDead || playerIsRecovering) return;

        damagePoints = -5;
        GameLogic.Instance.Charge(damagePoints);

        StartCoroutine(StartPlayerRecovering());
    }

    public void MoveHorizontally(float multiplyer = 1.0f)
    {
        if(!canMove) return;
        playerRigidbody.velocity = new Vector2(MoveSpeed * rawInputMovement.x * multiplyer, playerRigidbody.velocity.y);

        movementDirection = rawInputMovement;

        if (Mathf.Abs(movementDirection.x) > 0)
        {
            nonZeroMovementX = movementDirection.x;
        }

        if (playerRigidbody.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerRigidbody.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void PushVertically(float force = 0.0f)
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, force);
    }
    public IEnumerator StartPlayerRecovering()
    {
        spriteBlinkEffect.PlayBlinkEffect();
        playerIsRecovering = true;
        canMove = false;
        if (this.playerMode == PlayerMode.PUNK)
        {
            ExecuteState<PunkHurtState>();
        }
        else
        {
            //TODO: ExecuteState<MonkHurtState>();
        }

        playerRigidbody.AddForce(Vector2.left * Mathf.Sign(nonZeroMovementX) * 12, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        playerRigidbody.velocity = Vector2.zero;

        yield return new WaitForSeconds(.5f);
        playerIsRecovering = false;
        spriteBlinkEffect.StopBlinkEffect();
        if (this.playerMode == PlayerMode.PUNK)
        {
            ExecuteState<PunkIdleState>();
        }
    }
}
