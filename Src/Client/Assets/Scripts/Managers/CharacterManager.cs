using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;
using Entities;

namespace Managers
{
    class CharacterManager:Singleton<CharacterManager>
    {
        public Dictionary<int, Character> characters = new Dictionary<int, Character>();
        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;
        public CharacterManager()
        { }

        public void Dispose()
        {

        }
        public void Init()
        {

        }


        public void clear()
        {
            int[] keys = this.characters.Keys.ToArray();
            foreach (var key in keys)
            {
                this.RemoveCharacter(key);
            }
            this.characters.Clear();
        }

        public void AddCharacter(SkillBridge.Message.NCharacterInfo cha)
        {
            Debug.LogFormat("AddCharacter:{0}:{1} Map:{2} Entity:{3}", cha.Id, cha.Name, cha.mapId, cha.Entity.String());
            Character character = new Character( cha);
            this.characters[cha.Id] = character;
            EntityManager.Instance.AddEntity(character);
            if (this.OnCharacterEnter != null)
            {
                this.OnCharacterEnter(character);
            }
        }

        public void RemoveCharacter(int characterId)
        {
            Debug.LogFormat("RemoveCharacter:{0}", characterId);
            if (this.characters.ContainsKey(characterId))
            {
                EntityManager.Instance.RemoveEntity(this.characters[characterId].Info.Entity);
                if (this.OnCharacterLeave != null)
                {
                    this.OnCharacterLeave(this.characters[characterId]);
                }
            }
            this.characters.Remove(characterId);
        }
    }
}
