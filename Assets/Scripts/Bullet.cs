using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float lifetime = 5;

    private bool hasVelocity = false;
    
    public Transform target { get; set; }

    [SerializeField]
    private float projectileVelocity = 10;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    public void launch(bool goesRight)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.z = 0;
        GetComponent<Rigidbody2D>().velocity = dir * projectileVelocity;
    }

    public void launchDiagonal(bool goesRight, bool goesUp)
    {

        Vector3 dir = (target.position - transform.position).normalized;
        dir.z = 0;

        float x = dir.x;
        float y = dir.y;
        if (goesUp)
        {
            dir.x = x * Mathf.Cos(Mathf.Deg2Rad * 35) - y * Mathf.Sin(Mathf.Deg2Rad * 35);
            dir.y = y * Mathf.Cos(Mathf.Deg2Rad * 35) + x * Mathf.Sin(Mathf.Deg2Rad * 35);
        }
        else
        {
            dir.x = x * Mathf.Cos(Mathf.Deg2Rad * 35) + y * Mathf.Sin(Mathf.Deg2Rad * 35);
            dir.y = y * Mathf.Cos(Mathf.Deg2Rad * 35) - x * Mathf.Sin(Mathf.Deg2Rad * 35);
        }
        GetComponent<Rigidbody2D>().velocity = dir * projectileVelocity;
    }

    public void LaunchSecondaryFire(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * projectileVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().takeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
