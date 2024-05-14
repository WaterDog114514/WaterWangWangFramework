using System.Collections.Generic;


[System.Serializable]
public class TestInfoContainer : DataBaseContainer
{
	public Dictionary<int, TestInfo> dataDic = new Dictionary<int, TestInfo>();
}