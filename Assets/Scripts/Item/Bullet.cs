using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float bulletDamage;
    [SerializeField] protected int piercingCount;

    public float BulletDamage { get {  return bulletDamage; } set { bulletDamage = value; } }
    public int PiercingCount { get {  return piercingCount; } set { piercingCount = value; } }
    

    private void Update()
    {
        if(!GameManager.Instance.InCamera(gameObject)) gameObject.SetActive(false); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Monster" && collision.tag != "Platform") return;
        if (piercingCount <= 0) gameObject.SetActive(false);
        else piercingCount--;
    }
}
