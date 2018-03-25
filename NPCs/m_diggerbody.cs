using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestEnvironment.NPCs
{
    public class m_diggerbody : ModNPC
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
        public override void AI()
        {
            if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].life <= 0)
            {
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.active = false;
            }
        }
    }
}
