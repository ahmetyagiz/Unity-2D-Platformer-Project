using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spr;
    Animator anim;
    Animator animBomb;
    AudioSource audioSource;

    public AudioSource footSteps;
    public AudioClip LeftStep, RightStep;

    public AudioSource CollectSource;
    public AudioClip CoinClip;

    public AudioSource jumpManager;
    public AudioClip jumpClip;

    public AudioSource expManager;
    public AudioClip expClip;

    [SerializeField]
    private float moveSpeed;
    public float thrust;
    private float horizontal;

    public bool isGround;
    public bool isMoving;

    public GameObject LoseScreen;
    public Text ExpTimer;

    public Transform groundCheckPoint;
    public float groundAngle;
    public Vector2 groundVector;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        animBomb = GameObject.Find("Bomb").GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        isGround = Physics2D.OverlapBox(groundCheckPoint.position, groundVector, groundAngle, groundLayer);

        //Player Move
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        //Sprite flip
        if (horizontal != 0)
        {
            spr.flipX = horizontal == -1;
        }

        if (rb.velocity.x != 0)
            isMoving = true;
        else
            isMoving = false;

        if (isMoving && isGround)
        {
            if (!footSteps.isPlaying)
                footSteps.Play();
        }
        else
            footSteps.Stop();

        //Jump
        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, thrust);
            isGround = false;
            footSteps.Stop();
            jumpManager.PlayOneShot(jumpClip);
        }

        RunAnimations();
    }

    private void RunAnimations()
    {
        anim.SetFloat("playerWalk", Mathf.Abs(horizontal));
        anim.SetBool("playerJump", !isGround);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Enemy"))
        {
            anim.SetBool("playerDie", true);
            footSteps.Stop();
            GetComponent<PlayerMovement>().enabled = false;
            StartCoroutine(LoseCoroutine());

        }

        if (collider2D.gameObject.CompareTag("Coin"))
        {
            GameObject.Find("CoinCollectTxt").GetComponent<Text>().text = "Coin Collected!";
            CollectSource.PlayOneShot(CoinClip);
            Destroy(collider2D.gameObject);
            StartCoroutine(CoinRoutine());
        }

        if (collider2D.gameObject.name == "BombTrigger")
        {
            GameObject.Find("BombTrigger").GetComponent<BoxCollider2D>().enabled = false;

            Debug.Log("Tetikleyiciye giriş yapıldı");

            StartCoroutine(ExpCoroutine());

        }
    }

    IEnumerator CoinRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        GameObject.Find("CoinCollectTxt").GetComponent<Text>().text = "";
    }

    IEnumerator ExpCoroutine()
    {

        expManager.PlayOneShot(expClip);

        for (int i = 7; i >= 1; i--)
        {
            ExpTimer.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        animBomb.SetBool("isExplosion", true);

        Destroy(GameObject.Find("PS_Parent"));
        Destroy(GameObject.Find("Particle System"));

        Destroy(ExpTimer);

        yield return new WaitForSeconds(2);

        LoseScreen.SetActive(true);

        footSteps.Stop();

        anim.SetFloat("playerWalk", 0);

        GetComponent<PlayerMovement>().enabled = false;

        GameObject.Find("TimeManager").SetActive(false);


    }

    IEnumerator LoseCoroutine()
    {
        GameObject.Find("TimeManager").SetActive(false);
        yield return new WaitForSeconds(2f);
        LoseScreen.SetActive(true);

    }

}