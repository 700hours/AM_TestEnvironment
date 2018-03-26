using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestEnvironment.Items
{
    public class mimic_summon : ModItem
    {
        public override void SetStaticDefaults()
        {
        //  DisplayName.SetDefault("Test Summoner");
        //  Tooltip.SetDefault("Reminiscent of something...");
        }
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.useTime = 45;
            item.useAnimation = 45;
            item.useStyle = 4;
            item.value = 100;
            item.rare = 2;
            item.autoReuse = false;
            item.consumable = false;
            item.noMelee = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(9);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            int npcWidth = 100;
            int npcHeight = 93;
            float npcX = player.position.X - 8 + (player.width - npcWidth) * 0.5f;
            float npcY = player.position.Y - npcHeight + 40;
            int spawn = NPC.NewNPC((int)npcX, (int)npcY, mod.NPCType("m_mimic"));

            //	if (spawn < 0 || spawn >= 300)
            //		return;

            Main.npc[spawn].target = Main.myPlayer;
            //	Main.npc[spawn].RunMethod("CustomInit", null);

            return true;
        }
    }
}
