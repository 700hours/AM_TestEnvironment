using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestEnvironment.NPCs
{
    public class m_flame : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 48;
            npc.friendly = false;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.damage = 15;
            npc.defense = 0;
            npc.lifeMax = 10;
        //  npc.HitSound = SoundID.NPCHit1;
        //  npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        bool init = false;
        public void Initialize()
        {
            degrees = npc.ai[1];
        }
        float radius = 180;
        float degrees = 0.017f;
        Vector2 center;
        const float radians = 0.017f;
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }
            npc.TargetClosest(true);

            Player player = Main.player[npc.target];

            degrees += radians * 3.2f;
            radius -= 0.5f;

            center = player.position;
            npc.position.X = center.X + (float)(radius * Math.Cos(degrees));
            npc.position.Y = center.Y + (float)(radius * Math.Sin(degrees));

            if (radius < 1f)
                npc.active = false;
        }
    }
}
