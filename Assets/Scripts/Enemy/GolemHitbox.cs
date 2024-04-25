using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 100f;
    [SerializeField] private AudioSource punchSound;
    
    private void OnEnable()
    {
        transform.Translate(new Vector2(0, 0.001f));
    }

    private void Start()
    {
        punchSound = GetComponent<AudioSource>();
    }


    private IEnumerator ActiveCoroutine()
    {
        yield return new WaitForSeconds(2.3f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tower")
        {
            //punchSound.Play();
            SoundManager.instance.PlaySound("GolemPunch");
            GameManager.Instance.SetTowerHp(-damage);
            StartCoroutine(ActiveCoroutine());
        }
    }
}
