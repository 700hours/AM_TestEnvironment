using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Magno
{
    public class m_yoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Yoyo");
            Tooltip.SetDefault("Throws a Magno yoyo");
        }
        public override void SetDefaults()
        {
            item.width = 19;
            item.height = 13;
            item.useTime = 10;
            item.useAnimation = 25;
            item.useStyle = 25;
            item.value = 3000;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.consumable = false;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.melee = true;
            item.shoot = mod.ProjectileType("m_YoyoProjectile");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(9);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}