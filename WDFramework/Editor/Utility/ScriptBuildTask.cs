using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 脚本构建任务类，表示一个待生成的脚本文件
/// 封装了脚本生成所需的所有信息和操作
/// </summary>
public class ScriptBuildTask
{
    /// <summary> 类名 </summary>
    public string ClassName { get; private set; }

    /// <summary> 继承的基类名 </summary>
    public string InheritClassName { get; private set; }

    /// <summary> using指令列表 </summary>
    public List<string> UsingDirectives { get; private set; }

    /// <summary> 成员变量列表 </summary>
    public List<string> Members { get; private set; }

    /// <summary> 方法列表 </summary>
    public List<string> Methods { get; private set; }

    /// <summary> 额外内容（如自定义属性、注释等） </summary>
    public string ExtraContent { get; private set; }

    /// <summary> 脚本输出路径 </summary>
    public string OutputPath { get; private set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="className">要创建的类名</param>
    /// <param name="outputPath">脚本输出路径（可选）</param>
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
    /// 设置继承的基类
    /// </summary>
    /// <param name="baseClassName">基类名称</param>
    /// <returns>返回当前任务实例以支持链式调用</returns>
    public ScriptBuildTask SetInheritance(string baseClassName)
    {
        InheritClassName = baseClassName;
        return this;
    }

    /// <summary>
    /// 添加using指令
    /// </summary>
    /// <param name="usingDirective">using指令字符串</param>
    /// <returns>返回当前任务实例以支持链式调用</returns>
    public ScriptBuildTask AddUsing(string usingDirective)
    {
        if (!UsingDirectives.Contains(usingDirective))
        {
            UsingDirectives.Add(usingDirective);
        }
        return this;
    }

    /// <summary>
    /// 添加成员变量
    /// </summary>
    /// <param name="memberDeclaration">成员变量声明字符串</param>
    /// <returns>返回当前任务实例以支持链式调用</returns>
    public ScriptBuildTask AddMember(string memberDeclaration)
    {
        Members.Add(memberDeclaration);
        return this;
    }

    /// <summary>
    /// 添加方法
    /// </summary>
    /// <param name="methodDeclaration">方法声明字符串</param>
    /// <returns>返回当前任务实例以支持链式调用</returns>
    public ScriptBuildTask AddMethod(string methodDeclaration)
    {
        Methods.Add(methodDeclaration);
        return this;
    }

    /// <summary>
    /// 添加额外内容（如自定义属性、注释等）
    /// </summary>
    /// <param name="content">要添加的内容</param>
    /// <returns>返回当前任务实例以支持链式调用</returns>
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
    /// 通过文件保存对话框设置输出路径
    /// </summary>
    /// <param name="title">对话框标题</param>
    /// <param name="defaultName">默认文件名</param>
    /// <param name="extension">文件扩展名</param>
    /// <returns>返回当前任务实例以支持链式调用</returns>
    public ScriptBuildTask SetOutputPathWithDialog(string title = "保存脚本", string defaultName = "NewScript", string extension = "cs")
    {
        // 获取项目Assets文件夹路径
        string projectPath = Application.dataPath;
        string directory = Path.GetDirectoryName(projectPath);

        // 弹出保存文件对话框
        string path = EditorUtility.SaveFilePanel(title, directory, defaultName, extension);

        if (!string.IsNullOrEmpty(path))
        {
            // 确保路径在Assets目录下
            if (path.StartsWith(Application.dataPath))
            {
                OutputPath = path;
            }
            else
            {
                Debug.LogWarning("脚本必须保存在Assets目录下");
                // 如果不在Assets目录下，尝试转换为相对路径
                string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
                if (path.StartsWith(Application.dataPath))
                {
                    OutputPath = relativePath;
                }
                else
                {
                    Debug.LogError("无法将路径转换为Assets下的相对路径");
                }
            }
        }
        return this;
    }

    /// <summary>
    /// 生成脚本内容
    /// </summary>
    /// <returns>返回生成的脚本字符串</returns>
    public string GenerateScriptContent()
    {
        StringBuilder content = new StringBuilder();

        // 添加using指令
        foreach (var usingDirective in UsingDirectives)
        {
            content.AppendLine(usingDirective);
        }
        content.AppendLine();

        // 类声明
        if (string.IsNullOrEmpty(InheritClassName))
        {
            content.AppendLine($"public class {ClassName}");
        }
        else
        {
            content.AppendLine($"public class {ClassName} : {InheritClassName}");
        }
        content.AppendLine("{");

        // 添加成员变量
        foreach (var member in Members)
        {
            content.AppendLine($"    {member}");
        }

        // 如果既有成员变量又有方法，添加空行分隔
        if (Members.Count > 0 && Methods.Count > 0)
        {
            content.AppendLine();
        }

        // 添加方法
        foreach (var method in Methods)
        {
            content.AppendLine($"    {method}");
        }

        // 添加额外内容
        if (!string.IsNullOrEmpty(ExtraContent))
        {
            content.AppendLine();
            content.AppendLine($"    {ExtraContent}");
        }

        content.AppendLine("}");

        return content.ToString();
    }

    /// <summary>
    /// 执行构建任务，生成脚本文件
    /// </summary>
    public void Execute()
    {
        if (string.IsNullOrEmpty(ClassName))
        {
            Debug.LogError("脚本生成失败：类名不能为空");
            return;
        }

        if (string.IsNullOrEmpty(OutputPath))
        {
            Debug.LogWarning("输出路径为空，尝试通过对话框设置路径");
            SetOutputPathWithDialog();

            if (string.IsNullOrEmpty(OutputPath))
            {
                Debug.LogError("脚本生成失败：输出路径不能为空");
                return;
            }
        }

        string scriptContent = GenerateScriptContent();
        File.WriteAllText(OutputPath, scriptContent);
        AssetDatabase.Refresh();

        Debug.Log($"脚本生成成功：{OutputPath}");
    }
}
