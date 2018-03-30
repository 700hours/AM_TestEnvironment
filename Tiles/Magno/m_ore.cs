using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Tiles.Magno
{
	public class m_ore : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
            //  connects with dirt
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            //  for spelunker potions
            dustType = 1;
			drop = mod.ItemType("m_tile");
            //  UI map tile color
            AddMapEntry(new Color(200, 200, 200));
		}
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
	}
}