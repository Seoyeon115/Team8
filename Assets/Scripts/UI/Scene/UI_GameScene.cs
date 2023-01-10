using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class UI_GameScene : UI_Scene
{
    Vector3 beginDragPos; // BeginDrag position
    public Vector3 joystickDir;
    float joystickRadius;
    PlayerController player;
    Animator _playerAnim;

    enum GameObjects
    {
        JoystickPanel,
        OutLineCircle,
        FiiledCircle,
        ExpBar,
    }

    enum Texts
    {
        TimeText,
        GoldText,
    }

    enum Images
    {
        Heart1,
        Heart2,
        Heart3,
    }

    enum Buttons
    {
        PauseButton,
    }

    void Update()
    {
        UpdateTime();
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        player = Managers.Object.Player;
        _playerAnim = Managers.Object.Player.GetComponent<Animator>();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetObject((int)GameObjects.JoystickPanel).BindEvent(OnPointerDown, Define.UIEvent.PointerDown);
        GetObject((int)GameObjects.JoystickPanel).BindEvent(OnPointerUp, Define.UIEvent.PointerUp);
        GetObject((int)GameObjects.JoystickPanel).BindEvent(OnDrag, Define.UIEvent.Drag);

        GetText((int)Texts.GoldText).text = Managers.Game.SaveData.Gold.ToString();
        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(PopupGameOverUI);

        Managers.Resource.Load<Sprite>("Art/Sprites/UI/Heart_gray");
        Managers.Resource.Load<Sprite>("Art/Sprites/UI/Heart_red");

        joystickRadius = GetObject((int)GameObjects.OutLineCircle).GetComponent<RectTransform>().sizeDelta.y * 1.2f;
        GetObject((int)GameObjects.OutLineCircle).SetActive(false);
        GetObject((int)GameObjects.FiiledCircle).SetActive(false);
        
    }

    void OnPointerDown(PointerEventData evt)
    {
        Managers.Game.Jelly--;
        GetObject((int)GameObjects.OutLineCircle).SetActive(true);
        GetObject((int)GameObjects.FiiledCircle).SetActive(true);
        GetObject((int)GameObjects.OutLineCircle).transform.position = Input.mousePosition;
        GetObject((int)GameObjects.FiiledCircle).transform.position = Input.mousePosition;
        beginDragPos = Input.mousePosition;
    }

    void OnDrag(PointerEventData evt)
    {
        Vector3 endDragPosition = evt.position;
        joystickDir = (endDragPosition - beginDragPos).normalized;

        SetPlayerDir();
        player.MoveVec = joystickDir;

        player.State = Define.State.Walk;

        // Set FilledCircle Boundary
        float stickDistance = Vector3.Distance(endDragPosition, beginDragPos);
        if (stickDistance < joystickRadius)
        {
            GetObject((int)GameObjects.FiiledCircle).transform.position = beginDragPos + joystickDir * stickDistance;
        }
        else
        {
            GetObject((int)GameObjects.FiiledCircle).transform.position = beginDragPos + joystickDir * joystickRadius;
        }
    }

    void OnPointerUp(PointerEventData evt)
    {
        joystickDir = Vector3.zero;
        player.MoveVec = joystickDir;

        // Idle Animation
        player.State = Define.State.Idle;

        GetObject((int)GameObjects.OutLineCircle).SetActive(false);
        GetObject((int)GameObjects.FiiledCircle).SetActive(false);
    }

    void SetPlayerDir()
    {
        float Dot = Vector3.Dot(joystickDir, Vector3.up);
        float Angle = Mathf.Acos(Dot) * Mathf.Rad2Deg;

        if (Angle < 22)
        {
            // up
            _playerAnim.SetFloat("dirX", 0);
            _playerAnim.SetFloat("dirY", 1);
        }
        else if (Angle < 67 && Angle >= 22)
        {
            if (joystickDir.x > 0)
            {
                // up right
                _playerAnim.SetFloat("dirX", 1);
                _playerAnim.SetFloat("dirY", 1);
            }
            else
            {
                // up left
                _playerAnim.SetFloat("dirX", -1);
                _playerAnim.SetFloat("dirY", 1);
            }
        }
        else if (Angle < 112 && Angle >= 67)
        {
            if (joystickDir.x > 0)
            {
                // right
                _playerAnim.SetFloat("dirX", 1);
                _playerAnim.SetFloat("dirY", 0);
            }
            else
            {
                // left
                _playerAnim.SetFloat("dirX", -1);
                _playerAnim.SetFloat("dirY", 0);
            }
        }
        else if(Angle < 157 && Angle >= 122)
        {
            if (joystickDir.x > 0)
            {
                // down right
                _playerAnim.SetFloat("dirX", 1);
                _playerAnim.SetFloat("dirY", -1);
            }
            else
            {
                // down left
                _playerAnim.SetFloat("dirX", -1);
                _playerAnim.SetFloat("dirY", -1);
            }
        }
        else if(Angle > 157)
        {
            // down
            _playerAnim.SetFloat("dirX", 0);
            _playerAnim.SetFloat("dirY", -1);
        }
    }

    int min, sec;
    float limitTime = 121;
    void UpdateTime()
    {
        if (limitTime < 0)
        {
            limitTime = 0;
            (Managers.Scene.CurrentScene as GameScene).GameOver();
        }
        else
        {
            limitTime -= Time.deltaTime;
            min = (int)limitTime / 60;
            sec = (int)limitTime % 60;
            string result = sec.ToString("D2");
            GetText((int)Texts.TimeText).text = $"{min}:{result}";
        }
    }

    public void PlusTime(float time)
    {
        limitTime += time;
    }

    public void SetHeartUI(int hp)
    {
        switch (hp)
        {
            case 3:
                GetImage((int)Images.Heart1).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_red");
                GetImage((int)Images.Heart2).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_red");
                GetImage((int)Images.Heart3).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_red");
                break;
            case 2:
                GetImage((int)Images.Heart1).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_red");
                GetImage((int)Images.Heart2).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_red");
                GetImage((int)Images.Heart3).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_gray");
                break;
            case 1:
                GetImage((int)Images.Heart1).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_red");
                GetImage((int)Images.Heart2).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_gray");
                GetImage((int)Images.Heart3).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_gray");
                break;
            case 0:
                GetImage((int)Images.Heart1).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_gray");
                GetImage((int)Images.Heart2).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_gray");
                GetImage((int)Images.Heart3).sprite = Managers.Resource.Load<Sprite>("Sprites/UI/Heart_gray");
                (Managers.Scene.CurrentScene as GameScene).GameOver();
                break;
        }
    }

    public void SetExpBar(int curExp, int maxExp)
    {
        GetObject((int)GameObjects.ExpBar).GetComponent<Slider>().value = (float)curExp/maxExp;
    }

    public void UpdateGoldText()
    {
        GetText((int)Texts.GoldText).text = Managers.Game.SaveData.Gold.ToString();
    }

    void PopupGameOverUI(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<UI_GameOver>();
    }
}
