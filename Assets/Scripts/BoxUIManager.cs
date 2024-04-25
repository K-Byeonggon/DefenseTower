using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BoxUIManager : MonoBehaviour
{
    public int itemCount;
    public List<int> itemNums;
    public int[] itemNumsInBox;
    public GameObject player;
    public bool unboxing;

    private void Start()
    {
        unboxing = false;
    }

    private void Update()
    {
        if (GameManager.Instance.isUnboxing)
        {
            if(!unboxing)
            {
                unboxing = true;
                Get3ItemNums();
                SetItemInfo();
            }
        }
    }

    //모든 아이템 풀에서 같은 확률로 중복없이 3종의 아이템 번호 담음.
    private void Get3ItemNums()
    {
        itemCount = ItemManager.Instance.itemPrefabs.Length;
        itemNums = Enumerable.Range(1, itemCount-1).ToList();
        itemNumsInBox = new int[3];

        for(int i = 0; i < itemNumsInBox.Length; i++)
        {
            int index = Random.Range(0, itemNums.Count);
            itemNumsInBox[i] = itemNums[index];
            itemNums.RemoveAt(index);
            //Debug.Log($"아이템 번호: {itemNumsInBox[i]}");
        }
    }

    private void SetItemInfo()
    {
        for(int i = 0; i < itemNumsInBox.Length; i++)
        {
            Item item = ItemManager.Instance.itemPrefabs[itemNumsInBox[i]].GetComponent<Item>();
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = item.itemName;
            transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = item.itemIcon;
            transform.GetChild(i).GetChild(2).GetComponent<Text>().text = item.descript;
        }
    }

    public void SelectItem(int num)
    {
        //Debug.Log($"num = {num}, itemNum = {itemNumsInBox[num]}");
        GameObject item = ItemManager.Instance.itemPrefabs[itemNumsInBox[num]];
        if(item.tag == "Gun") player.GetComponent<Player>().AddItem(item);
        else if(item.tag == "Item") player.GetComponent<PlayerItem>().AddItem(item);

        GameManager.Instance.isUnboxing = false;
        unboxing = false;
        gameObject.SetActive(false);
    }
}
