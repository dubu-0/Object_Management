public interface IPersistableObject
{
	public void Save(GameDataWriter writer);
	public void Load(GameDataReader reader);
	public void Destroy(float time = 0);
}