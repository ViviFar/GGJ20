using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth = 15;
    protected int currentHealth;

    [SerializeField]
    protected Transform spawnBullet;

    [SerializeField]
    protected float distanceToShoot = 10;

    [SerializeField]
    protected float speed = 2;

    protected Player target;

    [SerializeField]
    protected GameObject bulletPrefab;

    [SerializeField]
    protected Transform bulletContainer;

    [SerializeField]
    protected float shootCooldown = 1;
    protected float timer = 0;

    public StuManager stu { get; set; }

    protected bool faceRight = true;
    [SerializeField]
    protected Animator anm;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        bulletContainer = GameObject.FindGameObjectWithTag("BulletSpawn").transform;
        currentHealth = maxHealth;
        target = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if ((target.transform.position - transform.position).magnitude <= distanceToShoot)
        {
            anm.SetBool("isWalking", false);
            anm.SetBool("isShooting", true);
            int shoot;
            if (!StateMachine.Instance.QARepaired)
            {
                shoot = UnityEngine.Random.Range(0, 10);
            }
            else
            {
                shoot = 9;
            }
            anm.SetInteger("shotToChoose", shoot);
            HandleShoot();
        }
        else
        {
            anm.SetBool("isWalking", true);
            anm.SetBool("isShooting", false);
            HandleMovements();
        }
    }

    protected void HandleMovements()
    {
        Vector3 newpos = transform.position;
        if(transform.position.x> target.transform.position.x && faceRight)
        {
            flip();
        }
        else if(transform.position.x < target.transform.position.x && !faceRight)
        {
            flip();
        }
        newpos = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position = newpos;
    }

    protected void flip()
    {
        faceRight = !faceRight;

        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
    }
    protected virtual void HandleShoot()
    {
        if (timer > shootCooldown)
        {
            GameObject go = Instantiate(bulletPrefab, spawnBullet.position, new Quaternion(), bulletContainer);

            go.GetComponent<Bullet>().target = target.transform;
            if (target.transform.position.x < transform.position.x)
            {
                if (faceRight)
                {
                    flip();
                }
                go.GetComponent<Bullet>().launch(false);
            }
            else
            {
                if (!faceRight)
                {
                    flip();
                }
                go.GetComponent<Bullet>().launch(true);
            }
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            stu.adjustNumberOfEnemies();
            Destroy(gameObject);
        }
    }
}
