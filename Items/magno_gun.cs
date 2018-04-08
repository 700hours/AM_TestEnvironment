using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    public class magno_gun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Gun");
            Tooltip.SetDefault("20% chance to fire homing bullets"
                +   "\n20% chance not to consume ammo");
        }
        public override void SetDefaults()
        {
            item.width = 62;
            item.height = 24;
            item.scale = 0.75f;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 5;
            item.damage = 10;
            item.knockBack = 3f;
            item.shootSpeed = 18f;
            item.value = 2500;
            item.rare = 1;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.consumable = false;
            item.noMelee = true;
            item.ranged = true;
            //  vanilla shooting method
            //  item.shoot = ProjectileID.Bullet;
            item.useAmmo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(9);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        Vector2 playerCenter;
        public override void UseStyle(Player player)
        {
            playerCenter = new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2);
            float HalfPi = (float)Math.PI / 2f;
            if (Main.MouseWorld.X - player.position.X > 0) player.direction = 1;
            else player.direction = -1;
            player.itemRotation = (float)Math.Atan2((Main.MouseWorld.Y - playerCenter.Y) * player.direction, (Main.MouseWorld.X - playerCenter.X) * player.direction);
            player.itemLocation.X -= player.direction * (player.width / 8) * (1f - (float)Math.Abs(player.itemRotation) / HalfPi);
        }

        int Proj;
        int ticks = 0;
        float nX, nY;
        float Offset;
        Vector2 position;
        Vector2 speed;
        public override bool UseItem(Player player)
        {
            ticks++;
            if (player.direction == 1)
                position = new Vector2(player.itemLocation.X + (player.direction * player.width), player.itemLocation.Y);
            else position = new Vector2(player.itemLocation.X - (player.direction * player.width), player.itemLocation.Y);
            speed = new Vector2(player.direction * Distance(null, player.itemRotation, 16f).X, player.direction * Distance(null, player.itemRotation, 16f).Y);

            float radius = player.width * 3f;
            if (player.direction == 1)
            {
                nX = position.X + (float)(radius * Math.Cos(player.itemRotation));
                nY = position.Y + (float)(radius * Math.Sin(player.itemRotation));
            }
            else
            {
                nX = position.X + (float)((radius * -1 / 4) * Math.Cos(player.itemRotation));
                nY = position.Y + (float)((radius * -1 / 4) * Math.Sin(player.itemRotation));
            }
            if (player.itemRotation >= MathHelper.ToRadians(315) && player.itemRotation <= MathHelper.ToRadians(360) ||
                player.itemRotation >= MathHelper.ToRadians(135) && player.itemRotation <= MathHelper.ToRadians(180) ||
                player.itemRotation >= MathHelper.ToRadians(0) && player.itemRotation <= MathHelper.ToRadians(45))
            {
                Offset = 20f;
            }
            else Offset = 8f;
            Proj = Projectile.NewProjectile(new Vector2(nX + Offset, nY + Offset / 2), speed, mod.ProjectileType<magno_bullet>(), 15 + Main.rand.Next(-3, 5), 3f, player.whoAmI, player.itemRotation, 0f);
            if (Main.netMode == 1) NetMessage.SendData(27, -1, -1, null, Proj);
            
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .20f;
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}