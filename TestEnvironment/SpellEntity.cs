using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
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
    public class SpellEntity
    {
        private Player myPlayer
        {
            get { return Main.player[owner]; }
        }
        private Player player
        {
            get { return Main.player[target]; }
        }
        private NPC npc
        {
            get { return Main.npc[target]; }
        }
        private Rectangle HitBox
        {
            get
            {
                if (type == mod.ItemType<Items.fire_spell>())
                {
                    return new Rectangle((int)position.X, (int)position.Y, width, height);
                }
                else return default(Rectangle);
            }
        }
        private float PlrDist
        {
            get { return Vector2.Distance(myPlayer.position - Main.player[target].position, Vector2.Zero); }
        }
        private float NPCDist
        {
            get { return Vector2.Distance(myPlayer.position - npc.position, Vector2.Zero); }
        }

        public bool active;
        private bool activated;
        public int ID;
        private int ticks;
        public int type;
        public int owner;
        public int target;
        private int width = 16;
        private int height = 16;
        public int damage;
        private float radius = 1f;
        public float scale;
        public float alpha;
        private float range;
        public Vector2 position;
        public Color newColor;
        private Texture2D texture;
        private Mod mod = ModLoader.GetMod("TestEnvironment");
        
        public static int NewEntity(Vector2 targetVector, Player myPlayer, Color color, int damage, float range, float radius, float scale, float alpha, int type, int owner, int target)
        {
            int num = 1000;
            for (int i = 0; i < num; i++)
            {
                if (!TestWorld.Spells[i].active)
                {
                    num = i;
                    break;
                }
            }
            if (num == 1000)
            {
                return num;
            }
            foreach (SpellEntity spells in TestWorld.Spells)
            {
                if (spells.ID != num && spells.target == target)
                {
                    return 1000;
                }
            }
            TestWorld.Spells[num] = new SpellEntity();
            TestWorld.Spells[num].active = true;
            TestWorld.Spells[num].position = targetVector;
            TestWorld.Spells[num].newColor = color;
            TestWorld.Spells[num].damage = damage;
            TestWorld.Spells[num].range = range;
            TestWorld.Spells[num].radius = radius;
            TestWorld.Spells[num].scale = scale;
            TestWorld.Spells[num].alpha = alpha;
            TestWorld.Spells[num].type = type;
            TestWorld.Spells[num].owner = owner;
            TestWorld.Spells[num].target = target;
            TestWorld.Spells[num].radius = 5f;
            TestWorld.Spells[num].ID = num;
            return num;
        }
        public Texture2D ParticleTex(int type)
        {
            return mod.GetTexture("Gores/chain");
        }

        public void Update()
        {
            ticks++;

            CheckHit(npc);
            DrawReticule(player, npc, Main.spriteBatch);

            if (type == mod.ItemType<Items.heal_spell>())
            {
                if (PlrDist > range || !player.active)
                    Kill();
                if (Main.LocalPlayer.HeldItem.type != type && Main.netMode < 2)
                    Kill();
            }
            if (type == mod.ItemType<Items.fire_spell>())
            {
                if (NPCDist > range || !npc.active || npc.life <= 0)
                    Kill();
                if (Main.LocalPlayer.HeldItem.type != type && Main.netMode < 2)
                    Kill();
            }
        }
        public bool UseItem(Player player, int type)
        {
            if (player.controlUseItem && player.HeldItem.type == type)
            {
                return true;
            }
            return false;
        }
        public void Kill()
        {
            active = false;
            target = -1;
            newColor = Color.White * 255;
        }

        public void CheckHit(NPC npc)
        {
            if (HitBox.Intersects(npc.Hitbox) && ticks % 10 == 0)
            {
                npc.life -= damage;
                npc.HitEffect(0, damage);
            }
        }
        public void DrawReticule(Player player, NPC npc, SpriteBatch sb)
        {
            if (UseItem(player, type))
                activated = true;
            if (activated)
                radius -= 0.15f;

            Texture2D tex = ParticleTex(type);

            if (active)
            {
                if (type == mod.ItemType<Items.heal_spell>())
                {
                    Main.spriteBatch.Draw(tex,
                        new Vector2(player.Center.X, player.position.Y - player.height) - Main.screenPosition,
                        new Rectangle(0, 0, tex.Width, tex.Height),
                        player.eyeColor, 0f, new Vector2(tex.Width / 2, tex.Height / 2), 1f, SpriteEffects.FlipVertically, 0f);
                }
                else if (type == mod.ItemType<Items.fire_spell>())
                {
                    Main.spriteBatch.Draw(tex,
                        new Vector2(npc.Center.X, npc.position.Y - npc.height / 1.2f) - Main.screenPosition,
                        new Rectangle(0, 0, tex.Width, tex.Height),
                        Color.White, 0f, new Vector2(tex.Width / 2, tex.Height / 2), 1f, SpriteEffects.FlipVertically, 0f);
                }
            }
        }

        private Vector2 Rotation(NPC npc, float radius, float degrees)
        {
            Vector2 center = position;
            float x = center.X + (float)(radius * Math.Cos(MathHelper.ToRadians(degrees)));
            float y = center.Y + (float)(radius * Math.Sin(MathHelper.ToRadians(degrees)));

            return new Vector2(x, y);
        }
    }
}
