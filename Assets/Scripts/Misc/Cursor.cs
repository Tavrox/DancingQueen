using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {
	
	private OTSprite childSprite;
	private Ray ray;
	private RaycastHit hit;
	private Camera cam;
	
	// Use this for initialization
	void Start () 
	{
		childSprite = GetComponentInChildren<OTSprite>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(OT.view.mouseWorldPosition.x, OT.view.mouseWorldPosition.y, -1f);
		
		checkHover();
	}
	
	
	private void checkHover()
	{
		ray = cam.ScreenPointToRay(OT.view.mouseWorldPosition);
		if (Physics.Raycast(ray, out hit, 300f))
		{
			print ("Ray up");
			
			if (hit.collider.tag == "Character")
			{
				print ("character ray hit");
			}
		}
	}
}
