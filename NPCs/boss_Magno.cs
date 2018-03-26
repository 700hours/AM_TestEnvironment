using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestEnvironment.NPCs
{
    public class boss_Magno : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 128;
            npc.height = 128;
            npc.friendly = false;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }
        bool sweep = false, spinAttack = false;
        bool pupsSpawned = false;
        bool magnoClone = false;
        int dust;
        int pups, clone, timeLeft = 600;
        int ticks = 0;
        int choose = 0;
        float TargetAngle, PlayerAngle;
        float degrees = 0;
        float Depreciate = 80, Point;
        const float Time = 80;
        const float radians = 0.017f;
        Rectangle target;
        Vector2 oldPosition;
        Vector2 npcCenter;
        Vector2 Start, End;
        public override void AI()
        {
            npc.ai[0]++;
            npc.TargetClosest(true);
            npcCenter = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);

            Player player = Main.player[npc.target];

            PlayerAngle = (float)Math.Atan2(player.position.Y - npc.position.Y,
                                            player.position.X - npc.position.X);
            #region default pattern
            if (npc.ai[0] < 720)
            {
                if(npc.ai[0] == 1 || npc.ai[0]%120 == 0 || Vector2.Distance(player.position - npc.position, Vector2.Zero) > 400f)
                {
                    oldPosition = new Vector2(player.position.X + Random(), player.position.Y + Random());

                    TargetAngle = (float)Math.Atan2(oldPosition.Y - npc.position.Y,
                                                    oldPosition.X - npc.position.X);

                    npc.velocity.X = Distance(null, TargetAngle, 4f).X;
                    npc.velocity.Y = Distance(null, TargetAngle, 4f).Y;

                    npc.rotation = TargetAngle;

                    if (npc.position.Y > player.position.Y)
                        npc.velocity.Y--;
                }
            }
            #endregion
            if(npc.ai[0] > 720)
            {
                #region sweep
                if (npc.ai[0] == 780)
                {
                    choose = Main.rand.Next(1, 2);
                }
                if (choose == 1)
                {
                    Start.X = player.position.X - 256;
                    Start.Y = player.position.Y - 448;

                    End.X = player.position.X + 256;
                    End.Y = Start.Y;

                    sweep = true;
                    choose = 0;
                }
                if (Depreciate > 0 && sweep)
                {
                    Depreciate--;
                    Point = (Time - Depreciate) / Time;
                    npc.position = Vector2.Lerp(Start, End, Point);

                    npc.rotation = radians * 90;
                    if (Depreciate % 8 == 0)
                    {
                        int attack = Projectile.NewProjectile(npc.position + new Vector2(npc.width / 2, npc.height / 2 + 32), Vector2.Zero, 134, 12 + Main.rand.Next(-2, 8), 2f, player.whoAmI, 0f, 0f);
                        Projectile proj = Main.projectile[attack];
                        proj.velocity.X += Distance(null, npc.rotation, 4f).X;
                        proj.velocity.Y += Distance(null, npc.rotation, 4f).Y;
                        proj.friendly = false;
                        proj.hostile = true;
                    }
                }
                else if (Depreciate == 0)
                {
                    sweep = false;
                    npc.ai[0] = 0;
                    Depreciate = Time;

                    npc.position = new Vector2(player.position.X + Random(), player.position.Y + Random());
                }
                #endregion
                #region spin
                if (choose == 2)
                {
                    oldPosition.X = player.position.X - npc.width / 2;
                    oldPosition.Y = player.position.Y - 384;
                    spinAttack = true;
                    choose = 0;
                }
                if (spinAttack)
                {
                    npc.position = oldPosition;

                    degrees += radians * 15;
                    npc.rotation = degrees;
                    if (degrees >= radians * 360)
                        degrees = radians;
                    if (degrees >= radians * 45f && degrees <= radians * 146.25f)
                    {
                        int attack = Projectile.NewProjectile(npc.position + new Vector2(npc.width/2, npc.height/2 + 32), Vector2.Zero, 134, 20, 2f, player.whoAmI, 0f, 0f);
                        Projectile proj = Main.projectile[attack];
                        proj.velocity.X += Distance(null, npc.rotation, 4f).X;
                        proj.velocity.Y += Distance(null, npc.rotation, 4f).Y;
                        proj.friendly = false;
                        proj.hostile = true;
                    }
                    ticks++;
                    if (ticks > 300)
                    {
                        spinAttack = false;
                        degrees = radians;
                        npc.rotation = PlayerAngle;
                        ticks = 0;
                        npc.ai[0] = 0;

                        npc.position = new Vector2(player.position.X + Random(), player.position.Y + Random());
                    }
                }
                #endregion
            }
            #region magno clone sequence
            if (!pupsSpawned && Main.rand.Next(0, 1200) == 0)
            {
                for (int k = 0; k < 4; k++)
                {
                    pups = NPC.NewNPC((int)npcCenter.X + Main.rand.Next(-npc.width, npc.width), (int)npcCenter.Y, mod.NPCType("m_diggerhead"));
                    NetMessage.SendData(23, -1, -1, null, pups, 0f, 0f, 0f, 0, 0, 0);
                    pupsSpawned = true;
                }
            }
            if (pupsSpawned)
            {
                Main.npc[pups].realLife = Main.npc[pups].whoAmI;
                if (!Main.npc[pups].active)
                {
                    pupsSpawned = false;
                    magnoClone = true;
                }
            }
            if (magnoClone)
            {
                clone = NPC.NewNPC((int)npcCenter.X, (int)npcCenter.Y + 128, mod.NPCType("m_mimic"));
                Main.npc[clone].color = Color.Gold;
                Main.npc[clone].scale = 0.6f;
                timeLeft = 600;
                magnoClone = false;
            }
            if(timeLeft > 0)
                timeLeft--;
            if (timeLeft < 60)
            {
            /*  degrees += radians * 11.25f;
                Main.npc[clone].rotation = degrees;
                Main.npc[clone].scale++;
                Main.npc[clone].velocity = Vector2.Zero;
            */
                if (timeLeft == 0)
                {
                    Main.npc[clone].active = false;
                    timeLeft = 600;
                }
            }
            #endregion
        }
        public void SpawnDust(Vector2 vector, int width, int height, int dustType, Color color, float scale)
        {
            dust = Dust.NewDust(vector, width, height, dustType, 0f, 0f, 255, color, scale);
            Main.dust[dust].noGravity = true;
        }
        public float Random()
        {
            return Main.rand.Next(-400, 400);
        }
        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}
