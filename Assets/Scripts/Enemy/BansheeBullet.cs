using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheeBullet : MonoBehaviour
{
    private Collider2D collider;
    private float alpha;
    private float fadeSpeed = 2f;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector2 moveVector;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 targetVector;
    [SerializeField] public GameObject banshee;
    [SerializeField] private float damage = 10;

    private void OnEnable()
    {
        if(collider != null) collider.enabled = true;
        alpha = 1f;
        if(spriteRenderer != null)
        {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            spriteRenderer.color = newColor;
        }
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        moveSpeed = 1.5f;
        moveVector = Vector2.zero;
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        alpha = spriteRenderer.color.a;
    }

    void Update()
    {
        if(banshee != null)
        {
            if (!banshee.GetComponent<Collider2D>().enabled) FadeOut();
        }
        Move();
    }

    private void Move()
    {
        targetVector = (target.transform.position - transform.position).normalized;
        moveVector = (targetVector + moveVector).normalized;
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);
    }

    private void FadeOut()
    {
        collider.enabled = false;
        alpha -= (fadeSpeed * Time.deltaTime);

        Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        spriteRenderer.color = newColor;

        if (alpha <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerOnHitManager.instance.onHitHandled) return;

        if (collision.tag == "Player") 
        {
            gameObject.SetActive(false);
            banshee.GetComponent<Banshee>().currentBulletCount--;
            collision.gameObject.GetComponent<PlayerHp>().hp -= damage;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerOnHitManager.instance.onHitHandled) return;

        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            banshee.GetComponent<Banshee>().currentBulletCount--;
            collision.gameObject.GetComponent<PlayerHp>().hp -= damage;
        }
    }
}
