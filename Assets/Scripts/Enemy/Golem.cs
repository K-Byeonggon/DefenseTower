using StateEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{

    [SerializeField] private GolemState currentState;

    public float radius = 4f;
    public int layerMask;
    public GameObject tower;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D collider;

    [SerializeField] private float moveSpeed = 0.5f;

    [SerializeField] private Vector2 moveVector;
    [SerializeField] private float hitboxOntime = 0.5f;
    [SerializeField] private float attackCooltime = 2.5f;
    [SerializeField] private bool attacking = false;

    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float maxHp = 500f;
    [SerializeField] private float hp = 500f;
    [SerializeField] private float alpha;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool towerFound = false;
    [SerializeField] private bool isRoaring;

    private void OnEnable()
    {
        if (collider != null) collider.enabled = true;
        currentState = GolemState.Moving;
        hp = maxHp;
        alpha = 1f;
        if (spriteRenderer != null)
        {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            spriteRenderer.color = newColor;
        }
        if(rigidbody != null) { rigidbody.gravityScale = 30; }
        isRoaring = false;
    }


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        moveVector = (tower.transform.position - transform.position).normalized;
        spriteRenderer = GetComponent<SpriteRenderer>();
        alpha = spriteRenderer.color.a;
        layerMask = LayerMask.GetMask("Tower");
        isRoaring = false;
    }

    void Update()
    {
        if (hp <= 0) ChangeState(GolemState.Dead);
        switch (currentState)
        {
            case GolemState.Idle:
                Idle();
                break;
            case GolemState.Rore:
                Rore();
                break;
            case GolemState.Moving:
                Move();
                break;
            case GolemState.Attacking:
                Attack();
                break;
            case GolemState.Dead:
                Dead();
                break;
        }
        SetAnimator();
    }

    private void Idle()
    {

    }

    private void Move()
    {
        Vector2 rayVector = (tower.transform.position - transform.position).x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 2f, 0), rayVector, radius, layerMask);
        Debug.DrawRay(transform.position + new Vector3(0, 2f, 0), rayVector * radius, Color.red);

        if (hit.collider != null)
        {
            moveVector = Vector2.zero;
            ChangeState(GolemState.Attacking);
            
        }
        else
        {
            moveVector = Vector2.right;
            if(rayVector.x > 0) { transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); }
            else if(rayVector.x < 0) { transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f)); }
        }
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);

    }

    private IEnumerator RoreCoroutine()
    {
        if (!isRoaring) { isRoaring = true; SoundManager.instance.PlaySound("Roar"); }
        yield return new WaitForSeconds(2f);
        towerFound = true;
    }
    private void Rore()
    {
        StartCoroutine(RoreCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        if (!towerFound)
        {
            ChangeState(GolemState.Rore);
            yield return new WaitForSeconds(2f);
            towerFound = true;
            ChangeState(GolemState.Attacking);
        }
        attacking = true;
        yield return new WaitForSeconds(0.3f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxOntime);
        transform.GetChild(0).gameObject.SetActive(false);
        ChangeState(GolemState.Idle);
        yield return new WaitForSeconds(attackCooltime);
        ChangeState(GolemState.Moving);
        attacking = false;
    }

    private void Attack()
    {
        if (!attacking) StartCoroutine(AttackCoroutine());
    }

    private void Dead()
    {
        if (!isDead)
        {
            collider.enabled = false;
            rigidbody.gravityScale = 0;
            isDead = true;
        }
        alpha -= (fadeSpeed * Time.deltaTime);

        Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        spriteRenderer.color = newColor;

        if (alpha <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void ChangeState(GolemState newState)
    {
        currentState = newState;
    }


    private void SetAnimator()
    {
        switch (currentState)
        {
            case GolemState.Idle:
                animator.SetBool("isAttack", false);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRore", false);
                break;
            case GolemState.Moving:
                animator.SetBool("isAttack", false);
                animator.SetBool("isMoving", true);
                animator.SetBool("isRore", false);
                break;
            case GolemState.Rore:
                animator.SetBool("isAttack", false);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRore", true);
                break;
            case GolemState.Attacking:
                animator.SetBool("isAttack", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRore", false);
                break;
            case GolemState.Dead:
                animator.SetBool("isAttack", false);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRore", false);
                break;
        }
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
