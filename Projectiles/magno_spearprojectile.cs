using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class magno_spearprojectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 78;
            projectile.scale = 1f;
            projectile.aiStyle = 19;
            projectile.timeLeft = 300;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.ownerHitCheck = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.melee = true;
        }

        public float movementFactor
        {
            get { return projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            
            projectile.direction = player.direction;
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation;

        //  need to deal with mounts in revision
        //  Vector2 mountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
        //  projectile.position = mountedCenter + new Vector2(projectile.width / 2, projectile.height / 2);

            if (!player.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                    projectile.netUpdate = true;
                }
                if(player.itemAnimation < player.itemAnimationMax / 3)
                {
                    movementFactor -= 2.4f;
                }
                else
                {
                    movementFactor += 2.1f;
                }
            }
            projectile.position += projectile.velocity * movementFactor;

            if (player.itemAnimation == 0)
            {
                projectile.Kill();
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            if(projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }
        }
    }
}
