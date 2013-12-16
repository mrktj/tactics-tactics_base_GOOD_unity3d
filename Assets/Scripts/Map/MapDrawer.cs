using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class MapDrawer: MonoBehaviour {
#region Public Variables

  public Map map;

#endregion
#region Unity Methods

  void OnEnable() {
    map.OnMapUpdate += () => GenerateTexture();
  }

  void OnDisable() {
    map.OnMapUpdate -= () => GenerateTexture();
  }

	void Start () {
    this.gameObject.renderer.material = new Material(Shader.Find("Unlit/Transparent"));
	}

#endregion
#region Public Methods

  public void GenerateTexture() {
    map.plane = this.gameObject;

    int width = map.width;
    int height = map.height;
    int heightOffset = map.pixelHeightOffset;
    int celSize = map.celSize;

    // Compute size of texture in pixels
    int pixelWidth = celSize * width + celSize / 2;
    int pixelHeight = (celSize - heightOffset) * (height - 1) + celSize;

    // Create texture
    Texture2D tex = new Texture2D (pixelWidth, pixelHeight);
    tex.filterMode = FilterMode.Point;
    

    // Scale plane to be pixel perfect
    this.gameObject.transform.localScale = 
      new Vector3(pixelWidth * .001f, 1, pixelHeight * .001f);
    
    // Cache Hex cel sprites
    int numCels = map.template.width / celSize;
    Color[][] cels = new Color[numCels][];
    for (int i = 0; i < numCels; i++) {
      cels[i] = map.template.GetPixels(i * celSize, 0, celSize, celSize);
    }
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

    // Update plane's texture
    this.gameObject.renderer.sharedMaterial.mainTexture = tex;
  }

#endregion
#region Private Methods

#endregion
}
