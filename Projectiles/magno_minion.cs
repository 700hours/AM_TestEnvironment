using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class magno_minion : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.scale = 1f;
            projectile.damage = 0;
            projectile.aiStyle = -1;
            projectile.timeLeft = 18000;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.ranged = true;
            projectile.netImportant = true;
        }
        bool target = false, targeted = false;
        int ticks = 0;
        int Proj1;
        int npcTarget = 0, oldNpcTarget = -1;
        float Angle, npcAngle;
        Vector2 orbitPosition;
        Vector2 npcCenter;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            ProjectPlayer modPlayer = player.GetModPlayer<ProjectPlayer>(mod);

            ticks++;
            projectile.damage = 0;

            if (player.dead || !player.HasBuff(mod.BuffType("magno_summon")))
            {
                modPlayer.magnoMinion = false;
            }
            if (modPlayer.magnoMinion)
            {
                projectile.timeLeft = 2;
            }

            orbitPosition = player.position + new Vector2(0f, -64f);
            Angle = (float)Math.Atan2(orbitPosition.Y - projectile.position.Y, orbitPosition.X - projectile.position.X);
            if (!target)
            {
                if (Vector2.Distance(orbitPosition - projectile.position, Vector2.Zero) > 32f)
                {
                    projectile.position += Distance(null, Angle, 4f);
                }
                else if (Vector2.Distance(orbitPosition - projectile.position, Vector2.Zero) > 128f)
                {
                    projectile.position += Distance(null, Angle, 16f);
                }
            }
            
            foreach (NPC n in Main.npc)
            {
                if (n.active && !n.friendly && !n.dontTakeDamage && n.target == player.whoAmI)
                {
                    npcCenter = new Vector2(n.position.X + n.width / 2, n.position.Y + n.height / 2);
                    if ((n.life <= 0 || !targeted || npcTarget != n.whoAmI) &&
                         Vector2.Distance(npcCenter - projectile.position, Vector2.Zero) < 384f)
                    {
                        oldNpcTarget = npcTarget;
                        npcTarget = n.whoAmI;
                        targeted = true;
                    }
                }
                else projectile.spriteDirection = player.direction * -1;
            }
            if(targeted)
            {
                NPC n = Main.npc[npcTarget];
                npcCenter = new Vector2(n.position.X + n.width / 2, n.position.Y + n.height / 2);
                npcAngle = (float)Math.Atan2(npcCenter.Y - projectile.position.Y, npcCenter.X - projectile.position.X);
                projectile.rotation = npcAngle;
                if (Vector2.Distance(npcCenter - projectile.position, Vector2.Zero) < 384f)
                {
                    if (!projectile.Hitbox.Intersects(n.Hitbox))
                    {
                        projectile.position += Distance(null, npcAngle, 16f);
                    }
                    else
                    {
                        projectile.position = n.position + new Vector2(n.width / 2, n.height / 2);
                        if (ticks % 120 == 0)
                        {
                            for (float k = 0; k < MathHelper.ToRadians(360); k += 0.017f * 9)
                            {
                                int Proj1 = Projectile.NewProjectile(projectile.position + new Vector2(projectile.width / 2, projectile.height / 2), Distance(null, k, 16f), mod.ProjectileType("dust_diffusion"), (int)(12 * player.minionDamage), 4f, projectile.owner, Distance(null, k, 16f).X, Distance(null, k, 16f).Y);
                                if (Main.netMode == 1) NetMessage.SendData(27, -1, -1, null, Proj1);
                            }
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/IceBeamChargeShot"), projectile.position);
                        }
                    }
                    target = true;
                }
                else target = false;
                if (!n.active || npcTarget != n.whoAmI)
                {
                    target = false;
                    targeted = false;
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
