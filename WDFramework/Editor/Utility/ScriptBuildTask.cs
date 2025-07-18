using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �ű����������࣬��ʾһ�������ɵĽű��ļ�
/// ��װ�˽ű����������������Ϣ�Ͳ���
/// </summary>
public class ScriptBuildTask
{
    /// <summary> ���� </summary>
    public string ClassName { get; private set; }

    /// <summary> �̳еĻ����� </summary>
    public string InheritClassName { get; private set; }

    /// <summary> usingָ���б� </summary>
    public List<string> UsingDirectives { get; private set; }

    /// <summary> ��Ա�����б� </summary>
    public List<string> Members { get; private set; }

    /// <summary> �����б� </summary>
    public List<string> Methods { get; private set; }

    /// <summary> �������ݣ����Զ������ԡ�ע�͵ȣ� </summary>
    public string ExtraContent { get; private set; }

    /// <summary> �ű����·�� </summary>
    public string OutputPath { get; private set; }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="className">Ҫ����������</param>
    /// <param name="outputPath">�ű����·������ѡ��</param>
    public ScriptBuildTask(string className, string outputPath = null)
    {
        ClassName = className;
        OutputPath = outputPath;
        UsingDirectives = new List<string> { "using UnityEngine;" };
        Members = new List<string>();
        Methods = new List<string>();
        ExtraContent = string.Empty;
    }

    /// <summary>
    /// ���ü̳еĻ���
    /// </summary>
    /// <param name="baseClassName">��������</param>
    /// <returns>���ص�ǰ����ʵ����֧����ʽ����</returns>
    public ScriptBuildTask SetInheritance(string baseClassName)
    {
        InheritClassName = baseClassName;
        return this;
    }

    /// <summary>
    /// ���usingָ��
    /// </summary>
    /// <param name="usingDirective">usingָ���ַ���</param>
    /// <returns>���ص�ǰ����ʵ����֧����ʽ����</returns>
    public ScriptBuildTask AddUsing(string usingDirective)
    {
        if (!UsingDirectives.Contains(usingDirective))
        {
            UsingDirectives.Add(usingDirective);
        }
        return this;
    }

    /// <summary>
    /// ��ӳ�Ա����
    /// </summary>
    /// <param name="memberDeclaration">��Ա���������ַ���</param>
    /// <returns>���ص�ǰ����ʵ����֧����ʽ����</returns>
    public ScriptBuildTask AddMember(string memberDeclaration)
    {
        Members.Add(memberDeclaration);
        return this;
    }

    /// <summary>
    /// ��ӷ���
    /// </summary>
    /// <param name="methodDeclaration">���������ַ���</param>
    /// <returns>���ص�ǰ����ʵ����֧����ʽ����</returns>
    public ScriptBuildTask AddMethod(string methodDeclaration)
    {
        Methods.Add(methodDeclaration);
        return this;
    }

    /// <summary>
    /// ��Ӷ������ݣ����Զ������ԡ�ע�͵ȣ�
    /// </summary>
    /// <param name="content">Ҫ��ӵ�����</param>
    /// <returns>���ص�ǰ����ʵ����֧����ʽ����</returns>
    public ScriptBuildTask AddExtraContent(string content)
    {
        ExtraContent += content + "\n";
        return this;
    }
    public ScriptBuildTask SetOutpath(string path)
    {
        OutputPath = path;
        return this;
    }
    /// <summary>
    /// ͨ���ļ�����Ի����������·��
    /// </summary>
    /// <param name="title">�Ի������</param>
    /// <param name="defaultName">Ĭ���ļ���</param>
    /// <param name="extension">�ļ���չ��</param>
    /// <returns>���ص�ǰ����ʵ����֧����ʽ����</returns>
    public ScriptBuildTask SetOutputPathWithDialog(string title = "����ű�", string defaultName = "NewScript", string extension = "cs")
    {
        // ��ȡ��ĿAssets�ļ���·��
        string projectPath = Application.dataPath;
        string directory = Path.GetDirectoryName(projectPath);

        // ���������ļ��Ի���
        string path = EditorUtility.SaveFilePanel(title, directory, defaultName, extension);

        if (!string.IsNullOrEmpty(path))
        {
            // ȷ��·����AssetsĿ¼��
            if (path.StartsWith(Application.dataPath))
            {
                OutputPath = path;
            }
            else
            {
                Debug.LogWarning("�ű����뱣����AssetsĿ¼��");
                // �������AssetsĿ¼�£�����ת��Ϊ���·��
                string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
                if (path.StartsWith(Application.dataPath))
                {
                    OutputPath = relativePath;
                }
                else
                {
                    Debug.LogError("�޷���·��ת��ΪAssets�µ����·��");
                }
            }
        }
        return this;
    }

    /// <summary>
    /// ���ɽű�����
    /// </summary>
    /// <returns>�������ɵĽű��ַ���</returns>
    public string GenerateScriptContent()
    {
        StringBuilder content = new StringBuilder();

        // ���usingָ��
        foreach (var usingDirective in UsingDirectives)
        {
            content.AppendLine(usingDirective);
        }
        content.AppendLine();

        // ������
        if (string.IsNullOrEmpty(InheritClassName))
        {
            content.AppendLine($"public class {ClassName}");
        }
        else
        {
            content.AppendLine($"public class {ClassName} : {InheritClassName}");
        }
        content.AppendLine("{");

        // ��ӳ�Ա����
        foreach (var member in Members)
        {
            content.AppendLine($"    {member}");
        }

        // ������г�Ա�������з�������ӿ��зָ�
        if (Members.Count > 0 && Methods.Count > 0)
        {
            content.AppendLine();
        }

        // ��ӷ���
        foreach (var method in Methods)
        {
            content.AppendLine($"    {method}");
        }

        // ��Ӷ�������
        if (!string.IsNullOrEmpty(ExtraContent))
        {
            content.AppendLine();
            content.AppendLine($"    {ExtraContent}");
        }

        content.AppendLine("}");

        return content.ToString();
    }

    /// <summary>
    /// ִ�й����������ɽű��ļ�
    /// </summary>
    public void Execute()
    {
        if (string.IsNullOrEmpty(ClassName))
        {
            Debug.LogError("�ű�����ʧ�ܣ���������Ϊ��");
            return;
        }

        if (string.IsNullOrEmpty(OutputPath))
        {
            Debug.LogWarning("���·��Ϊ�գ�����ͨ���Ի�������·��");
            SetOutputPathWithDialog();

            if (string.IsNullOrEmpty(OutputPath))
            {
                Debug.LogError("�ű�����ʧ�ܣ����·������Ϊ��");
                return;
            }
        }

        string scriptContent = GenerateScriptContent();
        File.WriteAllText(OutputPath, scriptContent);
        AssetDatabase.Refresh();

        Debug.Log($"�ű����ɳɹ���{OutputPath}");
    }
}
