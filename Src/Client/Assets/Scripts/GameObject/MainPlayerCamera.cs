using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>
{
    public Camera camera;
    public Transform viewPoint;
    public GameObject player;

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            player = User.Instance.currentCharacterObject;
        }
        if (player == null)
        {
            return;
        }
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
    }
}
