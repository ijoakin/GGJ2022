using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchController : MonoBehaviour
{
    [SerializeField] private List<string> targetTags = new List<string>() { "Enemy" };
    [SerializeField] private int damagePoints = 1;
    [SerializeField] private float autoDestroyDelay = 4f;

    private Rigidbody2D playerRigidbody2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ApplyDamage(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ApplyDamage(collision.gameObject);
    }

    private void ApplyDamage(GameObject gameObject)
    {
        if (targetTags.Contains(gameObject.tag))
        {
            var component = gameObject.GetComponent<IDamageTarget>();
            if (component != null)
            {
                component.TakeDamage(damagePoints);
                Destroy(this.gameObject);
            }
        }
    }

    public void Punch(Vector2 force)
    {
        if (playerRigidbody2D == null)
        {
            playerRigidbody2D = this.GetComponent<Rigidbody2D>();
        }

        var angle = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
        playerRigidbody2D.AddForce(force, ForceMode2D.Impulse);

        Destroy(this.gameObject, autoDestroyDelay);
    }
    public void Punch()
    {
        if (playerRigidbody2D == null)
        {
            playerRigidbody2D = this.GetComponent<Rigidbody2D>();
        }

        Destroy(this.gameObject, autoDestroyDelay);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector2(1f, 1f));
    }

#endif
}
