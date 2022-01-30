using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface DamageReporter
{
    void OnApplyDamage();
}

public class DamageController : MonoBehaviour
{
    [SerializeField] private List<string> targetTags = new List<string>() { "Player" };
    [SerializeField] private int damagePoints = 1;

    private DamageReporter damageReporter;

    public void SetDamageReporter(DamageReporter damageReporter)
    {
        this.damageReporter = damageReporter;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    ApplyDamage(collision.gameObject);
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    ApplyDamage(collision.gameObject);
    //}

    public void ApplyDamage(GameObject gameObject, int damagePoint)
    {
        if (targetTags.Contains(gameObject.tag))
        {
            var component = gameObject.GetComponent<IDamageTarget>();
            if (component != null)
            {
                component.TakeDamage(damagePoint);
                if (this.damageReporter != null)
                {
                    this.damageReporter.OnApplyDamage();
                }
            }
        }
    }
}
