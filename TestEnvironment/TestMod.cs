using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TestEnvironment;

namespace TestEnvironment
{
    public class TestMod : Mod
    {
        public const string magnoHead = "TestEnvironment/Gores/magno_icon";
        private Mod mod = ModLoader.GetMod("TestEnvironment");
        public void SetModInfo(out string name, ref ModProperties properties)
        {
            name = "The TestEnvironment Mod";
            properties.Autoload = true;
            properties.AutoloadGores = true;
            properties.AutoloadSounds = true;
        }
        public override void Load()
        {
            AddBossHeadTexture(magnoHead);
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.netMode < 2 && Main.LocalPlayer.active && !Main.gameMenu && Main.LocalPlayer.GetModPlayer<TestPlayer>().MagnoZone)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Magno_Biome_1");
                priority = MusicPriority.BiomeHigh;
            }
        }

        //  Need to sort out HandlePacket for NPCTargeting spell targets
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModMessageType msgType = (ModMessageType)reader.ReadByte();
            switch(msgType)
            {   
                case ModMessageType.DmgSpellCast:
                    NPCTargeting DmgSpell = new NPCTargeting() as NPCTargeting;
                    DmgSpell.HandlePacket(reader);
                    break;
                case ModMessageType.HealSpellCast:
                    PlayerTargeting HealSpell = new PlayerTargeting() as PlayerTargeting;
                    HealSpell.HandlePacket(reader);
                    break;
                default:
                    ErrorLogger.Log("Archaea Mod: Unknown Message Type" + msgType);
                    break;
            }
        }
    }
    enum ModMessageType : byte
    {
        DmgSpellCast,
        HealSpellCast
    }
    public class TestMain
    {
        public static IList<SpellEntity> entity = new SpellEntity[1001];
    }
}