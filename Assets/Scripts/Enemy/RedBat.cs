using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using StateEnums;
public class RedBat : MonoBehaviour
{

    [SerializeField] private RedBatState currentState;

    public float radius = 6f;
    public LayerMask layerMask;
    public GameObject tower;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D collider;

    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float chargingSpeed = 1f;
    [SerializeField] private float rushSpeed = 20f;
    [SerializeField] private Vector2 moveVector;
    [SerializeField] private Vector3 attackPoint;
    [SerializeField] private float chargeDelay = 1.5f;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float hp = 100f;
    [SerializeField] private float alpha;
    [SerializeField] private bool isDead = false;
    [SerializeField] private AudioSource audio;
    [SerializeField] private bool isCharging;
    [SerializeField] private AudioSource boomAudio;

    private void OnEnable()
    {
        isDead = false;
        isCharging = false;
        if (collider != null) collider.enabled = true;
        currentState = RedBatState.Moving;
        hp = maxHp;
        if(rigidbody != null) rigidbody.gravityScale = 0;
        alpha = 1f;
        if (spriteRenderer != null)
        {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            spriteRenderer.color = newColor;
        }
    }


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        moveVector = (tower.transform.position - transform.position).normalized;
        spriteRenderer = GetComponent<SpriteRenderer>();
        alpha = spriteRenderer.color.a;
        audio = GetComponent<AudioSource>();
        isCharging = false;
        boomAudio = transform.GetChild(0).GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hp <= 0) ChangeState(RedBatState.Dead);
        switch (currentState)
        {
            case RedBatState.Moving:
                Move();
                break;
            case RedBatState.Charging:
                Charge();
                break;
            case RedBatState.Rushing:
                Rush();
                break;
            case RedBatState.Dead:
                Dead();
                break;
        }

        SetAnimator();
    }

    private void Move()
    {
        moveVector = (tower.transform.position - transform.position).normalized;
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector3.up, radius, layerMask);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            attackPoint = hit.point;
            ChangeState(RedBatState.Charging);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        if (!isCharging) { isCharging = true; SoundManager.instance.PlaySound("bat"); }
        moveVector = (attackPoint - transform.position).normalized;
        transform.Translate(-moveVector * chargingSpeed * Time.deltaTime);
        yield return new WaitForSeconds(chargeDelay);
        ChangeState(RedBatState.Rushing);
    }

    private void Charge()
    {
        StartCoroutine(AttackCoroutine());
    }

    private void Rush()
    {
        transform.Translate(moveVector * rushSpeed * Time.deltaTime);
    }

    private void Dead()
    {
        if (!isDead)
        {
            isDead = true;
        }
        collider.enabled = false;
        rigidbody.gravityScale = 1;
        alpha -= (fadeSpeed * Time.deltaTime);

        Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        spriteRenderer.color = newColor;

        if (alpha <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void ChangeState(RedBatState newState)
    {
        currentState = newState;
    }


    private void SetAnimator()
    {
        if (moveVector.x < 0) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
        switch (currentState)
        {
            case RedBatState.Charging:
                animator.SetBool("isAttack", true);
                break;
            case RedBatState.Rushing:
                animator.SetBool("isRush", true);
                break;
            case RedBatState.Dead:
                animator.SetBool("isDead", true);
                break;
        }
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }*/

    private IEnumerator BoomCoroutine()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            //boomAudio.Play();
            SoundManager.instance.PlaySound("hitMonster");
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
            spriteRenderer.color = newColor;
            GameManager.Instance.SetTowerHp(-damage);
            StartCoroutine(BoomCoroutine());
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            hp -= bullet.BulletDamage;
            bullet.PiercingCount--;
        }
    }
}
