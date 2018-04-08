using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Buffs
{
    class magno_summon : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Magno Staff");
            Description.SetDefault("Summons a cannon-wielding minion");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            ProjectPlayer modPlayer = player.GetModPlayer<ProjectPlayer>(mod);
            if (player.ownedProjectileCounts[mod.ProjectileType("magno_minion")] > 0)
            {
                modPlayer.magnoMinion = true;
            }
            if (!modPlayer.magnoMinion)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
