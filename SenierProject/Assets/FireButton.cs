using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public projectileActor player;

    public void OnPointerDown(PointerEventData eventData)
    {
        player.PointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.PointerUp();
    }

    private void OnDisable()
    {
        player.PointerUp();
    }
}
