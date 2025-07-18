using UnityEngine;
/// <summary>
/// 资源加载接口，由资源加载类来调用加载
/// </summary>
public interface ICharacterAssetLoad : ICharacterOperator
{
    /// <summary>
    /// 加载角色资源
    /// </summary>
    /// <param name="asset"></param>
    public void I_LoadCharacterAsset(CharacterAsset asset);

}
