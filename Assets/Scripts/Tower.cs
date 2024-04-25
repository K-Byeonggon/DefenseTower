using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Slider towerHpBar;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        towerHpBar.maxValue = GameManager.Instance.towerMaxHp;
        towerHpBar.value = GameManager.Instance.towerHp;
    }

    private void SetHpBar()
    {
        towerHpBar.value = GameManager.Instance.towerHp;
    }
    // Update is called once per frame
    void Update()
    {
        SetHpBar();
        SetAnimator();
    }
    private void SetAnimator()
    {
        float towerHpRatio = GameManager.Instance.towerHp / GameManager.Instance.towerMaxHp;
        if (towerHpRatio > 0.66) { animator.SetBool("undamaged", true); animator.SetBool("below66", false); animator.SetBool("below33", false); animator.SetBool("crushed", false); }
        else if (towerHpRatio > 0.33) { animator.SetBool("undamaged", false); animator.SetBool("below66", true); animator.SetBool("below33", false); animator.SetBool("crushed", false); }
        else if (towerHpRatio > 0) { animator.SetBool("undamaged", false); animator.SetBool("below66", false); animator.SetBool("below33", true); animator.SetBool("crushed", false); }
        else { animator.SetBool("undamaged", false); animator.SetBool("below66", false); animator.SetBool("below33", false); animator.SetBool("crushed", true); }
    }
}
