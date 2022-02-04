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

    private Rigidbody2D playerRigidbody;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private float invicibleLenght = 2f;
    private float invicibleCounter;

    public float chargeCount;
    public float chargeLenght = 2.3f;

    private AnimatorClipInfo[] animatorinfo;
    private PlayerState currentState;

    [SerializeField] public string currentStateName { get; private set; } = "";

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

        //TODO: debug purpose
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (playerMode == PlayerMode.PUNK)
                this.ConvertToMonk();
            else if (playerMode == PlayerMode.MONK)
                this.ConvertToPunk();
        }
        if (playerMode == PlayerMode.MONK)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                this.ConvertToZen();
            }
        }

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

    public void ConvertToZen()
    {
        if (!animator.GetBool("ZenContinue"))
        {
            PlayerSounds.Instance.PlayTransformationZen();
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
        if (this.playerRigidbody.velocity.x > 0)
            xbounceForce *= -1f;

        playerRigidbody.velocity = new Vector2(xbounceForce, bounceForce);

        PlayerSounds.Instance.PlayPunch();
    }

    public void MoveHorizontally()
    {
        playerRigidbody.velocity = new Vector2(MoveSpeed * Input.GetAxis("Horizontal"), playerRigidbody.velocity.y);

        if (playerRigidbody.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerRigidbody.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}
