using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Tiles.Magno
{
    public class m_tile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            //  connects with dirt
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = false;
            //  shine when around torch
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            //  for spelunker potions
            dustType = 1;
            drop = mod.ItemType("m_bars");
            //  UI map tile color
            AddMapEntry(new Color(200, 200, 200));
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }
    }
}
