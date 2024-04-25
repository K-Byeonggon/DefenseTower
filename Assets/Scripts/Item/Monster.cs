using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float hp = 100f;
    public float Hp { get { return hp; }  }
    
    private Animator animator;

    private void OnEnable()
    {
        hp = maxHp;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetHp(float delta)
    {
        hp += delta;
    }

    private IEnumerator DeadCoroutine()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(hp <= 0) { StartCoroutine(DeadCoroutine()); }
    }
}
