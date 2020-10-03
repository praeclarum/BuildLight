using System;
namespace BuildLightPlugin
{
	public static class Helper
	{
		public Helper ()
		{
		}

		public Color HexToColor (string hexString)
		{
			//replace # occurences
			if (hexString.IndexOf ('#') != -1)
				hexString = hexString.Replace ("#", "");

			int r, g, b = 0;

			r = int.Parse (hexString.Substring (0, 2), NumberStyles.AllowHexSpecifier);
			g = int.Parse (hexString.Substring (2, 2), NumberStyles.AllowHexSpecifier);
			b = int.Parse (hexString.Substring (4, 2), NumberStyles.AllowHexSpecifier);

			return Color.FromArgb (r, g, b);
		}

		public (int r, int g, int b) HexToRGB (string hexString)
		{
			//replace # occurences
			if (hexString.IndexOf ('#') != -1)
				hexString = hexString.Replace ("#", "");

			int r, g, b = 0;

			r = int.Parse (hexString.Substring (0, 2), NumberStyles.AllowHexSpecifier);
			g = int.Parse (hexString.Substring (2, 2), NumberStyles.AllowHexSpecifier);
			b = int.Parse (hexString.Substring (4, 2), NumberStyles.AllowHexSpecifier);

			return (r, g, b);
		}
	}
}
