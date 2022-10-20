using Entities;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public Rigidbody rig;
    private SkillBridge.Message.CharacterState state;
    public Character character;
    public float rotateSpeed = 2f;
    public float turnAngle = 10f;
    public int speed;
    public EntityController entityController;
    public bool onAir=false;

    private void Start()
    {
        state = SkillBridge.Message.CharacterState.Idle;
        if (character == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo nChaInfo = new NCharacterInfo();
            nChaInfo.Id = 1;
            nChaInfo.Name = "aa";
            nChaInfo.ConfigId = 1;
            nChaInfo.Entity = new NEntity();
            nChaInfo.Entity.Position = new NVector3();
            nChaInfo.Entity.Direction = new NVector3();
            nChaInfo.Entity.Direction.X = 0;
            nChaInfo.Entity.Direction.Y = 100;
            nChaInfo.Entity.Direction.Z = 0;
            character = new Character(nChaInfo);
            if (entityController != null)
            {
                entityController.entity = character;
            }
        }
    }

    private void FixedUpdate()
    {
        if (character == null) return;
        float v = Input.GetAxis("Vertical");
        if (v > 0.01f)
        {
            if (state != CharacterState.Move)
            {
                state = CharacterState.Move;
                character.MoveForward();
                sendEntityEvent(EntityEvent.MoveFwd);
            }
            rig.velocity = rig.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (character.speed + 9.81f) / 100f;
        }
        else if (v < -0.01f)
        {
            if (state != CharacterState.Move)
            {
                state = CharacterState.Move;
                character.MoveBack();
                sendEntityEvent(EntityEvent.MoveBack);
            }
            rig.velocity = rig.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (character.speed + 9.81f) / 100f;
        }
        else
        {
            if (state != CharacterState.Idle)
            {
                state = CharacterState.Idle;
                rig.velocity = Vector3.zero;
                character.Stop();
                sendEntityEvent(EntityEvent.Idle);
            }
        }
        if (Input.GetButtonDown("Jump"))
        {
            sendEntityEvent(EntityEvent.Jump);
        }
        float h = Input.GetAxis("Horizontal");
        if (h < -0.1 || h > 0.1)
        {
            transform.Rotate(0, h * rotateSpeed, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir,transform.forward);
            if (rot.eulerAngles.y > turnAngle && rot.eulerAngles.y < (360 - turnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(transform.forward));
                rig.transform.forward = transform.forward;
                sendEntityEvent(EntityEvent.None);
            }
        }
    }

    private Vector3 lastPos;
    float lastSync = 0;
    private void LateUpdate()
    {
        Vector3 offset = rig.transform.position - lastPos;
        speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        lastPos = rig.transform.position;
        if ((GameObjectTool.WorldToLogic(rig.transform.position) - character.position).magnitude > 50)
        {
            character.SetPosition(GameObjectTool.WorldToLogic(rig.transform.position));
            sendEntityEvent(EntityEvent.None);
        }
        transform.position = rig.transform.position;
    }

    private void sendEntityEvent(EntityEvent entityEvent)
    {
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent);
        MapService.Instance.SendMapEntitySync(entityEvent, character.EntityData);
    }
}
