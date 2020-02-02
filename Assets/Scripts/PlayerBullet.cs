using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

     [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float lifetime = 5;

    private bool hasVelocity = false;
    
    [SerializeField]
    private float projectileVelocity = 10;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    public void launch(bool goesRight)
    {

        Vector3 fwd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        fwd.z = 0;
        Vector3 dir = (fwd - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = dir * projectileVelocity;
    }

    public void launchDiagonal(bool goesRight, bool goesUp)
    {
        Vector3 fwd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        fwd.z = 0;
        Vector3 dir = (fwd - transform.position).normalized;
        float x = dir.x;
        float y = dir.y;
        if (goesUp)
        {
            dir.x = x * Mathf.Cos(Mathf.Deg2Rad * 15) - y * Mathf.Sin(Mathf.Deg2Rad * 15);
            dir.y = y * Mathf.Cos(Mathf.Deg2Rad * 15) + x * Mathf.Sin(Mathf.Deg2Rad * 15);
        }
        else
        {
            dir.x = x * Mathf.Cos(Mathf.Deg2Rad * 15) + y * Mathf.Sin(Mathf.Deg2Rad * 15);
            dir.y = y * Mathf.Cos(Mathf.Deg2Rad * 15) - x * Mathf.Sin(Mathf.Deg2Rad * 15);
        }
        GetComponent<Rigidbody2D>().velocity = dir * projectileVelocity;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponentInParent<Enemy>().takeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}

