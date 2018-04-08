using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class bullet_homing : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 4;
            projectile.scale = 1f;
            projectile.aiStyle = -1;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.ranged = true;
        }

        public void Initialize()
        {
            projectile.rotation = projectile.ai[0];
            oldVel = projectile.velocity;
        }
        bool init = false;
        int ticks = 0;
        int pDust = 0;
        float newAngle;
        Vector2 oldPos;
        Vector2 oldVel;
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }
            for (int i = 0; i < 6; i++)
            {
                int pDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 0, Color.Red, 1f);
                Main.dust[pDust].noGravity = true;
            }
            ticks++;
            if (ticks % 10 == 0)
            {
                oldPos = projectile.position;
            }

            newAngle = (float)Math.Atan2(projectile.position.Y - oldPos.Y, projectile.position.X - oldPos.X);

            projectile.rotation = newAngle;

            foreach (NPC n in Main.npc)
            {
                if (n.active && !n.friendly && !n.dontTakeDamage)
                {
                    Vector2 nmePosition = new Vector2(n.position.X + n.width / 2, n.position.Y + n.height / 2);
                    if (Vector2.Distance(nmePosition - projectile.position, Vector2.Zero) < 256f)
                    {
                        float Angle = (float)Math.Atan2(n.position.Y - projectile.position.Y, n.position.X - projectile.position.X);
                        projectile.velocity = Vector2.Zero;
                        projectile.position += Distance(null, Angle, 2f);
                    }
                    else
                    {
                        projectile.velocity = oldVel;
                    }
                }
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
