using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite buttonImage;
    public Sprite onButtonImage;
    // 마우스가 버튼 위에 올라갔을 때 호출됩니다.
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Button>().image.sprite = onButtonImage;
    }

    // 마우스가 버튼에서 벗어났을 때 호출됩니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Button>().image.sprite = buttonImage;
    }

}
