using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    PlayerController _player;
    public PlayerController Player { get { return _player; } set { _player = value; } }

    CameraController _camera;
    public CameraController Camera
    {
        get
        {
            if (_camera == null)
                _camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

            return _camera;
        }
    }

    Grid _catHouse;
    public Grid CatHouse { get { return _catHouse; } set { _catHouse = value; } }

    public ObjectManager()
    {
        Init();
    }

    public void Init()
    {
        
    }

    public void SpawnPlayer(string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);
        _player = go.GetOrAddComponent<PlayerController>();
    }

    public void SpawnCatHouse(string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);
        _catHouse = go.GetOrAddComponent<Grid>();
    }
}
