using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 脚本生成类  为了每个模块都能使用，生成脚本必须使用此类
/// </summary>
public static class Tool_ScriptCreator
{
    /// <summary>
    /// 脚本信息
    /// </summary>
    public static ScriptInfo info;

    /// <summary>
    /// 生成脚本
    /// </summary>
    /// <param name="path">保存路径</param>
    /// <param name="ClassName">类名</param>
    /// <param name="InheritName">继承名</param>
    /// <param name="usingInfo">引用信息</param>
    /// <param name="Members">成员变量们</param>
    /// <param name="Methods">方法们</param>
    public static void m_CreateScript(string path, string ClassName, string InheritName = null, string usingInfo = null, List<ScriptFieldInfo> Members = null, List<ScriptMethodInfo> Methods = null)
    {
        //赋值
        info = new ScriptInfo();
        info.UsingInfo = usingInfo;
        info.ClassName = ClassName;
        info.InheritClassName = InheritName;
        info.MemberInfos = Members;
        info.MethodInfos = Methods;
        //  if (Methods != null)
        //  {
        //      info.MethodInfos = new ScriptFieldInfo[Methods.Count];
        //      for (int i = 0; i < Methods.Count; i++)
        //      {
        //          info.MethodInfos[i] = Methods[i];
        //      }
        //  }
        //  if (Members != null)
        //  {
        //      info.MemberInfos = new ScriptFieldInfo[Members.Count];
        //      for (int i = 0; i < Members.Count; i++)
        //      {
        //          info.MethodInfos[i] = Members[i];
        //      }
        //
        //  }
        m_CreateScript(info, path);
    }
    /// <summary>
    ///  生成脚本的真正方法
    /// </summary>
    public static void m_CreateScript(ScriptInfo info, string path)
    {
        StringBuilder content = new StringBuilder();

        //空三行
        if (info.ClassName == null) { 
            Debug.LogError("生成错误，类名为空，请检查！已停止生成"); 
            return;
        }

        //生成引用信息
        if(info.UsingInfo=="")
        content.AppendLine(DefaultUsingInfo);
        else content.AppendLine(info.UsingInfo);
        content.Append("\n\n\n");


        //写类名
        if (info.InheritClassName == null)
            content.AppendLine("public class " + info.ClassName);
        else
            content.AppendLine("public class " + info.ClassName + " : " + info.InheritClassName);
        //第一个大括号
        content.AppendLine("{");

        #region 写每行空成员和空方法
     
        //写每行成员字段
        if (info.MethodInfos != null)
        {
            //没写就跳过
            for (int i = 0; i < info.MemberInfos.Count; i++)
            {
                if (info.MemberInfos[i].FieldType == "" || info.MemberInfos[i].FieldName == "") { continue; }
                content.AppendLine("\t"+info.MemberInfos[i].GetTypeName() + " " + info.MemberInfos[i].FieldType + " " + info.MemberInfos[i].FieldName + ";");
            }
        }
        content.Append("\n");
        //写每行的方法
        if (info.MethodInfos != null)
        {
            for (int i = 0; i < info.MethodInfos.Count; i++)
            {
                //没写就跳过
                if (info.MethodInfos[i].FieldType == "" || info.MethodInfos[i].FieldName == "") continue;
                content.AppendLine("\t" + info.MethodInfos[i].GetTypeName() + " " + info.MethodInfos[i].FieldType + " " + info.MethodInfos[i].FieldName + $"({info.MethodInfos[i].ParameterInfo})");
                content.AppendLine("{\n}\n");
            }
        }
        #endregion
        //写附加内容
        if (info.ExtraContent != null)
        {
            content.AppendLine("\n");
            content.Append(info.ExtraContent);
        }

        //最后大括号
        content.AppendLine("\n\n}");

        //开始写入脚本
        if (path == null) path = AssetsPath;
        File.WriteAllText(path + "/" + info.ClassName + ".cs", content.ToString());
        //刷新一下 才能看到
        AssetDatabase.Refresh();
    }




    /// <summary>
    /// 必加引用
    /// </summary>
    private const string DefaultUsingInfo = "using UnityEngine;";
    /// <summary>
    /// 生成路径
    /// </summary>
    private static string Path;
    //固定路径
    public static string AssetsPath = Application.dataPath;
}



#region 生成代码的信息类们
//以下是代码必须信息
/// <summary>
/// 用于编辑器存储字段信息
/// </summary>
public class ScriptMemberInfo
{
    /// <summary>
    /// 访问修饰符
    /// </summary>
    public E_AccessModifiers Modifiers;
    public string FieldName;
    public string FieldType; 
    public string GetTypeName()
    {
        switch (Modifiers)
        {
            case E_AccessModifiers.Public:
                return "public";
            case E_AccessModifiers.Private:
                return "private";
            case E_AccessModifiers.Protected:
                return "protected";
        }
        return "public";
    }
}

[System.Serializable]
public class ScriptFieldInfo : ScriptMemberInfo
{

    
}
/// <summary>
/// 生成脚本方法信息
/// </summary>
[System.Serializable]

public class ScriptMethodInfo : ScriptMemberInfo
{
    /// <summary>
    /// 方法的参数信息，如果为字段可忽略此昂
    /// </summary>
    public string ParameterInfo;
}
public class ScriptInfo
{/// <summary>
/// 脚本引用信息
/// </summary>
    public string UsingInfo;
    public string ClassName;
    public string InheritClassName;
    /// <summary>
    /// 成员信息
    /// </summary>
    public List<ScriptFieldInfo> MemberInfos;
    /// <summary>
    /// 方法信息
    /// </summary>
    public List<ScriptMethodInfo> MethodInfos;
    /// <summary>
    /// 附加内容
    /// </summary>
    public string ExtraContent;
 }

/// <summary>
/// 访问修饰符
/// </summary>
public enum E_AccessModifiers
{
    Public,
    Private,
    Protected
}

#endregion
