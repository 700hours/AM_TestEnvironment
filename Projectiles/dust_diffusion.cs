using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class dust_diffusion : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 8;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 0.1f;
            projectile.alpha = 255;
            projectile.magic = true;
        }
        int ticks = 0;
        public override void AI()
        {
            ticks++;
            if (ticks == 1)
            {
                int Dust1 = Dust.NewDust(projectile.position, 1, 1, 73, projectile.ai[0], projectile.ai[1], 0, Color.White, 1f);
                Main.dust[Dust1].noGravity = true;
            }
        }
    }
}
