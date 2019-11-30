using UnityEngine;

public class MagnifyingGlassVolumetricSphere : MonoBehaviour
{
	public Material m_Mat = null;
	[Range(0.1f, 2f)] public float m_Radius = 1f;
	[Range(1f, 3f)] public float m_Distortion = 2f;
	[Range(0.05f, 0.3f)] public float m_Form = 0.2f;
	private float m_MouseX = 0f;
	private float m_MouseY = 0f;
	private bool m_TraceMouse = false;
	
	private void Start ()
	{
		if (!SystemInfo.supportsImageEffects)
			enabled = false;
		m_MouseX = m_MouseY = 0.5f;
	}
	private void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
	{
		m_Mat.SetVector ("_Center", new Vector4 (m_MouseX, m_MouseY, 0f, 0f));
		m_Mat.SetFloat ("_Radius", m_Radius);
		m_Mat.SetFloat ("_Distortion", m_Distortion);
		m_Mat.SetFloat ("_Form", m_Form);
		Graphics.Blit (sourceTexture, destTexture, m_Mat);
	}
	private void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			m_TraceMouse = true;
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
//				if (QualitySettings.antiAliasing != 0)
//					m_MouseY = 1f - Input.mousePosition.y / Screen.height;
//				else
					m_MouseY = Input.mousePosition.y / Screen.height;
			}
		}
	}
	private void OnGUI ()
	{
		GUI.Box (new Rect (10, 10, 300, 25), "Magnifying Glass Demo --- Volumetric Sphere");
	}
}