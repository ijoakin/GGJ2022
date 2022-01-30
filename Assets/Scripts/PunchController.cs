using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchController : MonoBehaviour
{
    [SerializeField] private List<string> targetTags = new List<string>() { "Enemy" };
    [SerializeField] private int damagePoints = 1;
    [SerializeField] private float autoDestroyDelay = 4f;

    private Rigidbody2D rigidbody2D;

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

    public void Shoot(Vector2 force)
    {
        if (rigidbody2D == null)
        {
            rigidbody2D = this.GetComponent<Rigidbody2D>();
        }

        var angle = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
        rigidbody2D.AddForce(force, ForceMode2D.Impulse);

        Destroy(this.gameObject, autoDestroyDelay);
    }
}
