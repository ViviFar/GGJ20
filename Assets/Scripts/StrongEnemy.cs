using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongEnemy : Enemy
{
    [SerializeField]
    protected Transform spawnSecondaryShoot, spawnTertiaryShoot;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    protected override void HandleShoot()
    {
        if (timer > shootCooldown)
        {
            if (target.transform.position.x < transform.position.x)
            {
                if (faceRight)
                {
                    flip();
                }
                GameObject go = Instantiate(bulletPrefab, spawnBullet.position, new Quaternion(), bulletContainer);
                go.GetComponent<Bullet>().target = target.transform;
                go.GetComponent<Bullet>().launch(false);
                GameObject go2 = Instantiate(bulletPrefab, spawnSecondaryShoot.position, new Quaternion(), bulletContainer);
                go2.GetComponent<Bullet>().target = target.transform;
                //  go2.GetComponent<Bullet>().launchDiagonal(false, true);
                go2.GetComponent<Bullet>().LaunchSecondaryFire((target.transform.position - spawnBullet.position).normalized);
                GameObject go3 = Instantiate(bulletPrefab, spawnTertiaryShoot.position, new Quaternion(), bulletContainer);
                go3.GetComponent<Bullet>().target = target.transform;
                //go3.GetComponent<Bullet>().launchDiagonal(false, false);
                go3.GetComponent<Bullet>().LaunchSecondaryFire((target.transform.position - spawnBullet.position).normalized);
            }
            else
            {
                if (!faceRight)
                {
                    flip();
                }
                GameObject go = Instantiate(bulletPrefab, this.transform.position, new Quaternion(), bulletContainer);
                go.GetComponent<Bullet>().target = target.transform;
                go.GetComponent<Bullet>().launch(true);
                GameObject go2 = Instantiate(bulletPrefab, this.transform.position, new Quaternion(), bulletContainer);
                go2.GetComponent<Bullet>().target = target.transform;
                //go2.GetComponent<Bullet>().launchDiagonal(true, true);
                go2.GetComponent<Bullet>().LaunchSecondaryFire((target.transform.position - spawnBullet.position).normalized);
                GameObject go3 = Instantiate(bulletPrefab, this.transform.position, new Quaternion(), bulletContainer);
                go3.GetComponent<Bullet>().target = target.transform;
                //go3.GetComponent<Bullet>().launchDiagonal(true, false);
                go3.GetComponent<Bullet>().LaunchSecondaryFire((target.transform.position - spawnBullet.position).normalized);
            }
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    override protected void Update()
    {
        base.Update();
    }

}
