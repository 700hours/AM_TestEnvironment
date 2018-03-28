using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class m_puphead : ModNPC
    {
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
            npc.damage = 30;
            npc.defense = 10;
            npc.lifeMax = 45;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        int Previous = 0, pup, despawn = 180;
        bool TailSpawned = false;
        public override void AI()
        {
            #region dig AI
            npc.ai[0]++;
            if (npc.ai[0] <= 1 || npc.ai[0] >= 400)
            {
                if (!TailSpawned)
                {
                    Previous = npc.whoAmI;
                    for (int num36 = 0; num36 < 5; num36++)
                    {
                        if (num36 >= 0 && num36 < 4)
                        {
                            pup = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("m_pupbody"), npc.whoAmI);
                        }
                        else
                        {
                            pup = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("m_puptail"), npc.whoAmI);
                        }
                        Main.npc[pup].realLife = npc.whoAmI;
                        Main.npc[pup].ai[2] = (float)npc.whoAmI;
                        Main.npc[pup].ai[1] = (float)Previous;
                        Main.npc[Previous].ai[0] = (float)pup;
                        NetMessage.SendData(23, -1, -1, null, pup, 0f, 0f, 0f, 0, 0, 0);
                        Previous = pup;
                    }
                    TailSpawned = true;
                }
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
