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
    public class PlayerTargeting : ModPlayer
    {
        int npcTarget;
        public int healAmount;
        public float maxDist = 256f;
        Rectangle MouseBox;
        Texture2D texture;
        public IList<int> playerIndex = new List<int>();
        Player player = Main.player[Main.myPlayer];
        Mod mod = ModLoader.GetMod("TestEnvironment");

        public void Update()
        {
            SetStats(Main.LocalPlayer);
            ChooseTargets(Main.LocalPlayer, player);
        }

        public void SetStats(Player plr)
        {
            if (plr.HeldItem.type == mod.ItemType<Items.heal_spell>())
                healAmount = 30;
        }

        public void ChooseTargets(Player localPlayer, Player myPlayer)
        {
            foreach (Player p in Main.player)
            {
                if (p.active && !p.dead && p.whoAmI != player.whoAmI)
                {
                    if (EnableCheck(player, mod.ItemType<Items.heal_spell>()) && PlayerDistance(p, player) < maxDist)
                    {
                        playerIndex.Add(p.whoAmI);
                    }
                    else playerIndex.Remove(p.whoAmI);
                }
            }
        }
        public float MouseDistance(Player plr)
        {
            return Vector2.Distance(Main.MouseWorld - plr.position, Vector2.Zero);
        }
        public float PlayerDistance(Player localPlayer, Player myPlayer)
        {
            return Vector2.Distance(myPlayer.position - localPlayer.position, Vector2.Zero);
        }

        public void DrawTargeting(NPC npc, SpriteBatch sb)
        {
            //  Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            //  Main.spriteBatch.End();
        }
        public bool EnableCheck(Player plr, int type)
        {
            if (plr.HeldItem.type == type)
                return true;
            else return false;
        }

        float degrees;
        const float radians = 0.017f;
        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {

        }

        private ModPacket GetPacket(PlayerMessageType type)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)ModMessageType.HealSpellCast);
            packet.Write((byte)type);
            return packet;
        }
        public void HandlePacket(BinaryReader reader)
        {
            PlayerMessageType type = (PlayerMessageType)reader.ReadByte();
            if (type == PlayerMessageType.TargetIDs)
            {
                int numTargets = reader.ReadInt32();
                playerIndex.Clear();
                for (int k = 0; k < numTargets; k++)
                {
                    playerIndex.Add(reader.ReadInt32());
                }
            }
            else if (type == PlayerMessageType.Healing)
            {
                healAmount = reader.ReadInt32();
                if (Main.netMode == 2)
                {
                    ModPacket netMessage = GetPacket(PlayerMessageType.Healing);
                    int ignore = reader.ReadInt32();
                    netMessage.Write(healAmount);
                    netMessage.Send(-1, ignore);
                }
            }
        }
    }
    enum PlayerMessageType : byte
    {
        TargetIDs,
        Healing
    }
}
