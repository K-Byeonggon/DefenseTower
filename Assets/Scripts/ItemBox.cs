using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private bool boxOpened;
    [SerializeField] private Animator animator;
    public GameObject boxUI;

    private void OnEnable()
    {
        boxOpened = false;
        transform.position = Vector3.zero;
    }

    private void Start()
    {
        transform.position = Vector3.zero;
        boxOpened = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            Debug.Log("ºÎ‹HÈû!");
            if (!boxOpened) 
            {
                boxOpened = true;
                boxUI.SetActive(true);
            }
        }
    }

}
