using UnityEngine;

[CreateAssetMenu(menuName = "Create SO/ShapeFactory", fileName = "ShapeFactory", order = 0)]
public class ShapeFactory : ScriptableObject
{
	[SerializeField] private Shape[] _shapes;

	public int Count => _shapes.Length;
	
	public Shape Create(int index)
	{
		var instance = Instantiate(_shapes[index]);
		Setup(instance);
		instance.InitID(index);
		return instance;
	}

	public Shape CreateRandom()
	{
		var randomIndex = Random.Range(0, _shapes.Length);
		var instance = Instantiate(_shapes[randomIndex]);
		Setup(instance);
		instance.InitID(randomIndex);
		return instance;
	}

	private void Setup(Shape instance)
	{
		instance.transform.localPosition = Random.insideUnitSphere * 5f;
		instance.transform.localRotation = Random.rotation;
		instance.transform.localScale = Vector3.one * Random.Range(0.1f, 1f);
	}
}