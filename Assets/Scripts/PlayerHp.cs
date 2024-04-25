using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public float maxHp = 50;
    public float hp = 50;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private PlayerController controller;
    private Animator animator;
    public Slider hpBar;

    [SerializeField] private float invincibleTime = 2f;
    [SerializeField] private float flickeringDelay = 0.2f;


    public bool eventHandled = false;

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
    }

    private void Update()
    {
        SetHpBar();
    }


    private IEnumerator HitStunCoroutine()
    {
        SoundManager.instance.PlaySound("hitPlayer");
        controller.Attacked = true;
        controller.MoveSpeed = 0f;
        yield return new WaitForSeconds(0.7f);
        controller.Attacked = false;
        controller.MoveSpeed = 10f;
    }

    private IEnumerator StunCoroutine()
    {
        SoundManager.instance.PlaySound("hitPlayer");
        gameObject.layer = LayerMask.NameToLayer("Invincible");
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

    public void SetEventHandled()
    {
        eventHandled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyBullet")
        {

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet")
        {

        }
    }
}
