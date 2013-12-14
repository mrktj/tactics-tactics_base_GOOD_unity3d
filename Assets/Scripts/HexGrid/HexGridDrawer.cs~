using UnityEngine;
using System.Collections;

public class HexGridDrawer : MonoBehaviour {
#region Public Variables

  public int width;
  public int height;
  public int heightOffset;
  public int spriteSize = 64;
  public Texture2D template;

#endregion
#region Private Variables

  private Texture2D tex;
  private HexGrid grid;
  private Color[][] cels;
  private int pixelWidth;
  private int pixelHeight;

#endregion
#region Unity Methods

	// Use this for initialization
	void Start () {

    pixelWidth = spriteSize * width + spriteSize / 2;
    pixelHeight = (spriteSize - heightOffset) * (height - 1) + spriteSize;

    this.gameObject.transform.localScale = 
      new Vector3(pixelWidth * .001f, 1, pixelHeight * .001f);

    grid = new HexGrid(width, height);

    tex = new Texture2D(pixelWidth, pixelHeight);
    tex.filterMode = FilterMode.Point;

    GenerateTexture();
	}
	
	// Update is called once per frame
	void Update () {
	}

#endregion
#region Public Methods

  public void GenerateTexture() {
    int numCels = template.width / spriteSize;
    cels = new Color[numCels][];
    for (int i = 0; i < numCels; i++) {
      cels[i] = template.GetPixels(i * spriteSize, 0, spriteSize, spriteSize);
    }

    Color[] cols = new Color[pixelWidth * pixelHeight];
    for (int i = 0; i < grid.width; i++) {
      for (int j = 0; j < grid.height; j++) {
        int x0 = (spriteSize / 2) * (j % 2) + i * spriteSize;
        int y0 = j * (spriteSize - heightOffset);
        int idx = grid.Cel(i, j);
        for (int x = 0; x < spriteSize; x++) { 
          for (int y = 0; y < spriteSize; y++) {
            Color c = cels[idx][x + y * spriteSize];
            if (c.a != 0) {
              cols[(pixelWidth - 1 - x0 - x) + (y0 +  y) * pixelWidth] = c;
            }
          }
        }
      }
    }
    tex.SetPixels(cols);
    tex.Apply();
    this.gameObject.renderer.material.mainTexture = tex;
  }

#endregion
#region Private Methods

#endregion
}
