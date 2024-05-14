using System.Collections.Generic;


[System.Serializable]
public class MonsterPZContainer : DataBaseContainer
{
	public Dictionary<int, MonsterPZ> dataDic = new Dictionary<int, MonsterPZ>();
}