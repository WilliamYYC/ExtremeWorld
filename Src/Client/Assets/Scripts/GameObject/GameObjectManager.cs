﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Services;
using SkillBridge.Message;
using Models;
using Managers;
using System;
using System.Xml.Serialization;

public class GameObjectManager :MonoSingleton<GameObjectManager>
{

    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
    // Use this for initialization
    protected override  void  OnStart()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
    }

    

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }



    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    void OnCharacterLeave(Character cha)
    {
        if ( !this.Characters.ContainsKey(cha.entityId))
        {
            return;
        }

        if (this.Characters[cha.entityId] != null)
        {
            Destroy(this.Characters[cha.entityId]);
            this.Characters.Remove(cha.entityId);
        }
    }
    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            UnityEngine.Object obj = Resloader.Load<UnityEngine.Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.",character.Define.TID, character.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj, this.transform);
            go.name = "Character_" + character.Id + "_" + character.Name;
            Characters[character.entityId] = go;
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
        this.InitGameObject(Characters[character.entityId], character);
    }

    private void InitGameObject(GameObject go ,Character character)
    {
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
        

        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsCurrentPlayer;
        }

        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            if (character.IsCurrentPlayer)
            {
                User.Instance.CurrentCharacterObject = go;
                MainPlayerCamera.Instance.player = go;
                pc.enabled = true;
                pc.character = character;
                pc.entityController = ec;
            }
            else
            {
                pc.enabled = false;
            }
        }
       
    }
}

