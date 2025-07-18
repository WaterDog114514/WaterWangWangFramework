using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �Զ�����ʽ�ļ̳�Mono�ĵ���ģʽ����
/// "�����ֶ�����|���趯̬���|��������г�������������"
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //��̬���� ��̬����
                //�ڳ����ϴ���������
                GameObject obj = new GameObject();
                //�õ�T�ű������� Ϊ������� �����ٱ༭���п�����ȷ�Ŀ�����
                //����ģʽ�ű�����������GameObject
                obj.name = typeof(T).Name.ToString();
                //��̬���ض�Ӧ�� ����ģʽ�ű�
                instance = obj.AddComponent<T>();
                //������ʱ���Ƴ����� ��֤����������Ϸ���������ж�����
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

}
