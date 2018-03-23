using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Tiles.Magno
{
    public class m_dirt : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            //  connects with dirt
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            drop = mod.ItemType("m_dirt");
            //  UI map tile color
            AddMapEntry(new Color(200, 200, 200));
        }
    }
}