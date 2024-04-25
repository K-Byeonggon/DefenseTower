using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D platformEffector;
    private bool isPlayerOnIt;

    private void Start()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }


    private IEnumerator JumpDown()
    {
        platformEffector.rotationalOffset = 180;
        yield return new WaitForSeconds(0.5f);
        platformEffector.rotationalOffset = 0;
    }

    private void OnDown(InputValue inputValue)
    {
        if(isPlayerOnIt) StartCoroutine(JumpDown());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player") 
        {
            isPlayerOnIt = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            isPlayerOnIt = false;
        }
    }
}
