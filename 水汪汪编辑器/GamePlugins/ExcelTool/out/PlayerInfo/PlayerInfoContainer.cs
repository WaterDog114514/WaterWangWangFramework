using System.Collections.Generic;


[System.Serializable]
public class PlayerInfoContainer : DataBaseContainer
{
	public Dictionary<int, PlayerInfo> dataDic = new Dictionary<int, PlayerInfo>();
}