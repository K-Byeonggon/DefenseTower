using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite buttonImage;
    public Sprite onButtonImage;
    // ���콺�� ��ư ���� �ö��� �� ȣ��˴ϴ�.
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Button>().image.sprite = onButtonImage;
    }

    // ���콺�� ��ư���� ����� �� ȣ��˴ϴ�.
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Button>().image.sprite = buttonImage;
    }

}
