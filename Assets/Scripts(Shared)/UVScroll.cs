using UnityEngine;
using System.Collections;
 
public class UVScroll : MonoBehaviour {
    public float SpeedX;
    public float SpeedY;
    public float Direction;
    private Material material;
    private float deltX;
    private float deltY;
 
	void Start () {
        material = GetComponent<Renderer>().material;
	}
	
	void Update () {
        if (material)
        {
            deltX += SpeedX * Time.deltaTime * Direction;
            deltY += SpeedY * Time.deltaTime * Direction;
            material.SetTextureOffset("_MainTex", new Vector2(deltX, deltY));
        }
	}
}
