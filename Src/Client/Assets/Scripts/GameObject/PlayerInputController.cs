using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Entities;
using SkillBridge.Message;
using Managers;
using Services;
using System;

public class PlayerInputController:MonoBehaviour
{


    public Rigidbody rb;
    SkillBridge.Message.CharacterState state;

    public Character character;

    public float rotateSpeed = 2.0f;

    public float turnAngle = 10;

    public int speed;

    public EntityController entityController;

    public bool onAir = false;

    private NavMeshAgent agent;
    private bool autoNav = false;
    // Use this for initialization
    void Start()
    {
        state = SkillBridge.Message.CharacterState.Idle;
        if (this.character == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo info = new NCharacterInfo();
            info.Id = 1;
            info.Name = "test";
            info.ConfigId = 1;
            info.Entity = new NEntity();
            info.Entity.Position = new NVector3();
            info.Entity.Direction = new NVector3();
            info.Entity.Direction.X = 0;
            info.Entity.Direction.Y = 100;
            info.Entity.Direction.Z = 0;

            this.character = new Character(info);

            if (entityController !=null)
            {
                entityController.entity = this.character;
            }
        }
        if (agent == null)
        {
            agent = this.gameObject.AddComponent<NavMeshAgent>();
            agent.stoppingDistance = 0.3f;
        }
    }
    //寻路移动
    public void NavMove()
    {
        //寻路还在进行中直接返回
        if (agent.pathPending)
        {
            return;
        }
        //寻路的路径无效关闭寻路
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            stopNav();
            return;
        }
        //寻路还在进行中直接返回
        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            return;
        }
        //玩家方向键有输入关闭自动寻路
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1)
        {
            stopNav();
            return;
        }

        NavPathRenderer.Instance.SetPath(agent.path, agent.destination);
        //代理关闭或者剩下的距离小于0.3关闭自动寻路
        if (agent.isStopped || agent.remainingDistance < 2f)
        {
            stopNav();
            return;
        }
    }


    void FixedUpdate()
    {
        if (this.character == null)
        {
            return;
        }

        if (autoNav)
        {
            this.NavMove();
            return;
        }
        if (InputManager.Instance !=null && InputManager.Instance.isInputMode)
        {
            return;
        }
        float v = Input.GetAxis("Vertical");
        if (v > 0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else if (v < -0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveBack();
                this.SendEntityEvent(EntityEvent.MoveBack);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else
        {
            if (state != SkillBridge.Message.CharacterState.Idle)
            {
                state = SkillBridge.Message.CharacterState.Idle;
                this.rb.velocity = Vector3.zero;
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            this.SendEntityEvent(EntityEvent.Jump);
        }

        float h = Input.GetAxis("Horizontal");
        if (h < -0.1 || h > 0.1)
        {
            this.transform.Rotate(0, h * rotateSpeed, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir, this.transform.forward);

            if (rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            }

        }
    }

   

    Vector3 lastPos;
    float lastSync = 0;
    private void LateUpdate()
    {
		if (this.character == null) return;
        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Debug.LogFormat("LateUpdate velocity {0} : {1}", this.rb.velocity.magnitude, this.speed);
        this.lastPos = this.rb.transform.position;

        Vector3Int goLogicPos = GameObjectTool.WorldToLogic(this.rb.transform.position);
        float logicOffset = (goLogicPos - this.character.position).magnitude;
        if (logicOffset > 100)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;

        //方向发生变化同步
        Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
        Quaternion rot = new Quaternion();

        rot.SetFromToRotation(dir, this.transform.forward);

        if (rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360-this.turnAngle))
        {
            this.character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
            this.SendEntityEvent(EntityEvent.None);
        }
    }

    public void SendEntityEvent(EntityEvent entityEvent, int param = 0)
    {
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent,param);

        MapService.Instance.SendMapEntitySync(entityEvent, this.character.EntityData, param);
    }

    //寻路
    public void startNav(Vector3 target)
    {
        StartCoroutine(beginNav(target));
    }

    //开始自动寻路
    IEnumerator beginNav(Vector3 target)
    {
        agent.SetDestination(target);
        yield return null;
        autoNav = true;
        if (state != CharacterState.Move)
        {
            state = CharacterState.Move;
            this.character.MoveForward();
            this.SendEntityEvent(EntityEvent.MoveFwd);
            agent.speed = this.character.speed / 100f;
        }
    }

    //停止自动寻路
    public void stopNav()
    {
        autoNav = false;
        agent.ResetPath();
        if (state != CharacterState.Idle)
        {
            state = CharacterState.Idle;
            this.rb.velocity = Vector3.zero;
            this.character.Stop();
            this.SendEntityEvent(EntityEvent.Idle);
        }
        NavPathRenderer.Instance.SetPath(null , Vector3.zero);
    }
}

