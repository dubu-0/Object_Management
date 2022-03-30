using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Create SO/ShapeFactory", fileName = "ShapeFactory", order = 0)]
public class ShapeFactory : ScriptableObject
{
	[SerializeField] private Shape[] _shapes;
	[SerializeField] private Material[] _materials;

	public int Count => _shapes.Length;

	public Shape Create(Color color, int id = 0, int materialID = 0)
	{
		var instance = Instantiate(_shapes[id]);
		Setup(instance, id, materialID, color);
		return instance;
	}

	public Shape CreateRandom()
	{
		var randomID = Random.Range(0, _shapes.Length);
		var randomMaterialID = Random.Range(0, _materials.Length);
		var randomColor = Random.ColorHSV(
			0f, 1f, 
			0.5f, 1f,
			0.25f, 1f, 
			1f, 1f);
		
		var instance = Instantiate(_shapes[randomID]);
		Setup(instance, randomID, randomMaterialID, randomColor);
		return instance;
	}

	private void Setup(Shape instance, int newID, int newMaterialID, Color color)
	{
		instance.transform.localPosition = Random.insideUnitSphere * 5f;
		instance.transform.localRotation = Random.rotation;
		instance.transform.localScale = Vector3.one * Random.Range(0.1f, 1f);
		instance.InitID(newID);
		instance.InitMaterialID(newMaterialID);
		instance.SetNewMaterial(_materials[newMaterialID]);
		instance.SetNewColor(color);
	}
}