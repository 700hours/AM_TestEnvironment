using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    class magno_spear : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 54;
            item.height = 56;
            item.useTime = 24;
            item.useAnimation = 18;
            item.useStyle = 5;
            item.damage = 20;
            item.knockBack = 3f;
            item.shootSpeed = 4f;
            item.value = 2500;
            item.rare = 1;
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
            item.shoot = mod.ProjectileType<magno_spearprojectile>();
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
