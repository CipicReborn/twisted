using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerDownHandler : MonoBehaviour, UnityEngine.EventSystems.IPointerDownHandler {

    PlayerController m_player;

    private void Awake () {
        m_player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void OnPointerDown (PointerEventData eventData) {
        m_player.Jump();
    }
}
