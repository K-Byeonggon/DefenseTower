using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<GameObject> inventory; //�ѱ� �������� ���纻�� ����.
    public Image weaponIcon;    //UI�� ǥ���� �̹��� 
    public List<Sprite> itemSprites;   //UI�� ǥ���� ��������Ʈ ����Ʈ
    public int inventoryIndex;
    public Transform gunAxis;

    private void Start()
    {
        inventory = new List<GameObject>();
        itemSprites = new List<Sprite>();
        inventoryIndex = 0;
        gunAxis = transform.GetChild(0);
        AddItem(ItemManager.Instance.itemPrefabs[0]);
        EquipItem(inventory[inventoryIndex]);
    }

    public void EquipItem(GameObject gameObject)
    {
        gunAxis.GetChild(inventoryIndex).gameObject.SetActive(true);
        weaponIcon.sprite = itemSprites[inventoryIndex];
    }

    public void UnEquipItem(GameObject gameObject)
    {
        gunAxis.GetChild(inventoryIndex).gameObject.SetActive(false);
    }

    public void AddItem(GameObject gameObject)
    {
        inventory.Add(gameObject);
        GameObject weapon = Instantiate(gameObject, new Vector3(0, -10, 0), Quaternion.identity);
        weapon.SetActive(false);
        weapon.transform.SetParent(gunAxis);
        weapon.transform.localPosition = new Vector3(1, 0, 0);
        weapon.transform.localRotation = Quaternion.identity;

        itemSprites.Add(weapon.GetComponent<SpriteRenderer>().sprite);
    }

    private void OnChangeWeapon(InputValue inputValue)
    {
        if (inputValue.Get<float>() > 0 && inventoryIndex > 0)
        {
            UnEquipItem(inventory[inventoryIndex]);
            inventoryIndex--;
            EquipItem(inventory[inventoryIndex]);
        }
        else if (inputValue.Get<float>() < 0 && inventoryIndex < inventory.Count - 1)
        {
            UnEquipItem(inventory[inventoryIndex]);
            inventoryIndex++;
            EquipItem(inventory[inventoryIndex]);
        }
    }

}
