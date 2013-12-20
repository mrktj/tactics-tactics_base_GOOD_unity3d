using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;

public static class MapLoader {

  private static string delimiters = 
    @"TemplatePath:\r+\n+" + @"|Grid:\r+\n+" + 
    @"|MapEntities:\r+\n+";
  private static string subdelimiters = @" |\r+\n+";

  public static void LoadFromFile(Map m, string filename) {
    TextAsset mapfile = (TextAsset) Resources.Load("MapFiles/" + filename, typeof(TextAsset));
    string[] sections = Regex.Split(mapfile.text, delimiters);

    string[] templatePath = Regex.Split(sections[1],subdelimiters);
    Texture2D template = Resources.Load<Texture2D>("Sprites/Hexagons/" + templatePath[0]);

    string[] grid = Regex.Split(sections[2],subdelimiters);
    foreach (string s in grid) Debug.Log(s);
    HexGrid g = new HexGrid(Convert.ToInt32(grid[0]), Convert.ToInt32(grid[1]));
    for (int i = 0; i < g.width * g.height; i++) {
      g.SetCel(i, Convert.ToInt32(grid[i + 2]));
    }

    m.Load(g, template); 
  }

}
