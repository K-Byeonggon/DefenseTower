using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnums;

public class Turret : Item
{
    public GameObject bulletPrefab;
    [SerializeField] protected Queue<GameObject> bulletPool = new Queue<GameObject>();
    
    [SerializeField] protected int bulletCount;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletDelay;
    [SerializeField] protected float spreadness;
    [SerializeField] protected int bulletsPerShot;      //샷건처럼 한방에 여러발 나가는 경우
    [SerializeField] protected float randomSpeedRange;  //샷건처럼 한방에 총알 속도가 다른 경우

    [SerializeField] protected Vector2 poolPosition = new Vector2(0, -10);
    [SerializeField] protected bool isFiring = false;
    [SerializeField] protected bool isCoroutine = false;
    [SerializeField] private int currentBullet;
    [SerializeField] private TurretState currentState;

    [SerializeField] private float barrelRotationSpeed;
    [SerializeField] private float radius;
    [SerializeField] private Vector2 targetVector;
    public LayerMask layerMask;
    [SerializeField] private bool trackDelay;
    [SerializeField] private Vector3 toEnemy;

    [SerializeField] private AudioSource audio;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Turret()
    {
        bulletCount = 10;
        bulletSpeed = 20f;
        bulletDelay = 0.5f;
        spreadness = 0f;
        bulletsPerShot = 1;
        randomSpeedRange = 1f;
        barrelRotationSpeed = 50f;
        radius = 8f;
    }

    private void OnEnable()
    {
        isFiring = false;
        currentBullet = 0;
        isCoroutine = false;
    }

    private void Start()
    {
        currentState = TurretState.Idle;
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, poolPosition, Quaternion.identity);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
            toEnemy = Vector3.zero;
        }
        trackDelay = false;
        audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        /*
        Debug.Log(currentState);
        
        if(currentState == TurretState.Attacking)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }*/

        switch (currentState)
        {
            case TurretState.Idle:
                Idle();
                break;
            case TurretState.Tracking:
                Track();
                break;
            case TurretState.Attacking:
                Attack();
                break;
        }
    }

    private void Idle()
    {
        //원형캐스트로 제일 먼저 감지되는 적을 정한다.
        RaycastHit2D circleHit = Physics2D.CircleCast(transform.position, radius, Vector3.up, radius, layerMask);

        if (circleHit.collider != null && GameManager.Instance.InCamera(circleHit.collider.gameObject))
        {
            targetVector = (circleHit.collider.transform.position - transform.position).normalized;
            ChangeState(TurretState.Tracking);
        }
    }

    private IEnumerator TrackDelaycoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        trackDelay = false;
    }

    private void Track()
    {
        //Idle상태에서 검출한 적을 향해 바로 직선ray를 쏜다. ray는 계속 적을 추적한다.
        RaycastHit2D linearHit = Physics2D.Raycast(transform.position, targetVector, radius, layerMask);
        Debug.DrawRay(transform.position, targetVector * radius, Color.white);

        //적을 추적하는 직선 Ray와 포신의 방향이 어느정도 같아질때까지 포신을 돌리며 적을 추격한다.
        //1. 직선 Ray와 포신의 방향 각도 계산
        //2. 차이가 있으면 돌린다.
        //
        //Debug.DrawRay(transform.position, transform.GetChild(0).up * radius, Color.red);
        if (linearHit.collider != null)
        {
            targetVector = (linearHit.collider.transform.position - transform.position).normalized;

            Quaternion deltaRotation = Quaternion.FromToRotation(Vector3.up, targetVector);
            if (deltaRotation.z > transform.GetChild(0).rotation.z + 0.005f)
            {
                transform.GetChild(0).Rotate(Vector3.forward, barrelRotationSpeed * Time.deltaTime, Space.Self);
            }
            else if (deltaRotation.z < transform.GetChild(0).rotation.z - 0.005f)
            {
                transform.GetChild(0).Rotate(Vector3.forward, -barrelRotationSpeed * Time.deltaTime, Space.Self);
            }
            else
            {
                Debug.Log("Track에서 Idle로");
                ChangeState(TurretState.Attacking);
            }
        }
        else    //만약 적방향 ray에 검출이 null이면 다시 Idle이 된다.
        {
            ChangeState(TurretState.Idle);
        }
    }

    private IEnumerator AttackCoroutine()
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
            bullet.transform.position = transform.position;

            Vector2 bulletVector = transform.GetChild(0).up;

            float randomSpeed = Random.Range(randomSpeedRange, 1);
            bullet.transform.rotation = transform.GetChild(0).rotation;
            bullet.GetComponent<Rigidbody2D>().velocity = bulletVector * bulletSpeed * randomSpeed;

            currentBullet++;
        }
        //audio.Play();
        SoundManager.instance.PlaySound("Turret");
        yield return new WaitForSeconds(bulletDelay);

        isCoroutine = false;
    }

    private void Attack()
    {
        Debug.DrawRay(transform.position, transform.GetChild(0).up * radius, Color.red);
        RaycastHit2D barrelHit = Physics2D.Raycast(transform.position, transform.GetChild(0).up, radius, layerMask);
        /*
        Debug.Log("검출한거" + barrelHit.collider.gameObject.name);
        Debug.Log("barrelHit.collider == null: " + (barrelHit.collider == null));
        Debug.Log("barrelHit.collider.gameObject == null: " + (barrelHit.collider.gameObject == null));
        */
        if (barrelHit.collider == null)
        {
            Debug.Log("Attack에서 Idle로!");
            ChangeState(TurretState.Idle);
            return;
        }
        if (!isCoroutine)
        {
            StartCoroutine(AttackCoroutine());
        }
        
    }

    private void ChangeState(TurretState newState)
    {
        currentState = newState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
