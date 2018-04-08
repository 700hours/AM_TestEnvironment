using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class m_yoyoprojectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mango Yoyo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 6.5f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 275f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13.5f;
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ignoreWater = false;
            projectile.scale = 1f;
            projectile.melee = true;
            projectile.extraUpdates = 0;
        }
    }
}