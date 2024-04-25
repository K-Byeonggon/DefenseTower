using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : Item
{
    public GameObject bulletPrefab;
    [SerializeField] protected Queue<GameObject> bulletPool = new Queue<GameObject>();
    private SpriteRenderer spriteRenderer;
    private Animator muzzleAnimator;

    [SerializeField] protected int bulletCount;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletDelay;
    [SerializeField] protected float spreadness;
    [SerializeField] protected int bulletsPerShot;      //샷건처럼 한방에 여러발 나가는 경우
    [SerializeField] protected float randomSpeedRange;  //샷건처럼 한방에 총알 속도가 다른 경우

    [SerializeField] protected Vector2 poolPosition = new Vector2(0, -10);
    [SerializeField] protected bool isFiring = false;
    [SerializeField] protected Vector2 mouseVector;
    [SerializeField] protected float mouseAngle;
    [SerializeField] protected float mouseSpeed;
    [SerializeField] protected bool isCoroutine = false;
    [SerializeField] private int currentBullet;

    [SerializeField] private AudioSource audio;

    public Gun()
    {
        bulletCount = 20;
        bulletSpeed = 20f;
        bulletDelay = 0.25f;
        spreadness = 0f;
        bulletsPerShot = 10;
        randomSpeedRange = 1f;
    }

    private void OnEnable()
    {
        isFiring = false;
        currentBullet = 0;
        isCoroutine = false;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, poolPosition, Quaternion.identity);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
        muzzleAnimator = transform.GetChild(0).GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }


    void Update()
    {
        Fire();
    }

    private void OnFire(InputValue inputValue)
    {
        
            if (inputValue.Get() != null)
            {
                if (!GameManager.Instance.isUnboxing && !GameManager.Instance.isPaused) isFiring = true;
            }
            else
            {
                isFiring = false;
            }

    }

    public virtual IEnumerator FireCoroutine()
    {
        currentBullet = 0;
        isCoroutine = true;
        GameObject bullet = bulletPool.Peek();
        while (currentBullet < bulletsPerShot)
        {
            bullet = bulletPool.Dequeue();
            bulletPool.Enqueue(bullet);
            bullet.SetActive(false);
            bullet.SetActive(true);
            bullet.transform.position = transform.GetChild(0).position;
            float bulletAngle = (spreadness == 0) ? mouseAngle : mouseAngle + Random.Range(-spreadness, spreadness);
            Vector2 bulletVector = (spreadness == 0) ? mouseVector.normalized : (Quaternion.Euler(0, 0, bulletAngle) * Vector2.right).normalized;
            float randomSpeed = Random.Range(randomSpeedRange, 1);
            bullet.transform.rotation = Quaternion.AngleAxis(bulletAngle, Vector3.forward);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletVector * bulletSpeed * randomSpeed;

            currentBullet++;
        }

        muzzleAnimator.SetTrigger("onFire");
        //audio.Play();
        PlayFireSound();
        yield return new WaitForSeconds(bulletDelay);

        isCoroutine = false;
    }

    public virtual void PlayFireSound()
    {
        SoundManager.instance.PlaySound("gun");
    }

    public virtual void Fire()
    {
        if (isFiring)
        {
            if (!isCoroutine)
            {
                StartCoroutine(FireCoroutine());
            }
        }
    }

    private void OnAimming(InputValue inputValue)
    {
        mouseVector = Camera.main.ScreenToWorldPoint(inputValue.Get<Vector2>());
        mouseVector -= new Vector2(transform.parent.position.x, transform.parent.position.y);
        mouseAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        //Debug.Log(mouseVector + " " + mouseAngle);
        if(spriteRenderer != null)
        {
            if (Mathf.Abs(mouseAngle) < 90)
            {
                spriteRenderer.flipY = false;
            }
            else
            {
                spriteRenderer.flipY = true; 
            }
        }
        Quaternion rotation = Quaternion.AngleAxis(mouseAngle, Vector3.forward);
        transform.parent.rotation = rotation;
    }
}
