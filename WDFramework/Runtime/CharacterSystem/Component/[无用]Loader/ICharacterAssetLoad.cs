using UnityEngine;
/// <summary>
/// ��Դ���ؽӿڣ�����Դ�����������ü���
/// </summary>
public interface ICharacterAssetLoad : ICharacterOperator
{
    /// <summary>
    /// ���ؽ�ɫ��Դ
    /// </summary>
    /// <param name="asset"></param>
    public void I_LoadCharacterAsset(CharacterAsset asset);

}
