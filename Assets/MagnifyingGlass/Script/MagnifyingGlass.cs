using UnityEngine;

public class MagnifyingGlass : MonoBehaviour
{
	public Material m_Mat = null;
	public float m_InitialAmount = 0.5f;
	public float m_InitialRadiusX = 0.1f;
	public float m_InitialRadiusY = 0.1f;
	public float m_InitialComplicatedRadiusInner = 0.3f;
	public float m_InitialComplicatedRadiusOuter = 0.6f;
	
	private float[] m_Amount =  new float[8];
	private float[] m_RadiusX = new float[8];
	private float[] m_RadiusY = new float[8];
	private float[] m_RadiusInner = new float[8];
	private float[] m_RadiusOuter = new float[8];
	private float m_MouseX = 0f;
	private float m_MouseY = 0f;
	private bool m_TraceMouse = false;
	private Rect[] m_GUIRects = new Rect[9];
	private bool m_UseComplicated = false;
	private bool m_UseMultiple = false;
	private bool m_InvertScale = false;
	private int m_GlassIndex = 0;
	
	void ResetData ()
	{
		// reset array values
		for (int i = 0; i < 8; i++)
		{
			m_Amount[i] = m_InitialAmount;
			m_RadiusX[i] = m_InitialRadiusX;
			m_RadiusY[i] = m_InitialRadiusY;
			m_RadiusInner[i] = m_InitialComplicatedRadiusInner;
			m_RadiusOuter[i] = m_InitialComplicatedRadiusOuter;
		}
		// reset material parameters
		m_Mat.SetVector ("_SimpleCenterRadial1", new Vector4 (0.3f, 0.2f, m_RadiusX[0], m_RadiusY[0]));
		m_Mat.SetVector ("_SimpleCenterRadial2", new Vector4 (0.3f, 0.4f, m_RadiusX[1], m_RadiusY[1]));
		m_Mat.SetVector ("_SimpleCenterRadial3", new Vector4 (0.3f, 0.6f, m_RadiusX[2], m_RadiusY[2]));
		m_Mat.SetVector ("_SimpleCenterRadial4", new Vector4 (0.3f, 0.8f, m_RadiusX[3], m_RadiusY[3]));
		m_Mat.SetVector ("_SimpleCenterRadial5", new Vector4 (0.6f, 0.2f, m_RadiusX[4], m_RadiusY[4]));
		m_Mat.SetVector ("_SimpleCenterRadial6", new Vector4 (0.6f, 0.4f, m_RadiusX[5], m_RadiusY[5]));
		m_Mat.SetVector ("_SimpleCenterRadial7", new Vector4 (0.6f, 0.6f, m_RadiusX[6], m_RadiusY[6]));
		m_Mat.SetVector ("_SimpleCenterRadial8", new Vector4 (0.6f, 0.8f, m_RadiusX[7], m_RadiusY[7]));
		m_Mat.SetFloat ("_SimpleAmount1", m_Amount[0]);
		m_Mat.SetFloat ("_SimpleAmount2", m_Amount[1]);
		m_Mat.SetFloat ("_SimpleAmount3", m_Amount[2]);
		m_Mat.SetFloat ("_SimpleAmount4", m_Amount[3]);
		m_Mat.SetFloat ("_SimpleAmount5", m_Amount[4]);
		m_Mat.SetFloat ("_SimpleAmount6", m_Amount[5]);
		m_Mat.SetFloat ("_SimpleAmount7", m_Amount[6]);
		m_Mat.SetFloat ("_SimpleAmount8", m_Amount[7]);
		
		m_Mat.SetVector ("_ComplicatedCenterRadial1", new Vector4 (0.3f, 0.2f, m_RadiusX[0], m_RadiusY[0]));
		m_Mat.SetVector ("_ComplicatedCenterRadial2", new Vector4 (0.3f, 0.4f, m_RadiusX[1], m_RadiusY[1]));
		m_Mat.SetVector ("_ComplicatedCenterRadial3", new Vector4 (0.3f, 0.6f, m_RadiusX[2], m_RadiusY[2]));
		m_Mat.SetVector ("_ComplicatedCenterRadial4", new Vector4 (0.3f, 0.8f, m_RadiusX[3], m_RadiusY[3]));
		m_Mat.SetVector ("_ComplicatedCenterRadial5", new Vector4 (0.6f, 0.2f, m_RadiusX[4], m_RadiusY[4]));
		m_Mat.SetVector ("_ComplicatedCenterRadial6", new Vector4 (0.6f, 0.4f, m_RadiusX[5], m_RadiusY[5]));
		m_Mat.SetVector ("_ComplicatedCenterRadial7", new Vector4 (0.6f, 0.6f, m_RadiusX[6], m_RadiusY[6]));
		m_Mat.SetVector ("_ComplicatedCenterRadial8", new Vector4 (0.6f, 0.8f, m_RadiusX[7], m_RadiusY[7]));
		m_Mat.SetFloat ("_ComplicatedAmount1", m_Amount[0]);
		m_Mat.SetFloat ("_ComplicatedAmount2", m_Amount[1]);
		m_Mat.SetFloat ("_ComplicatedAmount3", m_Amount[2]);
		m_Mat.SetFloat ("_ComplicatedAmount4", m_Amount[3]);
		m_Mat.SetFloat ("_ComplicatedAmount5", m_Amount[4]);
		m_Mat.SetFloat ("_ComplicatedAmount6", m_Amount[5]);
		m_Mat.SetFloat ("_ComplicatedAmount7", m_Amount[6]);
		m_Mat.SetFloat ("_ComplicatedAmount8", m_Amount[7]);
		m_Mat.SetFloat ("_ComplicatedRadiusInner1", m_RadiusInner[0]);
		m_Mat.SetFloat ("_ComplicatedRadiusInner2", m_RadiusInner[1]);
		m_Mat.SetFloat ("_ComplicatedRadiusInner3", m_RadiusInner[2]);
		m_Mat.SetFloat ("_ComplicatedRadiusInner4", m_RadiusInner[3]);
		m_Mat.SetFloat ("_ComplicatedRadiusInner5", m_RadiusInner[4]);
		m_Mat.SetFloat ("_ComplicatedRadiusInner6", m_RadiusInner[5]);
		m_Mat.SetFloat ("_ComplicatedRadiusInner7", m_RadiusInner[6]);
		m_Mat.SetFloat ("_ComplicatedRadiusInner8", m_RadiusInner[7]);
		m_Mat.SetFloat ("_ComplicatedRadiusOuter1", m_RadiusOuter[0]);
		m_Mat.SetFloat ("_ComplicatedRadiusOuter2", m_RadiusOuter[1]);
		m_Mat.SetFloat ("_ComplicatedRadiusOuter3", m_RadiusOuter[2]);
		m_Mat.SetFloat ("_ComplicatedRadiusOuter4", m_RadiusOuter[3]);
		m_Mat.SetFloat ("_ComplicatedRadiusOuter5", m_RadiusOuter[4]);
		m_Mat.SetFloat ("_ComplicatedRadiusOuter6", m_RadiusOuter[5]);
		m_Mat.SetFloat ("_ComplicatedRadiusOuter7", m_RadiusOuter[6]);
		m_Mat.SetFloat ("_ComplicatedRadiusOuter8", m_RadiusOuter[7]);
	}
	void Start ()
	{
		if (!SystemInfo.supportsImageEffects)
			enabled = false;
		
		QualitySettings.antiAliasing = 8;
		
		ResetData ();
		m_GUIRects[0] = new Rect (10, 45, 150, 25);
		m_GUIRects[1] = new Rect (10, 70, 150, 25);
		m_GUIRects[2] = new Rect (10, 100, 200, 25);
		m_GUIRects[3] = new Rect (95, 145, 115, 25);
		m_GUIRects[4] = new Rect (95, 175, 115, 25);
		m_GUIRects[5] = new Rect (95, 205, 115, 25);
		m_GUIRects[6] = new Rect (95, 235, 115, 25);
		m_GUIRects[7] = new Rect (95, 265, 115, 25);
		m_GUIRects[8] = new Rect (10, 295, 150, 25);
		m_MouseX = m_MouseY = 0.5f;
	}
	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
	{
		// select which pass should we use
		int pass = 0;
		if (m_UseComplicated)
		{
			if (m_UseMultiple)
				pass = 3;
			else
				pass = 2;
		}
		else
		{
			if (m_UseMultiple)
				pass = 1;
			else
				pass = 0;
		}

		// fill material parameters
		int ind = m_GlassIndex;
		string simpleAmount = "_SimpleAmount" + (ind + 1);
		m_Mat.SetFloat (simpleAmount, m_Amount[ind]);
		string simpleCenterRadial = "_SimpleCenterRadial" + (ind + 1);
		m_Mat.SetVector (simpleCenterRadial, new Vector4 (m_MouseX, m_MouseY, m_RadiusX[ind], m_RadiusY[ind]));
		
		string complicatedAmount = "_ComplicatedAmount" + (ind + 1);
		m_Mat.SetFloat (complicatedAmount, m_Amount[ind]);
		string complicatedCenterRadial = "_ComplicatedCenterRadial" + (ind + 1);
		m_Mat.SetVector (complicatedCenterRadial, new Vector4 (m_MouseX, m_MouseY, m_RadiusX[ind], m_RadiusY[ind]));
		string complicatedRadiusInner = "_ComplicatedRadiusInner" + (ind + 1);
		m_Mat.SetFloat (complicatedRadiusInner, m_RadiusInner[ind]);
		string complicatedRadiusOuter = "_ComplicatedRadiusOuter" + (ind + 1);
		m_Mat.SetFloat (complicatedRadiusOuter, m_RadiusOuter[ind]);

		// let's draw it
		Graphics.Blit (sourceTexture, destTexture, m_Mat, pass);
	}
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			m_TraceMouse = true;
			
