using UnityEngine;

public abstract class EnemyGameObject : MonoBehaviour, IDamageTarget
{
    [Header("Debug Variables")]
    [SerializeField] private bool active = false;

    [SerializeField] private string currentStateName = "";

    [Header("Animation")]
    [SerializeField] private AnimatorController animatorController;

    [Header("Health Variables")]
    [SerializeField] protected float health = 3;

    [SerializeField] private GameObject damageEffect;

    public bool stateFinished;
    private bool enemyIsDead = false;

    // Enemy States Management
    private EnemyState currentState = null;

    private Rigidbody2D _rigidBody2D;
    private Transform lookAtTransform;

    public abstract void OnAwake();

    public abstract void OnUpdate();

    public void Jump()
    {
        Debug.Log("JUMP GAMEOBJECT");
    }

    public virtual void OnDie()
    {
        Instantiate(damageEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public virtual void OnSleep()
    {
    }

    private void Update()
    {
        if (active && !enemyIsDead)
        {
            if (this.lookAtTransform != null)
            {
                var dir = this.lookAtTransform.position - this.transform.position;
                if (dir.x >= 0)
                {
                    this.transform.rotation = Quaternion.identity;
                }
                else
                {
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }

            OnUpdate();
            if (this.lookAtTransform == null)
            {
                HandleLookingDirection();
            }
            if (currentState != null)
            {
                currentState.OnUpdateState();
            }
        }
    }

    public void StateFinished()
    {
        stateFinished = true;
    }

    public void SetLookAt(Transform lookAtTransform)
    {
        this.lookAtTransform = lookAtTransform;
    }

    protected bool CurrentStateIs<T>() where T : Component
    {
        return currentState != null && (currentState is T);
    }

    protected void ExecuteState<T>() where T : Component
    {
        var componentInObject = GetComponent<T>();
        if (componentInObject != null)
        {
            if (_rigidBody2D == null)
            {
                _rigidBody2D = GetComponent<Rigidbody2D>();
            }
            if (_rigidBody2D != null)
            {
                _rigidBody2D.velocity = Vector2.zero;
            }
            if (currentState != null)
            {
                currentState.OnExitState();
            }
            stateFinished = false;

            currentState = componentInObject as EnemyState;
            currentState.SetEnemyGameObject(this);
            currentState.SetRigidBody2D(_rigidBody2D);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            AwakeEnemy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            SleepEnemy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            AwakeEnemy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            SleepEnemy(collision.gameObject);
        }
    }

    private void AwakeEnemy(GameObject cameraObject)
    {
        if (cameraObject.tag.Equals("MainCamera") && !active)
        {
            active = true;
            OnAwake();
        }
    }

    private void SleepEnemy(GameObject cameraObject)
    {
        if (cameraObject.tag.Equals("MainCamera") && active)
        {
            if (_rigidBody2D != null)
            {
                _rigidBody2D.velocity = Vector2.zero;
            }
            active = false;
            OnSleep();
        }
    }

    private void HandleLookingDirection()
    {
        if (_rigidBody2D == null)
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        if (_rigidBody2D.velocity.magnitude > 0)
        {
            if (_rigidBody2D.velocity.x >= 0)
            {
                this.transform.rotation = Quaternion.identity;
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public void TakeDamage(int damagePoints)
    {
        if (enemyIsDead) return;

        health -= damagePoints;

        if (damageEffect)
            Instantiate(damageEffect, this.transform.position, Quaternion.identity);

        OnReceiveDamage();
        if (health <= 0 && !enemyIsDead)
        {
            health = 0;
            enemyIsDead = true;
            OnDie();
        }
    }

    public virtual void OnReceiveDamage()
    {
    }
}
