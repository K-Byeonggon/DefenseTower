using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordKnightHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnEnable()
    {
        transform.Translate(new Vector2(0, 0.001f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<PlayerHp>().invincible)
            {
                gameObject.SetActive(false);
                collision.gameObject.GetComponent<PlayerHp>().hp -= damage;
            }
        }
        else if (collision.tag == "Tower")
        {
            gameObject.SetActive(false);
            GameManager.Instance.SetTowerHp(-damage);
        }
    }

}
