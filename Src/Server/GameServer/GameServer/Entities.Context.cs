﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameServer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ExtremeWorldEntities : DbContext
    {
        public ExtremeWorldEntities()
            : base("name=ExtremeWorldEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TUser> Users { get; set; }
        public virtual DbSet<TPlayer> Players { get; set; }
        public virtual DbSet<TCharacter> Characters { get; set; }
        public virtual DbSet<TCharacterItem> CharacterItem { get; set; }
        public virtual DbSet<TCharacterBag> CharacterBags { get; set; }
        public virtual DbSet<TCharacterQuests> CharacterQuests { get; set; }
        public virtual DbSet<TCharacterFriend> CharacterFriends { get; set; }
        public virtual DbSet<TGuild> Guilds { get; set; }
        public virtual DbSet<TGuildMember> GuildMembers { get; set; }
        public virtual DbSet<TGuildApply> GuildApplies { get; set; }
    }
}
