using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    public List<GameObject> inventory;
    public Image itemIcon;    //UI에 표시할 이미지 
    public List<Sprite> itemSprites;   //UI에 표시할 스프라이트 리스트
    public int inventoryIndex;
    public GameObject deployField;

    private void Start()
    {
        inventoryIndex = 0;
        inventory = new List<GameObject>();
        itemSprites = new List<Sprite>();
        AddItem(ItemManager.Instance.itemPrefabs[3]);
    }

    public void EquipItem()
    {
        if (inventory.Count != 0) itemIcon.sprite = itemSprites[inventoryIndex];
    }

    public void UnEquipItem(GameObject gameObject)
    {

    }

    public void AddItem(GameObject gameObject)
    {
        inventory.Add(gameObject);
        itemSprites.Add(gameObject.GetComponent<SpriteRenderer>().sprite);

        EquipItem();
    }

    public void deployItem()
    {
        //현재 아이템 인덱스로 설치된 아이템이 있는지 검사한다.
        Transform deployedItem = null;
        foreach(Transform child in deployField.transform)
        {
            if(child.GetComponent<Item>().DeployIndex == inventoryIndex)
            {
                deployedItem = child;
                break;
            }
        }

        if( deployedItem != null) 
        {
            //Debug.Log("있음! 파괴한다!");
            Destroy(deployedItem.gameObject);
        }
        else
        {
            //Debug.Log("없음");
        }
        GameObject item = Instantiate(inventory[inventoryIndex], gameObject.transform.position, Quaternion.identity);
        item.SetActive(true);
        item.GetComponent<Item>().DeployIndex = inventoryIndex;
        item.transform.parent = deployField.transform;


    }

    public void OnDeploy(InputValue inputValue)
    {
        if (inputValue.Get() != null)
        {
            if(inventory.Count != 0) deployItem();
        }
    }

    public void OnChangeItem()
    {
        inventoryIndex = ++inventoryIndex % inventory.Count;
        EquipItem();
    }
}
