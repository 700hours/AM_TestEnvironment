using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TestEnvironment
{
    public class NPCTargeting : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get { return true; }
        }

        int npcTarget;
        public int damage;
        public float maxDist = 256f;
        Rectangle MouseBox;
        Texture2D texture;
        public IList<int> npcIndex = new List<int>();
        Mod mod = ModLoader.GetMod("TestEnvironment");

        public void Update()
        {
            SetStats(Main.LocalPlayer);
            ChooseTargets(Main.LocalPlayer);
        }

        public void SetStats(Player player)
        {
            if (player.HeldItem.type == mod.ItemType<Items.fire_spell>())
                damage = 32;
        }

        public void ChooseTargets(Player player)
        {
            MouseBox = new Rectangle((int)Main.MouseWorld.X - 32, (int)Main.MouseWorld.Y - 32, 64, 64);
            foreach (NPC n in Main.npc)
            {
                if (n.active && !n.friendly && n.lifeMax >= 15 && !n.dontTakeDamage && !n.immortal)
                {
                    if (MouseDistance(n) < maxDist)
                        npcIndex.Add(n.whoAmI);
                }
            }
            foreach (int n in npcIndex)
            {
                NPC npc = Main.npc[n];
                if (!npc.active || MouseDistance(npc) > maxDist)
                {
                    npcIndex.Remove(n);
                }
            }
        }
        public float MouseDistance(NPC npc)
        {
            return Vector2.Distance(Main.MouseWorld - npc.position, Vector2.Zero);
        }

        public void DrawTargeting(NPC npc, SpriteBatch sb)
        {
            //  Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            //  Main.spriteBatch.End();
        }
        public bool EnableCheck(Player player, int type)
        {
            if (player.whoAmI == Main.myPlayer && player.HeldItem.type == type && Main.netMode < 2)
                return true;
            else return false;
        }

        float degrees;
        const float radians = 0.017f;
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {

        } 

        private ModPacket GetPacket(SpellMessageType type)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)ModMessageType.DmgSpellCast);
            packet.Write((byte)type);
            return packet;
        }
        public void HandlePacket(BinaryReader reader)
        {
            SpellMessageType type = (SpellMessageType)reader.ReadByte();
            if (type == SpellMessageType.TargetIDs)
            {
                int numTargets = reader.ReadInt32();
                npcIndex.Clear();
                for (int k = 0; k < numTargets; k++)
                {
                    npcIndex.Add(reader.ReadInt32());
                }
            }
            else if (type == SpellMessageType.Damage)
            {
                damage = reader.ReadInt32();
                if (Main.netMode == 2)
                {
                    ModPacket netMessage = GetPacket(SpellMessageType.Damage);
                    int ignore = reader.ReadInt32();
                    netMessage.Write(damage);
                    netMessage.Send(-1, ignore);
                }
            }
        }
    }
    enum SpellMessageType : byte
    {
        TargetIDs,
        Damage
    }

    class LocalTargeting : ModPlayer
    {
        bool init;
        float degrees;
        float maxDist = 256f;
        const float radians = 0.017f;
        Texture2D tex, texture;
        Player player = Main.player[Main.myPlayer];
        
        public bool EnableCheck(Player plr, int type)
        {
            if (plr.HeldItem.type == type)
                return true;
            else return false;
        }
        public float NPCDistance(NPC npc)
        {
            if (Main.netMode != 0)
            {
                foreach (Player p in Main.player)
                {
                    if (p.whoAmI != player.whoAmI)
                    {
                        return Vector2.Distance(p.position - npc.position, Vector2.Zero);
                    }
                }
            }
            else return Vector2.Distance(player.position - npc.position, Vector2.Zero);
            return default(float);
        }
        public float MouseDistance(NPC npc)
        {
            return Vector2.Distance(Main.MouseWorld - npc.position, Vector2.Zero);
        }
    }
}
