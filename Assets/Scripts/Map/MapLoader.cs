using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

public static class MapLoader {

  private static string subdelimiters = @",* +|\r+\n+";
  private static string baseDirectory = "Sprites/Map/";

  public static void LoadFromXML(Map m, string filename) {
    TextAsset mapfile = (TextAsset) Resources.Load("MapFiles/" + filename, typeof(TextAsset));
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.LoadXml(mapfile.text);
    string templatePath = xmlDoc.SelectSingleNode("map/template").InnerText;
    Texture2D template = Resources.Load<Texture2D>(baseDirectory + templatePath);
  
    XmlNodeList rows = xmlDoc.SelectNodes("map/grid/row");
    HexGrid g = new HexGrid(0, 0);
    int i = 0;
    foreach (XmlNode row in rows) {
      string[] rowText = Regex.Split(row.InnerText, subdelimiters);
      if (i == 0) {
        g = new HexGrid(rowText.Length, rows.Count);
      }
      foreach (string cel in rowText) {
        g.SetCel(i, Convert.ToInt32(cel));
        i += 1;
      }
    }
     
    m.Load(g, template); 

    XmlNodeList xmlentities = xmlDoc.SelectNodes("map/entities/entity");
    foreach (XmlNode e in xmlentities) {
      string[] pos = Regex.Split(e.SelectSingleNode("position").InnerText, subdelimiters);
      string[] stats = Regex.Split(e.SelectSingleNode("stats").InnerText, subdelimiters);
      string unitpath = e.SelectSingleNode("sprite").InnerText;
      bool friendly = Boolean.Parse(e.SelectSingleNode("friendly").InnerText);
      m.SpawnMapEntity(new HexCoord(pos), new Unit(unitpath, new Stats(stats)), friendly);
    }
  }
}
