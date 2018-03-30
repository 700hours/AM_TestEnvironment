using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Magno
{
    public class m_ore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Block");
            Tooltip.SetDefault("Placeable tile");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.value = 100;
            item.rare = 1;
            item.autoReuse = true;
            item.useTurn = true;
            item.consumable = true;
            item.noMelee = true;
            item.createTile = mod.TileType("m_tile");
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