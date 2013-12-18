using UnityEngine;
using System;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class MapDrawer: MonoBehaviour {
#region Public Variables

  public int width = 0;
  public int height = 0;
  public int heightOffset = 16;
  public int celSize = 64;
  public Texture2D template;

#endregion
#region Private Variables

  private Color[][] cels;
  [SerializeField]
  private Map map;

#endregion
#region Accessors
  
  public int pixelWidth {get {return celSize * width + celSize / 2;}}
  public int pixelHeight {get {return (celSize - heightOffset) * (height - 1) + celSize;}}
  public int numCelTypes {get {return template.width / celSize;}}

#endregion
#region Unity Methods

  void OnEnable() {
    cels = new Color[numCelTypes][];
    for (int i = 0; i < numCelTypes; i++) {
      cels[i] = template.GetPixels(i * celSize, 0, celSize, celSize);
    }

    if (map == null) {
      map = ScriptableObject.CreateInstance<Map>();
      map.Init(width, height);

      this.gameObject.renderer.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
      this.gameObject.renderer.sharedMaterial.mainTexture = new Texture2D(pixelWidth, pixelHeight);
      GenerateTexture();
    }
  }

  void OnDisable() {
  }

	void Start () {
	}

#endregion
#region Public Methods

  public void ReApply() {
    map.ResizeMap(width, height);
  }

  public void GenerateTexture() {
    map.ResizeMap(width, height);

    Texture2D tex = new Texture2D(pixelWidth, pixelHeight);
    tex.Resize(pixelWidth, pixelHeight);
    tex.filterMode = FilterMode.Point;

    // Scale plane to be pixel perfect
    this.gameObject.transform.localScale = 
      new Vector3(pixelWidth * .001f, 1, pixelHeight * .001f);
    
    // Color texture properly
    Color[] cols = new Color[pixelWidth * pixelHeight];
    for (int i = 0; i < map.width; i++) {
      for (int j = 0; j < map.height; j++) {
        int x0 = (celSize / 2) * (j % 2) + i * celSize;
        int y0 = j * (celSize - heightOffset);
        int idx = map.Cel(i, j);
        for (int x = 0; x < celSize; x++) { 
          for (int y = 0; y < celSize; y++) {
            Color c = cels[idx][x + y * celSize];
            if (c.a != 0) {
              cols[(pixelWidth - 1 - x0 - x) + (y0 +  y) * pixelWidth] = c;
            }
          }
        }
      }
    }
    tex.SetPixels(cols);
    tex.Apply();
    this.gameObject.renderer.sharedMaterial.mainTexture = tex;
  }

  public HexCoord GetCoordFromTex(Vector2 TextureCoord) {
    float x = ((1 - TextureCoord.x) * pixelWidth) ;
    float y = (TextureCoord.y * pixelHeight) ;

    x = (x - celSize / 2) / celSize;
    float t1 = y / (celSize / 2);
    float t2 = Mathf.Floor(x + t1);
    float r = Mathf.Floor((Mathf.Floor(t1 - x) + t2) / 3);
    float q = Mathf.Floor((Mathf.Floor(2 * x + 1) + t2) / 3) - r;

    return new HexCoord((int) q, (int) r, true);
  }

  public void ColorCel(HexCoord idx, int val) {
    
    if (!map.SetCel(idx, val)) {
      return;
    }

    Texture2D tex = (Texture2D) this.gameObject.renderer.sharedMaterial.mainTexture;

    int x0 = (celSize / 2) * (idx.j % 2) + idx.i * celSize;
    int y0 = idx.j * (celSize - heightOffset);
    for (int x = 0; x < celSize; x++) { 
      for (int y = 0; y < celSize; y++) {
        Color c;
        if (val == numCelTypes - 1) {
          c = cels[0][x + y * celSize];
          if (c.a != 0) {
            tex.SetPixel(pixelWidth - 1 - x0 - x, (y0 + y) , Color.clear);
          }
        }
        else {
          c = cels[val][x + y * celSize];
          if (c.a != 0) {
            tex.SetPixel(pixelWidth - 1 - x0 - x, (y0 + y) , c);
          }
        }
      }
    }
    tex.Apply();
  }

#endregion
#region Private Methods

#endregion
}
