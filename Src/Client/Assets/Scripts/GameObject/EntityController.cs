using Entities;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour,IEntityNotify
{
    public Animator anim;
    public Rigidbody rig;
    private AnimatorStateInfo currentBaseState;

    public Entity entity;
    public Vector3 position;
    public Vector3 direction;
    private Quaternion rotation;

    public Vector3 lastPosition;
    private Quaternion lastRotation;

    public float speed;
    public float animSpeed = 1.0f;
    public float jumpPower = 3.0f;
    public bool isPlayer = false;

    public RideController rideController;
    private int currentRide = 0;
    public Transform rideBone;

    private void Start()
    {
        if (entity != null)
        {
            EntityManager.Instance.RegisterEntityChangeNotify(entity.entityId, this);
            UpdateTransform();
        }
        if (!isPlayer)
        {
            rig.useGravity = false;
        }
    }

    private void UpdateTransform()
    {
        position = GameObjectTool.LogicToWorld(entity.position);
        direction = GameObjectTool.LogicToWorld(entity.direction);
        rig.MovePosition(position);
        transform.forward = direction;
        lastPosition = position;
        lastRotation = rotation;
    }

    private void OnDestroy()
    {
        if (entity != null)
        {
            Debug.LogFormat("Name:{0} OnDestroy :entityId:{1} Position:{2} Direction:{3} Speed:{4} ", this.name, entity.entityId, entity.position, entity.direction, entity.speed);
        }
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(transform);
        }
    }

    private void FixedUpdate()
    {
        if (entity == null) return;
        entity.OnUpdate(Time.fixedDeltaTime);
        if (!isPlayer)
        {
            UpdateTransform();//不是玩家就用逻辑驱动位置方向
        }
    }

    public void OnEntityEvent(EntityEvent entityEvent,int param) 
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
            case EntityEvent.Ride:
                this.Ride(param);
                break;
        }
        if (this.rideController != null) this.rideController.OnEntityEvent(entityEvent, param);
    }

    public void Ride(int rideId) 
    {
        if (currentRide == rideId) return;
        currentRide = rideId;
        if (rideId > 0)
        {
            rideController = GameObjectManager.Instance.LoadRide(rideId, transform);
        }
        else 
        {
            Destroy(rideController.gameObject);
            rideController = null;
        }
        if (rideController == null)
        {
            anim.transform.localPosition = Vector3.zero;
            anim.SetLayerWeight(1, 0);
        }
        else 
        {
            rideController.SetRider(this);
            anim.SetLayerWeight(1, 1);
        }
    }

    public void SetRidePosition(Vector3 position) //position是坐骑的骨点
    {
        anim.transform.position = position + (anim.transform.position - rideBone.position);
    }

    public void OnEntityRemoved()
    {
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(transform);
        }
        Destroy(gameObject);
    }

    public void OnEntityChanged(Entity entity)
    {
        Debug.LogFormat("MapEntityUpdateRequest: ID:{0}, Pos:{1}, DIR:{2}, SPEED:{3}", entity.entityId, entity.position, entity.direction, entity.speed);
    }
}
