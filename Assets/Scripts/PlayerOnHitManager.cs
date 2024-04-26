using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerOnHitManager : MonoBehaviour
{
    public static PlayerOnHitManager instance;
    public float invincibleTime = 2f;
    public float flickeringDelay = 0.2f;
    public bool onHitHandled = false;   //중복 충돌 체크
    public bool playerStuned = false;
    public bool playerFainted = false;
    public float stunTime = 0.7f;
    public float faintTime = 2f;


    public GameObject player;
    public SpriteRenderer playerSprite;
    public Rigidbody2D playerRigidbody;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
    }

    public void OnHit(Collider2D collision)
    {
        StartCoroutine(OnHitCoroutine());
        StartCoroutine(FlickeringCoroutine());
        StartCoroutine(InvincibleCoroutine());
        StartCoroutine(StunCoroutine());
        KnockBack(collision);
    }

    public void Faint()
    {
        StartCoroutine(FaintCoroutine());
    }


    public IEnumerator OnHitCoroutine()
    {
        yield return new WaitForSeconds(invincibleTime);
        onHitHandled = false;
    }


    private IEnumerator FlickeringCoroutine()
    {
        int countTime = 0;
        float maxCount = invincibleTime / flickeringDelay;
        while (countTime < maxCount)
        {
            if (countTime % 2 == 0)
            {
                playerSprite.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                playerSprite.color = new Color32(255, 255, 255, 180);
            }
            yield return new WaitForSeconds(flickeringDelay);
            countTime++;
        }
        playerSprite.color = new Color32(255, 255, 255, 255);
    }

    private IEnumerator InvincibleCoroutine()
    {
        player.layer = LayerMask.NameToLayer("Invincible");
        yield return new WaitForSeconds(invincibleTime);
        player.layer = LayerMask.NameToLayer("Player");
    }

    private void KnockBack(Collider2D collision)
    {
        if (onHitHandled) return;
        onHitHandled = true;
        Vector2 knockback = Vector2.zero;
        if (collision.gameObject.transform.position.x > transform.position.x)
        {
            knockback = new Vector2(-100f, 400f);
        }
        else { knockback = new Vector2(100f, 400f); }
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.AddForce(knockback);
    }

    private IEnumerator StunCoroutine()
    {
        playerStuned = true;
        yield return new WaitForSeconds(stunTime);
        playerStuned = false;
    } 

    private IEnumerator FaintCoroutine() 
    {
        playerFainted = true;
        yield return new WaitForSeconds(faintTime);
        StartCoroutine(InvincibleCoroutine());
        StartCoroutine(FlickeringCoroutine());
        playerFainted = false;
    }


}
