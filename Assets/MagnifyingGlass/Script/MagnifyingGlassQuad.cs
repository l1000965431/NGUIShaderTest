using UnityEngine;

public class MagnifyingGlassQuad : MonoBehaviour 
{
	private Camera m_MagnifyCamera;
	private Rect[] m_GUIRects = new Rect[3];
	private float m_QuadMagAmount = 1f;
	private float m_QuadMagWidth = Screen.width / 5f;
	private float m_QuadMagHeight = Screen.width / 5f;
	private float m_MouseX = 0f;
	private float m_MouseY = 0f;
	private bool  m_TraceMouse = false;

	void Start () 
	{
		m_GUIRects[0] = new Rect (95, 65, 115, 25);
		m_GUIRects[1] = new Rect (95, 95, 115, 25);
		m_GUIRects[2] = new Rect (95, 125, 115, 25);
		
		m_MouseX = Screen.width / 2f;
		m_MouseY = Screen.height / 2f;
		
		CreateMagnifyGlass ();
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
				if (CanMoveHorizontal ())
					m_MouseX = Input.mousePosition.x;
				if (CanMoveVertical ())
					m_MouseY = Input.mousePosition.y;
			}
		}
		m_MagnifyCamera.orthographicSize = m_QuadMagAmount;
		m_MagnifyCamera.pixelRect = new Rect (m_MouseX - m_QuadMagWidth / 2f, m_MouseY - m_QuadMagHeight / 2f, m_QuadMagWidth, m_QuadMagHeight);
		m_MagnifyCamera.transform.position = GetWorldPosition (new Vector3 (m_MouseX, m_MouseY, Input.mousePosition.z));
	}
	bool CanMoveHorizontal ()
	{
		if (m_MouseX < m_QuadMagWidth / 2 && Input.mousePosition.x < m_MouseX)
			return false;
		if (m_MouseX > Screen.width - m_QuadMagWidth / 2 && Input.mousePosition.x > m_MouseX)
			return false;
		return true;
	}
	bool CanMoveVertical ()
	{
		if (m_MouseY < m_QuadMagHeight / 2 && Input.mousePosition.y < m_MouseY)
			return false;
		if (m_MouseY > Screen.height - m_QuadMagHeight / 2 && Input.mousePosition.y > m_MouseY)
			return false;
		return true;
	}
	void CreateMagnifyGlass ()
	{
		GameObject go = new GameObject ("Magnify Camera");
		float x = Screen.width / 2f - m_QuadMagWidth / 2f;
		float y = Screen.height / 2f - m_QuadMagHeight / 2f; 
		m_MagnifyCamera = go.AddComponent<Camera> ();   
		m_MagnifyCamera.pixelRect = new Rect (x, y, m_QuadMagWidth, m_QuadMagHeight);
		m_MagnifyCamera.transform.position = new Vector3 (0, 0, 0);
		m_MagnifyCamera.clearFlags = Camera.main.clearFlags;
		m_MagnifyCamera.backgroundColor = Camera.main.backgroundColor;
		if (Camera.main.orthographic)
		{
			m_MagnifyCamera.orthographic = true;
			m_MagnifyCamera.orthographicSize = Camera.main.orthographicSize / 5.0f;
			m_QuadMagAmount = m_MagnifyCamera.orthographicSize;
		}
		else
		{
			m_MagnifyCamera.orthographic = false;
			m_MagnifyCamera.fieldOfView = Camera.main.fieldOfView / 10.0f;
		}
	}
	Vector3 GetWorldPosition (Vector3 screenPos)
	{
		Vector3 wldpos = Vector3.zero;
		if (Camera.main.orthographic)
		{
			wldpos = Camera.main.ScreenToWorldPoint (screenPos);
			wldpos.z = Camera.main.transform.position.z;
		}
		else
		{
			wldpos = Camera.main.ScreenToWorldPoint (new Vector3 (screenPos.x, screenPos.y, Camera.main.transform.position.z));
			wldpos.x *= -1;
			wldpos.y *= -1;
		}
		return wldpos;
	}
	void OnGUI ()
	{
		GUI.Box (new Rect (10, 10, 280, 25), "Magnifying Glass Demo --- Quad Glass");
		GUI.Box (new Rect (10, 50, 80, 25), "Amount");
		m_QuadMagAmount = GUI.HorizontalSlider (m_GUIRects[0], m_QuadMagAmount, 0.1f, 2f);
		GUI.Box (new Rect (10, 80, 80, 25), "Width");
		m_QuadMagWidth = GUI.HorizontalSlider (m_GUIRects[1], m_QuadMagWidth, 0f, Screen.width);
		GUI.Box (new Rect (10, 110, 80, 25), "Height");
		m_QuadMagHeight = GUI.HorizontalSlider (m_GUIRects[2], m_QuadMagHeight, 0f, Screen.height);		
	}
}
