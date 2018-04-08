using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    class magno_javelin : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.useTime = 24;
            item.useAnimation = 18;
            item.useStyle = 5;
            item.damage = 20;
            item.knockBack = 3f;
            item.shootSpeed = 4f;
            item.value = 250;
            item.rare = 1;
            item.maxStack = 250;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            //  or vanilla sound
            item.UseSound = SoundID.Item1;

            item.autoReuse = false;
            item.consumable = false;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.melee = true;
            //  vanilla shooting method
            item.shoot = mod.ProjectileType<magno_javelinprojectile>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(9);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        int Proj = 0;
        Vector2 speed;
        /*
        public override bool UseItem(Player player)
        {
            speed = new Vector2(player.direction * Distance(null, player.itemRotation, 16f).X, player.direction * Distance(null, player.itemRotation, 16f).Y);
            Proj = Projectile.NewProjectile(player.Center, speed, mod.ProjectileType<magno_javelinprojectile>(), 18, 0f, player.whoAmI, 0f, 0f);
            return true;
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }   */
    }
}
