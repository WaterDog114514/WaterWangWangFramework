using System.Collections.Generic;

using System;

/// <summary>
/// 动态枚举基类 - 支持无限扩展的字符串枚举
/// </summary>
/// <typeparam name="T">实际枚举类型</typeparam>
/// 

//enumValue的获取
/*
   1. 获取已有值（可能为null）
    var eventValue = GameEvent.Get("PlayerDeath");
   2. 定义枚举类时预定义值
    public class GameEvent : DynamicEnum<GameEvent> {
    public static readonly GameEvent PlayerDeath = new("PlayerDeath");
    public static readonly GameEvent LevelComplete = new("LevelComplete");
}

 */
public abstract class DynamicEnum<T> where T : DynamicEnum<T>
{
    private static readonly Dictionary<string, T> _values = new(StringComparer.OrdinalIgnoreCase);

    public string Name { get;private set; }

    // 保护构造函数
    protected DynamicEnum(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("枚举名称不能为空");

        Name = name;

        // 自动注册
        if (!_values.ContainsKey(name))
            _values[name] = (T)this;
    }

    // 静态点出支持
    public static T Get(string name) => _values.TryGetValue(name, out var value) ? value : null;

    // 动态添加新值
    public static T Add(string name)
    {
        if (_values.ContainsKey(name))
            throw new ArgumentException($"'{name}' 已存在");

        // 通过反射创建实例
        var instance = Activator.CreateInstance(typeof(T), name) as T;
        return instance;
    }

    // 获取所有值
    public static IEnumerable<T> GetAll() => _values.Values;

    public override string ToString() => Name;
}