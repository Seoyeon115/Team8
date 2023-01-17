using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        Num_Text,
    }

    string _name;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.ItemIcon);
        Get<GameObject>((int)GameObjects.Num_Text).GetComponent<TextMeshProUGUI>().text = "999";// �������� ��
        Get<GameObject>((int)GameObjects.ItemIcon);// �̺�Ʈ�߰�

    }

    private void Update()
    {

    }
    public void SetInfo(string name)
    {
        _name = name;
    }
    
}