			for (int i = 0; i < m_GUIRects.Length; i++)
			{
				if (m_GUIRects[i].Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
				{
					m_TraceMouse = false;
					break;
				}
			}
		}
		else if (Input.GetMouseButtonUp (0))
		{
			m_TraceMouse = false;
		}
		else if (Input.GetMouseButton (0))
		{
			if (m_TraceMouse)
			{
				m_MouseX = Input.mousePosition.x / Screen.width;
				// unity anti-alias will flip Y coordinate of uv
//				if (QualitySettings.antiAliasing != 0)
//					m_MouseY = 1f - Input.mousePosition.y / Screen.height;
//				else
					m_MouseY = Input.mousePosition.y / Screen.height;
			}
		}
	}
	void OnGUI ()
	{
		GUI.Box (new Rect (10, 10, 280, 25), "Magnifying Glass Demo --- Circle Glass");

		bool previousFrameMultiple = m_UseMultiple;
		int previousFrameGlassIndex = m_GlassIndex;
		
		m_UseComplicated = GUI.Toggle (m_GUIRects[0], m_UseComplicated, " Use Complicated");
		m_UseMultiple = GUI.Toggle (m_GUIRects[1], m_UseMultiple, " Use Multiple");
		if (m_UseMultiple)
		{
			string[] names = { "1", "2", "3", "4", "5", "6", "7", "8" };
			m_GlassIndex = GUI.SelectionGrid (m_GUIRects[2], m_GlassIndex, names, 8);
		}
		// change multiple
		if (previousFrameMultiple != m_UseMultiple)
		{
			if (m_UseMultiple)   // on toggle enable event
			{
				ResetData ();
				m_MouseX = 0.3f;
				m_MouseY = 0.2f;
			}
			else   // on toggle disable event
			{
				m_GlassIndex = 0;
			}
		}
		// change glass index
		if (previousFrameGlassIndex != m_GlassIndex)
		{
			m_MouseX = m_MouseY = 0.5f;
		}
		
		GUI.Box (new Rect (10, 130, 80, 25), "Amount");
		m_Amount[m_GlassIndex] = GUI.HorizontalSlider (m_GUIRects[3], m_Amount[m_GlassIndex], 0f, 1f);
		GUI.Box (new Rect (10, 160, 80, 25), "Radial X");
		m_RadiusX[m_GlassIndex] = GUI.HorizontalSlider (m_GUIRects[4], m_RadiusX[m_GlassIndex], 0f, 0.7f);
		GUI.Box (new Rect (10, 190, 80, 25), "Radial Y");
		m_RadiusY[m_GlassIndex] = GUI.HorizontalSlider (m_GUIRects[5], m_RadiusY[m_GlassIndex], 0f, 0.7f);
		if (m_UseComplicated)
		{
			GUI.Box (new Rect (10, 220, 80, 25), "Inner");
			m_RadiusInner[m_GlassIndex] = GUI.HorizontalSlider (m_GUIRects[6], m_RadiusInner[m_GlassIndex], 0f, 1f);
			GUI.Box (new Rect (10, 250, 80, 25), "Outer");
			m_RadiusOuter[m_GlassIndex] = GUI.HorizontalSlider (m_GUIRects[7], m_RadiusOuter[m_GlassIndex], 0f, 1f);
		}
		m_InvertScale = GUI.Toggle (m_GUIRects[8], m_InvertScale, " Invert Scale");
		if (m_InvertScale)
			m_Amount[m_GlassIndex] = -m_Amount[m_GlassIndex];
	}
}