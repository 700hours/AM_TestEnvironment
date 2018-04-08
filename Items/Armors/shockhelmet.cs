using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class shockhelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shock Helmet");
        }
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.maxStack = 1;
            item.value = 100;
            item.rare = 2;
            item.defense = 5;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(9);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
        //  drawHair = true;
        }
    }
}
