using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class boss_magnobody : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 74;
            npc.height = 62;
            npc.friendly = false;
            npc.aiStyle = 6;
          //aiType = NPCID.Worm;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
            npc.damage = 18;
            npc.defense = 4;
            npc.lifeMax = 120;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }
        public override void AI()
        {
            if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].life <= 0)
            {
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.active = false;
            }
            /*  //  optional pup spawning
            npc.ai[0]++;
            Vector2 npcCenter = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);
            if (npc.ai[0] % 450 == 0)
            {
                int pups = NPC.NewNPC((int)npcCenter.X, (int)npcCenter.Y, mod.NPCType("m_puphead"));
                if (Main.netMode != 0)
                    NetMessage.SendData(23, -1, -1, null, pups, 0f, 0f, 0f, 0, 0, 0);
            }   */
        }
    }
}
