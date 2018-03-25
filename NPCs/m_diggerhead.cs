﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestEnvironment.NPCs
{
    public class m_diggerhead : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 32;
            npc.friendly = false;
            npc.aiStyle = 6;
            aiType = NPCID.Worm;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
        }

        int Previous = 0, digger;
        bool TailSpawned = false;
        public override void AI()
        {
            npc.ai[0]++;
            if (npc.ai[0] <= 1 || npc.ai[0] >= 400)
            {
                npc.damage = 30;
                Main.npc[digger].damage = 30;
                if (!TailSpawned)
                {
                    Previous = npc.whoAmI;
                    for (int num36 = 0; num36 < 7; num36++)
                    {
                        if (num36 >= 0 && num36 < 13)
                        {
                            digger = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("m_diggerbody"), npc.whoAmI);
                        }
                        else
                        {
                            digger = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("m_diggertail"), npc.whoAmI);
                        }
                        Main.npc[digger].realLife = npc.whoAmI;
                        Main.npc[digger].ai[2] = (float)npc.whoAmI;
                        Main.npc[digger].ai[1] = (float)Previous;
                        Main.npc[Previous].ai[0] = (float)digger;
                        NetMessage.SendData(23, -1, -1, "", digger, 0f, 0f, 0f, 0);
                        Previous = digger;
                    }
                    TailSpawned = true;
                }
            }
            else if (npc.ai[0] >= 2)
            {
                npc.damage = 30;
            }

            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }
            if (!Main.player[npc.target].active || Main.player[npc.target].dead)
            {
                Main.npc[digger].active = false;
                npc.active = false;
            }
        }
    }
}
