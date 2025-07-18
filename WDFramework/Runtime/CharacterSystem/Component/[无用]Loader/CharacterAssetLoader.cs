using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色检测持有者  负责监测攻击判定，受伤判定，攻击伤害判定等
/// </summary>
public class CharacterAssetLoader : CharacterComponent
{

    private List<ICharacterAssetLoad> list_LoadComponent;
    public CharacterAssetLoader(BaseCharacter baseCharacter) : base(baseCharacter)
    {
        list_LoadComponent = baseCharacter.GetComponentInterfaces<ICharacterAssetLoad>();
    }

    public override void IntializeComponent()
    {

    }
    public override void UpdateComponent()
    {

    }
    public void I_LoadCharacterAsset(CharacterAsset asset)
    {
        throw new System.NotImplementedException();
    }


}
