using UnityEngine;
/// <summary>
/// ��ܲ�ϵͳ
///�ṩ��Ϸ������ͨ�ù��ܣ���ֱ�Ӳ�����Ϸ�߼���
///ͨ��ȫ��Ψһ��������������Ϸ����һ��
/// </summary>
public interface IFrameworkSystem : ISystem
{
    /// <summary>
    /// ��ʼ��ϵͳ������Ϸ���̸�����ʱ�����
    /// </summary>
   void InitializedSystem();
}
