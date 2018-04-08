using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class shockgreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shock Greaves");
        }
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 100;
            item.rare = 2;
            item.defense = 3;
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
