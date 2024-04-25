using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public float maxHp = 50;
    public float hp = 50;
    public bool invincible = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private PlayerController controller;
    private Animator animator;
    public Slider hpBar;
    private AudioSource onHitAudio;

    private void SetHpBar()
    {
        if (hpBar != null)
        {
            hpBar.value = hp;
        }
    }


    private void OnEnable()
    {
        hp = maxHp;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        controller = gameObject.GetComponent<PlayerController>();
        hpBar.maxValue = maxHp;
        hpBar.value = hp;
        hp = maxHp;
        onHitAudio = transform.GetChild(2).GetComponent<AudioSource>();
    }

    private void Update()
    {
        SetHpBar();
    }

    private IEnumerator DamagedCoroutine()
    {
        invincible = true;
        int countTime = 0;
        Color originalColor = spriteRenderer.color;
        while (countTime < 10)
        {
            if(countTime%2 == 0)
            {
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            }
            yield return new WaitForSeconds(0.2f);
            countTime++;
        }
        spriteRenderer.color = new Color32(255,255,255,255);
        yield return null;
        invincible = false;
    }
    private IEnumerator HitStunCoroutine()
    {
        //onHitAudio.Play();
        SoundManager.instance.PlaySound("hitPlayer");
        controller.Attacked = true;
        controller.MoveSpeed = 0f;
        yield return new WaitForSeconds(0.7f);
        controller.Attacked = false;
        controller.MoveSpeed = 10f;
    }

    private IEnumerator StunCoroutine()
    {
        //onHitAudio.Play();
        SoundManager.instance.PlaySound("hitPlayer");
        invincible = true;
        controller.Attacked = true;
        controller.MoveSpeed = 0f;
        controller.JumpForce = 0f;
        animator.SetBool("isStuned", true);
        yield return new WaitForSeconds(3f);
        hp = maxHp;
        animator.SetBool("isStuned", false);
        controller.Attacked = false;
        controller.MoveSpeed = 10f;
        controller.JumpForce = 800f;
        controller.JumpCount = 0;
        Debug.Log(controller.JumpForce);
        StartCoroutine(DamagedCoroutine());
    }

    private void Damaged(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet" && !invincible)
        {
            Vector2 knockback = Vector2.zero;
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                knockback = new Vector2(-100f, 400f);
            }
            else { knockback = new Vector2(100f, 400f); }
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(knockback);
            if (hp <= 0)
            {
                StartCoroutine(StunCoroutine());
            }
            else
            {
                StartCoroutine(DamagedCoroutine());
                StartCoroutine(HitStunCoroutine());
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damaged(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Damaged(collision);
    }
}
