using UnityEngine;
using System.Collections.Generic;

// Flood Fill implementation for Unity3D C#
// Used in: https://www.elfgamesworks.com/2016/12/14/identify-unwanted-maze-solutions-using-flood-fill-with-unity3d/
public static class ImageUtils
{

	public struct Point {

		public int x;
		public int y;

		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

  // fgPoints : An array of points (x,y) at the contour of the core of the terrain
  // bgPoints: An array of points (x,y) at the border representing the background

	
	
	static bool CheckValidity(Texture2D texture, int width, int height, Point p, Color sourceColor, float tollerance) {
		if (p.x < 0 || p.x >= width) {
			return false;
		}
		if (p.y < 0 || p.y >= height) {
			return false;
		}

		var color = texture.GetPixel(p.x, p.y);

		var distance = Mathf.Abs (color.r - sourceColor.r) +  Mathf.Abs (color.g - sourceColor.g) +  Mathf.Abs (color.b - sourceColor.b);
		return distance <= tollerance;
	}
}