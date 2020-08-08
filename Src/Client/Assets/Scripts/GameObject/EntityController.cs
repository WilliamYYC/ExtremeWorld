using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Managers;
using System;

public class EntityController:MonoBehaviour,IEntityNotify
{
    public Animator anim;
    public Rigidbody rb;
    private AnimatorStateInfo currentBaseState;

    public Entity entity;

    public UnityEngine.Vector3 position;
    public UnityEngine.Vector3 direction;
    Quaternion rotation;

    //上一次的位置信息
    public UnityEngine.Vector3 lastPosition;
    Quaternion lastRotation;

    public float speed;
    public float animSpeed = 1.5f;
    public float jumpPower = 3.0f;

    public bool isPlayer = false;

    //坐骑相关
    public RideController rideController;
    private int currentRide;

    public Transform rideBone;


    void Start()
    {
        if (entity != null)
        {
            //注册位置同步事件
            EntityManager.Instance.RegisterEntityChangeNotify(entity.entityId,this);
            this.UpdateTransform();
            
        }

        if (!this.isPlayer)
            rb.useGravity = false;
    }
    void UpdateTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
        this.direction = GameObjectTool.LogicToWorld(entity.direction);

        this.rb.MovePosition(this.position);
        this.transform.forward = this.direction;
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;
    }


    void OnDestroy()
    {
        if (entity != null)
            Debug.LogFormat("{0} OnDestroy :ID:{1} POS:{2} DIR:{3} SPD:{4} ", this.name, entity.entityId, entity.position, entity.direction, entity.speed);

        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
    }

    void FixedUpdate()
    {
        if (this.entity == null)
            return;

        this.entity.OnUpdate(Time.fixedDeltaTime);

        if (!this.isPlayer)
        {
            this.UpdateTransform();
        }
    }

    public void OnEntityRemoved()
    {
        //UIWorldElementManager全局mono元素 删除放到实体被移除这里来
        if (UIWorldElementManager.Instance !=null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
        Destroy(this.gameObject);
    }

    //角色改变打印日志，（开启会增加大量日志信息,如果要调试其他功能可以先注释掉）
    public void OnEntityChanged(Entity entity)
    {
        Debug.LogFormat("OnEntityChanged ID {0}  pos {1}  dir {2} speed{3} ", entity.entityId, entity.position, entity.direction, entity.speed);
    }


    //实体事件处理函数
    public void OnEntityEvent(EntityEvent entityEvent, int param)
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
        //如果有坐骑，调用坐骑事件处理函数
        if (this.rideController!=null)
        {
            this.rideController.OnEntityEvent(entityEvent, param);
        }
    }

    public void Ride(int rideId)
    {
        if (currentRide == rideId)
        {
            return;
        }
        currentRide = rideId;
        //判断是否上坐骑
        if (rideId > 0)
        {
            this.rideController = GameObjectManager.Instance.LoadRide(rideId, this.transform);
        }
        else
        {
            Destroy(this.rideController.gameObject);
            this.rideController = null;
        }

        //上坐骑和下坐骑的切换
        if (this.rideController == null)
        {
            this.anim.transform.localPosition = Vector3.zero;
            this.anim.SetLayerWeight(1,0);
        }
        else
        {
            this.rideController.SetRide(this);
            this.anim.SetLayerWeight(1, 1);
        }
    }

    //设置任务角色和坐骑的接触点
    public void SetRidePosition(Vector3 position)
    {
        this.anim.transform.position = position + (this.anim.transform.position - this.rideBone.position);
    }

}
