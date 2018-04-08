using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class boss_magnohead : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 54;
            npc.height = 46;
            npc.scale = 1.3f;
            npc.friendly = false;
            npc.aiStyle = 6;
          //aiType = NPCID.Worm;
            npc.boss = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
            npc.damage = 35;
            npc.defense = 6;
            npc.lifeMax = 8450;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        bool TailSpawned = false;
        bool flames = false;
        bool pupsSpawned = false;
        bool magnoClone = false;
        bool soundOnce = false;
        bool part2 = false;
        int Previous, digger, despawn = 210;
        int pups = 255, clone, timeLeft = 600;
        int flamesID;
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
            npc.ai[0]++;
            #region spawn parts
            if (npc.ai[0] <= 1 || npc.ai[0] >= 400)
            {
                //  npc.damage = 35;
                //  Main.npc[digger].damage = 35;
                if (!TailSpawned)
                {
                    Previous = npc.whoAmI;
                    for (int num36 = 0; num36 < 14; num36++)
                    {
                        if (num36 >= 0 && num36 < 13)
                        {
                            digger = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, mod.NPCType("boss_magnobody"), npc.whoAmI);
                        }
                        else
                        {
                            digger = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, mod.NPCType("boss_magnotail"), npc.whoAmI);
                        }
                        Main.npc[digger].color = npc.color;
                        Main.npc[digger].scale = npc.scale;
                        Main.npc[digger].lifeMax = npc.lifeMax;
                        Main.npc[digger].life = Main.npc[digger].lifeMax;
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
                    npc.netUpdate = true;
                    TailSpawned = true;
                }
            }
        /*  else if (npc.ai[0] >= 2)
            {
                npc.damage = 30;
            }   */
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
            npcCenter = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);

            if (Vector2.Distance(player.position - npc.position, Vector2.Zero) < 250f)
            {
                if (npc.ai[0] % 8 == 0)
                {
                    int attack = Projectile.NewProjectile(npc.position + new Vector2(npc.width / 2, npc.height / 2), Vector2.Zero, 96, 8 + Main.rand.Next(-2, 5), 0.5f, player.whoAmI, 0f, 0f);
                    Projectile proj = Main.projectile[attack];
//                  if (npc.life > npc.lifeMax / 2 || Main.npc[digger].life > Main.npc[digger].lifeMax / 2)
                    proj.velocity.X = Distance(null, npc.rotation + radians * 270f, 16f).X;
                    proj.velocity.Y = Distance(null, npc.rotation + radians * 270f, 16f).Y;
                    proj.timeLeft = 45;
                    proj.penetrate = 1;
                    proj.tileCollide = false;
                    proj.friendly = false;
                    proj.hostile = true;
                }
            }   
            #region spirit flames
            if (!flames && Main.rand.Next(0, 6000) == 0)
            {
                // playsound
                for (int k = 1; k < 5; k++)
                {
                    degrees = 90f;
                    radius = 128f;
                    center = player.position;
                    float nX = center.X + (float)(radius * Math.Cos(degrees * k));
                    float nY = center.Y + (float)(radius * Math.Sin(degrees * k));

                    if (npc.life > npc.lifeMax / 2 || Main.npc[digger].life > Main.npc[digger].lifeMax / 2)
                    {
                        flamesID = NPC.NewNPC((int)nX, (int)nY, mod.NPCType("m_flame"));
                        Main.npc[flamesID].damage = 10;
                    }
                    else
                    {
                        flamesID = NPC.NewNPC((int)nX, (int)nY, mod.NPCType("c_flame"));
                        Main.npc[flamesID].damage = 16;
                    }
                    Main.npc[flamesID].ai[1] = degrees * k;
                    Main.npc[flamesID].scale = 1.2f;
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(23, -1, -1, null, flamesID, 0f, 0f, 0f, 0, 0, 0);
                    }
                    flames = true;
                }
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/curse"), npc.Center);
            }
            if (flames)
            {
                radius -= 0.5f;
                NPC n = Main.npc[flamesID];
                if (n.active = false || radius <= 1f)
                    flames = false;
            }
            #endregion
            
            // needs to sprite/code separate NPC for magno clone
            #region magno clone sequence
        /*  ticks++;
            npcCenter = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);
            if (!Main.npc[(int)npc.ai[3]].active)
            {
                if (!pupsSpawned && ticks % 600 == 0)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        npc.ai[2] = NPC.NewNPC((int)npcCenter.X + Main.rand.Next(-64, 64), (int)npcCenter.Y, mod.NPCType("m_puphead"));
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendData(23, -1, -1, null, (int)npc.ai[2], 0f, 0f, 0f, 0, 0, 0);
                        }
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/conjure"), npc.Center);
                        pupsSpawned = true;
                    }
                }
                if (pupsSpawned)
                {
                    if (!Main.npc[(int)npc.ai[2]].active && Main.npc[(int)npc.ai[2]] != null)
                    {
                        pupsSpawned = false;
                        magnoClone = true;
                    }
                }
                if (magnoClone)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/conjure"), npc.Center);
                    npc.ai[3] = NPC.NewNPC((int)npcCenter.X, (int)npcCenter.Y, mod.NPCType("boss_magnohead"));
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(23, -1, -1, null, (int)npc.ai[3], 0f, 0f, 0f, 0, 0, 0);
                    }
                    Main.npc[(int)npc.ai[3]].color = Color.Gold;
                    Main.npc[(int)npc.ai[3]].scale = 0.6f;
                    Main.npc[(int)npc.ai[3]].lifeMax = npc.lifeMax / 4;
                    Main.npc[(int)npc.ai[3]].life = Main.npc[(int)npc.ai[3]].lifeMax;
                    timeLeft = 600;
                    magnoClone = false;
                }
            }
            if (timeLeft > 0)
            {
                timeLeft--;
                if (Main.expertMode && Main.npc[(int)npc.ai[3]].active)
                {
                    npc.immortal = true;
                }
            }
            else if (timeLeft == 0)
            {
                npc.immortal = false;
                Main.npc[(int)npc.ai[3]].HitEffect(0, 10.0);
                Main.npc[(int)npc.ai[3]].active = false;
                timeLeft = 600;
            }   */
            #endregion

            if (npc.life < npc.lifeMax / 2 || Main.npc[digger].life < Main.npc[digger].lifeMax / 2)
            {
                if (!soundOnce)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/blast"), npc.Center);
                    soundOnce = true;
                }
                npc.color = Color.Orange;
                Main.npc[digger].color = Color.Orange;

                part2 = true;
            } 
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}
