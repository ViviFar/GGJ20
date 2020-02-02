using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    #region Health parameters
    [SerializeField]
    private int maxHealth = 10;
    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    
    private int numberOfFullHearts;
    [SerializeField]
    private List<Image> hearts;

    [SerializeField]
    private Sprite fullHeart, emptyHeart;
    #endregion

    #region movement parameters
    [SerializeField]
    private float speed = 2.0f;
    //TODO : manage maxHeight and Dist by the resolution and not by arbitrary values;
    [SerializeField]
    private float maxHeight = 7;
    public float MaxHeight { get { return maxHeight; } }
    [SerializeField]
    private float maxDistHorizontal = 31;
    public float MaxDistHorizontal { get { return maxDistHorizontal; } }
    [SerializeField]
    private float minDistHorizontal = 31;
    public float MinDistHorizontal { get { return minDistHorizontal; } }


    private bool faceRight = true;

    [SerializeField]
    private Animator anim;

    #endregion

    #region shoot parameters
    [SerializeField]
    private GameObject projectilePrefab;
    private bool canShoot = true;
    [SerializeField]
    private float cooldownBetweenShoots = 0.75f;
    private float timer;
    [SerializeField]
    private float cooldownForSecondaryShoot = 1;
    private float timerForSecondaryShoot;

    [SerializeField]
    private Transform bulletParent;
    #endregion

    

    private void Start()
    {
        numberOfFullHearts = maxHealth;
        Debug.Log("number of hearts :" + numberOfFullHearts);
        hearts = new List<Image>();
        IAmAHeart[] go = FindObjectsOfType<IAmAHeart>();
        Debug.Log("number of objects in go: "  +go.Length);
        foreach(IAmAHeart g in go)
        {
            g.GetComponent<Image>().sprite = fullHeart;
            hearts.Add(g.GetComponent<Image>());
        }
        bulletParent = GameObject.FindGameObjectWithTag("BulletSpawn").transform;
        Camera.main.GetComponent<FollowPlayer>().playerScript = this;
        Camera.main.GetComponent<FollowPlayer>().PlayerAdded();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        HandleMovement();
        HandleShoot();
    }


    private void HandleMovement()
    {
        //manage which side the charater is facing
        Vector2 mouse = Input.mousePosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, Camera.main.nearClipPlane));
        //the player shoots on the side where the mouse points
        if (faceRight && mousePos.x < transform.position.x)
        {
            flip();
        }
        else if (!faceRight && mousePos.x > transform.position.x)
        {
            flip();
        }
        //change the player animation state when start or stop moving
        if (Input.GetAxis("Horizontal") ==0 && Input.GetAxis("Vertical") == 0)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }
        float mvt = speed * Time.deltaTime;
        float horizMvt = transform.position.x + Input.GetAxis("Horizontal") * mvt;
        float vertMvt = transform.position.y + Input.GetAxis("Vertical") * mvt;
        //limit movement within range of the map
        vertMvt = Mathf.Min(vertMvt, maxHeight);
        vertMvt = Mathf.Max(vertMvt, -maxHeight);
        horizMvt = Mathf.Min(horizMvt, maxDistHorizontal);
        horizMvt = Mathf.Max(horizMvt, minDistHorizontal);
        transform.position = new Vector3(horizMvt, vertMvt, 0); ;
    }

    private void flip()
    {
        faceRight = !faceRight;

        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
    }

    private void HandleShoot()
    {
        //update timer only if shoot is in cooldown, reset ability to shoot if cooldown is finished
        if (timer>=cooldownBetweenShoots && timerForSecondaryShoot>= cooldownForSecondaryShoot && !canShoot)
        {
            canShoot = true;
        }
        else if(!canShoot)
        {
            timer += Time.deltaTime;
            timerForSecondaryShoot += Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if (canShoot)
            {

                //reset tier, block ability to shoot any of the 2 shoots
                canShoot = false;
                timer = 0;
                GameObject go = Instantiate(projectilePrefab, this.transform.position, new Quaternion(), bulletParent);
                //make the projectile go the direction faced by the character;
                go.GetComponent<PlayerBullet>().launch(faceRight);
            }
        }
        else if(Input.GetMouseButton(1) && StateMachine.Instance.GDRepaired)
        {
            if (canShoot)
            {
                canShoot = false;
                timerForSecondaryShoot = 0;
                GameObject go1 = Instantiate(projectilePrefab, this.transform.position, new Quaternion(), bulletParent);
                //make the projectile go the direction faced by the character;
                go1.GetComponent<PlayerBullet>().launch(faceRight);
                GameObject go2 = Instantiate(projectilePrefab, this.transform.position, new Quaternion(), bulletParent);
                //make the projectile go the direction faced by the character;
                go2.GetComponent<PlayerBullet>().launchDiagonal(faceRight, true);
                GameObject go3 = Instantiate(projectilePrefab, this.transform.position, new Quaternion(), bulletParent);
                //make the projectile go the direction faced by the character;
                go3.GetComponent<PlayerBullet>().launchDiagonal(faceRight, false);
            }
        }
    }


    public void takeDamage(int damage)
    {
        Debug.Log(CurrentHealth + " PV actuels");
        currentHealth -= damage;
        //hearts[numberOfFullHearts - 1].sprite = emptyHeart;
        foreach(Image im in hearts)
        {
            if (im.GetComponent<IAmAHeart>().Id == numberOfFullHearts - 1)
            {
                im.sprite = emptyHeart;
            }
        }
        Debug.Log("la liste de hearts mesure " + hearts.Count);
        numberOfFullHearts--;
        if (currentHealth <= 0)
        {
            StateMachine.Instance.CurrentState = GameState.LostGameScreen;
        }
    }
    private void OnDestroy()
    {
        Camera.main.GetComponent<FollowPlayer>().RemovePlayer();
    }
}
