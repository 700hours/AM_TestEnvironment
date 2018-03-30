using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class m_diggerhead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Digger");
        //  Main.npcCatchable[npc.type] = true;
        }
        public override void SetDefaults()
        {
            npc.width = 30;
            npc.height = 34;
            npc.friendly = false;
            npc.aiStyle = 6;
        //  aiType = NPCID.Worm;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
            npc.damage = 35;
            npc.defense = 6;
            npc.lifeMax = 120;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        int Previous = 0, digger, despawn = 180;
        bool TailSpawned = false;
        public override void AI()
        {
            #region dig AI
            npc.ai[0]++;
            if (npc.ai[0] <= 1 || npc.ai[0] >= 400)
            {
            //  npc.damage = 35;
            //  Main.npc[digger].damage = 35;
                if (!TailSpawned)
                {
                    Previous = npc.whoAmI;
                    for (int num36 = 0; num36 < 7; num36++)
                    {
                        if (num36 >= 0 && num36 < 6)
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
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendData(23, -1, -1, null, digger, 0f, 0f, 0f, 0, 0, 0);
                        }
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
                despawn--;
            if (despawn <= 0)
                npc.active = false;
            #endregion
        }
    }
}
