using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    public class magno_book : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 62;
            item.height = 24;
            item.scale = 1f;
            item.useTime = 30;
            item.useAnimation = 30;
            item.damage = 15;
            item.mana = 8;
            item.useStyle = 1;
            item.value = 2500;
            item.rare = 2;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            item.autoReuse = false;
            item.consumable = false;
            item.noMelee = true;
            item.magic = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(9);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        int Proj1;
        public override bool UseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("magno_orb")] < 1)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/conjure"), player.Center);
                Proj1 = Projectile.NewProjectile(player.position + new Vector2(player.width / 2, player.height / 2), Vector2.Zero, mod.ProjectileType("magno_orb"), (int)(15 * player.magicDamage), 4f, player.whoAmI, 0f, 0f);
                if (Main.netMode == 1) NetMessage.SendData(27, -1, -1, null, Proj1);
            }
            return true;
        }
    }
}
