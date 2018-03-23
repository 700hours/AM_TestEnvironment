using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod
{
	public class ArchaeaMod : Mod
	{
		public void SetModInfo(out string name, ref ModProperties properties)
		{
			name = "The Archaea Mod";
			properties.Autoload = true;
			properties.AutoloadGores = true;
			properties.AutoloadSounds = true;
		}
	}
}
