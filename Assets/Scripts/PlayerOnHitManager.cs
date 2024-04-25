using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerOnHitManager : MonoBehaviour
{
    public static PlayerOnHitManager instance;
    public float invincibleTime = 2f;
    public float flickeringDelay = 0.2f;
    public bool onHitHandled = false;


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

    public void OnHit()
    {
        StartCoroutine(OnHitCoroutine());
        StartCoroutine(FlickeringCoroutine());
        StartCoroutine(InvincibleCoroutine());
    }

    private IEnumerator OnHitCoroutine()
    {
        onHitHandled = true;
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
        player.layer = LayerMask.NameToLayer("Invinvible");
        yield return new WaitForSeconds(invincibleTime);
        player.layer = LayerMask.NameToLayer("Player");
    }

    private void KnockBack(Collision collision)
    {
        Vector2 knockback = Vector2.zero;
        if (collision.gameObject.transform.position.x > transform.position.x)
        {
            knockback = new Vector2(-100f, 400f);
        }
        else { knockback = new Vector2(100f, 400f); }
        GetComponent<Rigidbody>().velocity = Vector2.zero;
        GetComponent<Rigidbody>().AddForce(knockback);
    }


}
