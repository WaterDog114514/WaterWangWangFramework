using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ɫ��������  �����⹥���ж��������ж��������˺��ж���
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
