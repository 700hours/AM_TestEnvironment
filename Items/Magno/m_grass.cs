using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Magno
{
    public class m_grass : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Grass");
            Tooltip.SetDefault("For graphical testing purposes");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 99;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.value = 100;
            item.rare = 1;
            item.autoReuse = true;
            item.useTurn = true;
            item.consumable = true;
            item.noMelee = true;
            item.createTile = mod.TileType("m_dirt");
        }
        //  Recipe standin is dirt 
        /*
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(9);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }   */
    }
}