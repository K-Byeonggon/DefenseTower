using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarControll : MonoBehaviour
{
    public List<GameObject> obj;
    public List<GameObject> hpBar;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < obj.Count; i++)
        {
            hpBar[i].transform.position = obj[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < obj.Count; i++)
        {
            if(GameManager.Instance.mainCamera != null)
            {
                hpBar[i].transform.position = GameManager.Instance.mainCamera.WorldToScreenPoint(obj[i].transform.position + new Vector3(0, 1f, 0));
            }
        }
    }
}
