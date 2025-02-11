using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : Item
{
    [SerializeField]
    float plusTime = 8;

    ItemGenerator ig;

    private void Start()
    {
        ig = transform.parent.GetComponent<ItemGenerator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ig.isActive = true;
        (Managers.UI.SceneUI as UI_GameScene).PlusTime(plusTime);
        gameObject.SetActive(false);
    }
}
