using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;

namespace GameServer.Managers
{
    class CharacterManager:Singleton<CharacterManager>
    {
        public Dictionary<int, Character> characters = new Dictionary<int, Character>();

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
            this.characters.Clear();
        }

        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player, cha);
            EntityManager.Instance.AddEntity(cha.MapID,character);
            character.Info.EntityId = character.entityId;
            this.characters[character.Id] = character;
            return character;
        }

        public void RemoveCharacter(int characterId)
        { 
            var cha = this.characters[characterId];
            EntityManager.Instance.RemoveEntity(cha.Data.MapID, cha);
            this.characters.Remove(characterId);
        }

        public Character GetCharacter(int characterId)
        {
            Character character = null;
            this.characters.TryGetValue(characterId, out character);
            return character;
        }
    }
}
