using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnums;

public class SwordKnight : MonoBehaviour
{

    [SerializeField] private SwordKnightState currentState;

    public float radius;
    public float randomDelta;
    public int layerMask;
    public GameObject tower;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D collider;

    [SerializeField] private float moveSpeed = 1.5f;

    [SerializeField] private Vector2 moveVector;
    [SerializeField] private Vector2 rayVector;
    [SerializeField] private float hitboxOntime = 0.6f;
    [SerializeField] private float attackCooltime = 2.5f;
    [SerializeField] private bool attacking = false;

    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float maxHp = 50f;
    [SerializeField] private float hp = 50f;
    [SerializeField] private float alpha;
    [SerializeField] private bool isDead = false;
    [SerializeField] private AudioSource audio;

    private void OnEnable()
    {
        isDead = false;
        randomDelta = Random.Range(0, 0.5f);
        radius = 2f - randomDelta;
        if (collider != null) collider.enabled = true;
        currentState = SwordKnightState.Moving;
        hp = maxHp;
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
        layerMask = (1 << LayerMask.NameToLayer("Tower")) | (1 << LayerMask.NameToLayer("Player"));
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hp <= 0) ChangeState(SwordKnightState.Dead);
        switch (currentState)
        {
            case SwordKnightState.Idle:
                Idle();
                break;
            case SwordKnightState.Moving:
                Move();
                break;
            case SwordKnightState.Attacking:
                Attack();
                break;
            case SwordKnightState.Dead:
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
        rayVector = (tower.transform.position - transform.position).x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), rayVector, radius, layerMask);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), rayVector * radius, Color.red);

        if (hit.collider != null)
        {
            moveVector = Vector2.zero;

            ChangeState(SwordKnightState.Attacking);
        }
        else
        {
            moveVector = Vector2.right;
            if (rayVector.x > 0) { transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); }
            else if (rayVector.x < 0) { transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f)); }
        }
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);

    }

    private IEnumerator SoundTestCoroutine()
    {
        SoundManager.instance.PlaySound("blade");
        yield return null;
    }

    private IEnumerator AttackCoroutine()
    {
        attacking = true;
        yield return new WaitForSeconds(0.2f);
        //audio.Play();
        //SoundManager.instance.PlaySound("blade");
        StartCoroutine(SoundTestCoroutine());
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxOntime);
        transform.GetChild(0).gameObject.SetActive(false);
        ChangeState(SwordKnightState.Idle);
        yield return new WaitForSeconds(attackCooltime);
        ChangeState(SwordKnightState.Moving);
        attacking = false;
    }

    private void Attack()
    {
        if(!attacking) StartCoroutine(AttackCoroutine());
    }

    private void Dead()
    {
        if (!isDead)
        {
            GameManager.Instance.defeatedEnemyCount++;
            
            collider.enabled = false;
            Vector2 knockback = -rayVector * 100f + Vector2.up * 200;
            rigidbody.AddForce(knockback);
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

    private void ChangeState(SwordKnightState newState)
    {
        currentState = newState;
    }


    private void SetAnimator()
    {
        switch (currentState)
        {
            case SwordKnightState.Idle:
                animator.SetBool("isAttack", false);
                break;
            case SwordKnightState.Moving:
                animator.SetBool("isAttack", false);
                break;
            case SwordKnightState.Attacking:
                animator.SetBool("isAttack", true);
                break;
            case SwordKnightState.Dead:
                animator.SetBool("isDead", true);
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
