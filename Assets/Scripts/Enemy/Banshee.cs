using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnums;
public class Banshee : MonoBehaviour
{

    [SerializeField] private BansheeState currentState;

    public float radius = 4f;
    public LayerMask layerMask;
    [SerializeField] private GameObject target;

    public GameObject bansheeBullet;
    private Queue<GameObject> bulletPool;
    private int bulletCount = 3;
    private Vector3 poolPoint;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D collider;

    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private Vector2 moveVector;
    [SerializeField] private Vector3 attackPoint;
    [SerializeField] private float attackDelay = 4f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float hp = 100f;
    [SerializeField] private float alpha;
    [SerializeField] private bool attacking = false;
    public int currentBulletCount = 0;

    private bool isDead = false;


    private void OnEnable()
    {
        isDead = false;
        if (collider != null) collider.enabled = true;
        currentState = BansheeState.Moving;
        hp = maxHp;
        if(rigidbody != null) rigidbody.gravityScale = 0;
        alpha = 1f;
        if(spriteRenderer != null)
        {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            spriteRenderer.color = newColor;
        }
    }


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        moveVector = (target.transform.position - transform.position).normalized;
        spriteRenderer = GetComponent<SpriteRenderer>();
        alpha = spriteRenderer.color.a;

        poolPoint = new Vector3(0, -15, 0);
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < bulletCount; i++) 
        {
            GameObject bullet = Instantiate(bansheeBullet, poolPoint, Quaternion.identity);
            bullet.GetComponent<BansheeBullet>().banshee = gameObject;
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    void Update()
    {
        if (hp <= 0) ChangeState(BansheeState.Dead);
        switch (currentState)
        {
            case BansheeState.Moving:
                Move();
                break;
            case BansheeState.Attacking:
                Attack();
                break;
            case BansheeState.Dead:
                Dead();
                break;
        }

        SetAnimator();
    }

    private void Move()
    {
        moveVector = (target.transform.position - transform.position).normalized;
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);
        
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector3.up, radius, layerMask);

        if (hit.collider != null && GameManager.Instance.InCamera(gameObject) && currentBulletCount < 3)
        {
            attackPoint = hit.point;
            ChangeState(BansheeState.Attacking);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        attacking = true;
        GameObject bullet = bulletPool.Dequeue();
        bulletPool.Enqueue(bullet);
        bullet.SetActive(false);
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        currentBulletCount++;
        SoundManager.instance.PlaySound("howl");
        yield return new WaitForSeconds(attackDelay);
        attacking = false;
        ChangeState(BansheeState.Moving);
    }

    private void Attack()
    {
        if(!attacking) StartCoroutine(AttackCoroutine());
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

    private void ChangeState(BansheeState newState)
    {
        currentState = newState;
    }


    private void SetAnimator()
    {
        if (moveVector.x < 0) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
        switch (currentState)
        {
            case BansheeState.Moving:
                animator.SetBool("isAttack", false);
                break;
            case BansheeState.Attacking:
                animator.SetBool("isAttack", true);
                break;
            case BansheeState.Dead:
                animator.SetBool("isDead", true);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            hp -= bullet.BulletDamage;
            bullet.PiercingCount--;
        }
    }
}
