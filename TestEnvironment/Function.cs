using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Terraria;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TestEnvironment
{
    public class Function
    {
        public static Function Func;
        public Function()
        {
            Func = this;
        }
        private float degrees;
        private const float radians = 0.017f;
        public Player myPlayer
        {
            get { return Main.player[Main.myPlayer]; }
        }
        public Mod mod = ModLoader.GetMod("TestEnvironment");
        private Texture2D texture;
        public bool healProx, fireProx;
        public void Update()
        {
            foreach (NPC npc in Main.npc)
            {
                if (PlrNPCDist(LocalPlayer(), npc))
                {
                    fireProx = LocalPlayer().HeldItem.type == mod.ItemType<Items.fire_spell>();
                    break;
                }
            }
        }
        public void Draw()
        {
            Player player = LocalPlayer();
            texture = mod.GetTexture("Gores/chain");

            foreach (NPC npc in Main.npc)
                if (PlrNPCDist(player, npc) && npc.active && !npc.immortal && !npc.dontTakeDamage)
                    Main.spriteBatch.Draw(texture,
                            new Vector2(npc.position.X + npc.width / 2, (npc.position.Y - 64) - 4 * intensity) - Main.screenPosition,
                            new Rectangle(0, 0, texture.Width, texture.Height),
                            Color.White, 0f, new Vector2(npc.width / 2, npc.height - 64), 1f, SpriteEffects.FlipVertically, 0f);    
        }
        public Player LocalPlayer()
        {
            if (Main.netMode != 0)
                return Main.LocalPlayer;
            else
                return myPlayer;
        }
        public bool PlrNPCDist(Player player, NPC npc)
        {
            return Vector2.Distance(player.position - npc.position, Vector2.Zero) < 384f;
        }
        public bool PlrDist(Player player)
        {
            return Vector2.Distance(LocalPlayer().position - player.position, Vector2.Zero) < 384f;
        }
        public float intensity
        {
            get
            {
                degrees += radians * 7.5f;
                float radius = radians * 360f;
                float point = (float)(radius * Math.Cos(degrees));
                return (radius - point) / radius;
            }
        }
    }
}
