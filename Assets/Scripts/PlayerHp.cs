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

    public void SetEventHandled()
    {
        eventHandled = false;
    }

    private IEnumerator SetFaintRecovery()
    {
        yield return new WaitForSeconds(PlayerOnHitManager.instance.faintTime);
        hp = maxHp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyBullet")
        {
            PlayerOnHitManager.instance.OnHit(collision);
            if (hp <= 0)
            {
                PlayerOnHitManager.instance.Faint();
                StartCoroutine(SetFaintRecovery());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet")
        {
            PlayerOnHitManager.instance.OnHit(collision);
            if (hp <= 0)
            {
                PlayerOnHitManager.instance.Faint();
                StartCoroutine(SetFaintRecovery());
            }
        }
    }
}
