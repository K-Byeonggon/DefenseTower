using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public string descript;
    [SerializeField] protected int deployIndex;
    public int DeployIndex { get { return deployIndex; } set { deployIndex = value; } }
}
