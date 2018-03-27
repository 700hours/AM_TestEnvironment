using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestEnvironment.NPCs
{
    public class boss_magnohead : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 64;
            npc.height = 128;
            npc.friendly = false;
            npc.aiStyle = 6;
        //  aiType = NPCID.Worm;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
            npc.damage = 35;
            npc.defense = 6;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        bool TailSpawned = false;
        bool flames = false;
        bool pupsSpawned = false;
        bool magnoClone = false;
        int Previous = 255, digger = 0, despawn = 210;
        int pups = 255, clone = 255, timeLeft = 600;
        int flamesID = 255;
        int ticks = 0;
        float TargetAngle, PlayerAngle;
        float degrees = 0, radius = 64;
        float Depreciate = 80, Point;
        const float Time = 80;
        const float radians = 0.017f;
        Rectangle target;
        Vector2 oldPosition, newPosition;
        Vector2 npcCenter, playerCenter, center;
        Vector2 Start, End;
        public override void AI()
        {
            #region spawn parts
            npc.ai[0]++;
            if (npc.ai[0] <= 1 || npc.ai[0] >= 400)
            {
                //  npc.damage = 35;
                //  Main.npc[digger].damage = 35;
                if (!TailSpawned)
                {
                    Previous = npc.whoAmI;
                    for (int num36 = 0; num36 < 10; num36++)
                    {
                        if (num36 >= 0 && num36 < 9)
                        {
                            digger = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("boss_magnobody"), npc.whoAmI);
                        }
                        else
                        {
                            digger = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("boss_magnotail"), npc.whoAmI);
                        }
                        Main.npc[digger].color = npc.color;
                        Main.npc[digger].realLife = npc.whoAmI;
                        Main.npc[digger].ai[2] = (float)npc.whoAmI;
                        Main.npc[digger].ai[1] = (float)Previous;
                        Main.npc[Previous].ai[0] = (float)digger;
                        NetMessage.SendData(23, -1, -1, null, digger, 0f, 0f, 0f, 0, 0, 0);
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

            Player player = Main.player[npc.target];

            if (!flames && Main.rand.Next(0, 6000) == 0)
            {
                for (int k = 1; k < 5; k++)
                {
                    degrees = 90f;
                    radius = 128f;
                    center = player.position;
                    float nX = center.X + (float)(radius * Math.Cos(degrees * k));
                    float nY = center.Y + (float)(radius * Math.Sin(degrees * k));

                    flamesID = NPC.NewNPC((int)nX, (int)nY, mod.NPCType("m_flame"));
                    NetMessage.SendData(23, -1, -1, null, flamesID, 0f, 0f, 0f, 0, 0, 0);

                    Main.npc[flamesID].ai[1] = degrees * k;
                    flames = true;
                }
            }
            if (flames)
            {
                radius -= 0.5f;
                NPC n = Main.npc[flamesID];
                if (n.active = false || radius <= 1f)
                    flames = false;
            }

            #region magno clone sequence
            ticks++;
            npcCenter = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);
            if (!pupsSpawned && ticks % 1080 == 0)
            {
                for (int k = 1; k < 4; k++)
                {
                    pups = NPC.NewNPC((int)npcCenter.X, (int)npcCenter.Y, mod.NPCType("m_diggerhead"));
                    NetMessage.SendData(23, -1, -1, null, pups, 0f, 0f, 0f, 0, 0, 0);
                    pupsSpawned = true;
                }
            }
            if (pupsSpawned)
            {
                Main.npc[pups].realLife = Main.npc[pups].life;
                if (!Main.npc[pups].active)
                {
                    pupsSpawned = false;
                    magnoClone = true;
                }
            }
            if (magnoClone)
            {
                clone = NPC.NewNPC((int)npcCenter.X, (int)npcCenter.Y, mod.NPCType("_boss"));
                Main.npc[clone].color = Color.Gold;
                Main.npc[clone].scale = 0.6f;
                timeLeft = 600;
                magnoClone = false;
            }
            if(timeLeft > 0)
                timeLeft--;
            if (timeLeft == 0)
            {
                Main.npc[clone].active = false;
                timeLeft = 600;
            }
            #endregion
        }
    }
}
