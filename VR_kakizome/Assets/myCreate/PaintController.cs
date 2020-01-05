using UnityEngine;
using System.Collections;

public class PaintController : MonoBehaviour
 {

	[SerializeField] Transform HandAnchor;//positionを取得するコントローラーの位置情報
	
	// コントローラーを設定
	[SerializeField] OVRInput.Controller controller;

	private Transform Pointer
	{
	    get
	    {
		    return HandAnchor;
	    }
	}

	Texture2D drawTexture ;
	Color[] buffer;

	void Start () {
        	// OVRControllerHelperから左右どっちかを取得する
	      controller = GetComponent<OVRControllerHelper>().m_controller;

		Texture2D mainTexture = (Texture2D) GetComponent<Renderer>().material.mainTexture;
		Color[] pixels = mainTexture.GetPixels();

		buffer = new Color[pixels.Length];
		pixels.CopyTo(buffer, 0);

		drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
		drawTexture.filterMode = FilterMode.Point;
	}

	public void Draw(Vector2 p)
	{
		buffer.SetValue(Color.black, (int)p.x + 256 * (int)p.y);
	}

	void Update() 
	{
		var pointer = Pointer;
		if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger, controller)) 
		{
			Ray ray = Camera.main.ScreenPointToRay (pointer.position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100.0f)) {
				Draw (hit.textureCoord * 256);
			}

			drawTexture.SetPixels (buffer);
			drawTexture.Apply ();
			GetComponent<Renderer> ().material.mainTexture = drawTexture;
		}
	}
}
