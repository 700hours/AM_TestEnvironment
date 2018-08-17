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
    public class TestPlayer : ModPlayer
    {
        public bool magnoMinion;
        public bool magnoShield;
        public bool magnoRanged;
        public bool MagnoZone;
        public bool magnoInvuln;
        public bool bossMode;
        private bool init;
        public bool targeted;
        bool flag;
        public int[] MPID = new int[255];
        int num;
        int ticks = 0;
        int debuffTime = 300;
        const int hellLayer = 192;
        const int tileSize = 16;
        private Texture2D texture;
        private Function Func;
        public override void Initialize()
        {
            Func = new Function();
        }
        public override void PostUpdate()
        {
            foreach (Player plr in Main.player)
            {
                if (PlrDist(plr) && plr.active)
                {
                    if (player.HeldItem.type == mod.ItemType<Items.heal_spell>())
                    {
                        SpellEntity.NewEntity(plr.Center, player, player.eyeColor, 0, 384f, 8f, 1f, 1f, mod.ItemType<Items.heal_spell>(), player.whoAmI, plr.whoAmI);
                    }
                }
            }
            foreach (NPC npc in Main.npc)
            {
                if (PlrNPCDist(player, npc) && npc.active && !npc.friendly && npc.lifeMax >= 15 && !npc.immortal && !npc.dontTakeDamage)
                {
                    if (player.HeldItem.type == mod.ItemType<Items.fire_spell>())
                    {
                        SpellEntity.NewEntity(npc.Center, player, Color.White, 0, 256f, 8f, 1f, 1f, mod.ItemType<Items.fire_spell>(), player.whoAmI, npc.whoAmI);
                    }
                }
            }
            
            TestWorld modWorld = mod.GetModWorld<TestWorld>();

            #region item checks
            for (int k = 0; k < player.armor.Length; k++)
            {
                if (player.armor[k].type == mod.ItemType<Items.magno_shieldacc>())
                {
                    magnoShield = true;
                    break;
                }
                else magnoShield = false;
            }
            for (int k = 0; k < player.armor.Length; k++)
            {
                if (player.armor[k].type == mod.ItemType<Items.Armors.magnoheadgear>())
                {
                    magnoRanged = true;
                    break;
                }
                else
                {
                    magnoRanged = false;
                }

            }
            #endregion

            if (MagnoZone && !modWorld.MagnoDefeated)
            {
                if(!player.HasBuff(mod.BuffType<Buffs.magno_cursed>()))
                {
                    player.AddBuff(BuffID.WaterCandle, debuffTime, true);
                }
            }

            if (!modWorld.MagnoDefeated)
            {
                if (player.position.Y > (Main.maxTilesY * tileSize) - hellLayer * tileSize) 
                {
                    player.AddBuff(BuffID.OnFire, debuffTime, false);
                    if (!flag)
                    {
                        Main.NewText("Energy from the red crystals gathers here", 210, 110, 110);
                        flag = true;
                    }
                }
            }

            if (Main.netMode == 0)
            {
            //  player.statLife = player.statLifeMax;
                if (Main.mouseMiddle)
                {   
                //  Main.dayTime = true;
                //  Main.time = 12400;
                    player.statMana = player.statManaMax;
                    //  Main.NewText("Location: i, " + Math.Round(player.position.X / 16, 0) + " j, " + Math.Round(player.position.Y / 16, 0), Color.White);
                    //  Main.dayTime = false;
                    //  Main.time = 12400;
                }  
            }
            if (ticks == 0)
            {
                Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.GravitationPotion, 10);
                ticks = 1;
            }
        }

        private bool PlrDist(Player target)
        {
            return Vector2.Distance(player.position - target.position, Vector2.Zero) < 384f;
        }
        private bool PlrNPCDist(Player player, NPC npc)
        {
            return Vector2.Distance(player.position - npc.position, Vector2.Zero) < 384f;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)player.whoAmI);
            packet.Send(toWho, fromWho);
        }

        public override void UpdateBiomes()
        {
            MagnoZone = (TestWorld.magnoTiles >= 100);
        }
        public override bool CustomBiomesMatch(Player other)
        {
            TestPlayer modOther = other.GetModPlayer<TestPlayer>(mod);
            return MagnoZone == modOther.MagnoZone;
        }
        public override void CopyCustomBiomesTo(Player other)
        {
            TestPlayer modOther = other.GetModPlayer<TestPlayer>(mod);
            modOther.MagnoZone = MagnoZone;
        }
        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = MagnoZone;
            writer.Write(flags);
        }
        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            MagnoZone = flags[0];
        }
        public override Texture2D GetMapBackgroundImage()
        {
            if (MagnoZone)
            {
                return mod.GetTexture("Backgrounds/MapBGMagno");
            }
            return null;
        }

        int monolith;
        private NPC boss
        {
            get { return Main.npc[monolith]; }
        }
        public void SpaceArena()
        {
            foreach(NPC n in Main.npc)
            {
                if(n.boss && n.active && n.type == mod.NPCType<NPCs.npc_one>())
                {
                    monolith = n.whoAmI;
                }
            }

            int i = (int)player.position.X / 16;
            int j = (int)player.position.Y / 16;
            if (boss.active && boss.type == mod.NPCType<NPCs.npc_one>())
            {
                Tile tile = Main.tile[i, j];
                if(tile.type == mod.TileType<Tiles.boss_roomlight>())
                {
                    WarpedGravity();
                }
                bossMode = true;
            }
            else bossMode = false;
        }
        public void WarpedGravity()
        {
            if (boss.ai[0] != 0)
            {
                if (player.controlUp)
                    player.velocity.Y -= 1.5f;
                if (player.controlLeft)
                    player.velocity.X -= 0.4f;
                if (player.controlDown)
                    player.velocity.Y += 0.3f;
                if (player.controlRight)
                    player.velocity.X += 0.4f;

                float clamp = 4f;
                if (player.velocity.Y > clamp)
                    player.velocity.Y = clamp;
                if (player.velocity.Y < clamp * -1)
                    player.velocity.Y = clamp * -1;
                if (player.velocity.X > clamp)
                    player.velocity.X = clamp;
                if (player.velocity.X < clamp * -1)
                    player.velocity.X = clamp * -1;

                if (!player.controlUp && !player.controlLeft && !player.controlDown && !player.controlRight)
                    player.velocity = Vector2.Zero;

                player.slowFall = true;
                player.noFallDmg = true;
            }
        }
    }
}