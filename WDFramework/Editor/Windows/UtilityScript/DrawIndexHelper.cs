using UnityEditor;
using UnityEngine;
/// <summary>
/// 在非Layout的编辑器绘制布局中，在指定Rect内采用自动布局的助手
/// </summary>
public class DrawIndexHelper
{
    //绘制控件的宽高
    public Vector2 DrawRectOriginPos;
    public Vector2 DrawRectSize;
    public float defaultSingleHeight => EditorGUIUtility.singleLineHeight;
    //上次绘制区域
    private Rect LastDrawRect;
    //是否开启横向布局
    public bool isHorizontal;
    //横向布局数量
    public int HorizontalCount;
    //横向布局时单控件宽度
    public float HorizontalSingleHeight;
    //横向布局计数索引
    public int HorizontalIndex;
    /// <summary>
    /// 更新绘制初始位置，需要在总绘制逻辑前调用
    /// </summary>
    public void Update(Rect DrawRect)
    {
        //重定位坐标
        DrawRectOriginPos = DrawRect.position;
        DrawRectSize = DrawRect.size;
        LastTotalHeight = 0;
        //重置横向布局
        isHorizontal = false;
        HorizontalIndex = 0;
        HorizontalCount = 0;
        HorizontalSingleHeight = 0;
    }
    //用来记录上次绘制的单行高度总和
    public float LastTotalHeight;
    /// <summary>
    /// 获取下个单行绘制区域
    /// </summary>
    public Rect GetNextSingleRect(float SingleHeight = 1.25f)
    {
        if (isHorizontal)
        {
            //横向排列中 单个控件的宽度
            var singleHorizontalWidth = DrawRectSize.x / HorizontalCount;
            //计算绘制位置 初始绘制位置+横向计数*单个控件宽度+绘制高度
            Vector2 DrawPosition = DrawRectOriginPos + Vector2.right * HorizontalIndex * singleHorizontalWidth + Vector2.up * LastTotalHeight;
            //计算绘制大小 
            Vector2 DrawSize = new Vector2(singleHorizontalWidth, HorizontalSingleHeight);
            //增加横向计数
            HorizontalIndex++;
            //记录上次绘制区域
            Rect rect = new Rect(DrawPosition, DrawSize);
            LastDrawRect = rect;
            return rect;
        }
        else
        {
            //获取单行绘制高度
            var DrawHeight = EditorGUIUtility.singleLineHeight * SingleHeight;
            //计算绘制位置 初始绘制位置+绘制高度
            Vector2 DrawPosition = DrawRectOriginPos + Vector2.up * LastTotalHeight;
            //计算绘制大小
            Vector2 DrawSize = new Vector2(DrawRectSize.x, DrawHeight);
            //累加上次绘制高度
            LastTotalHeight += DrawHeight;
            //记录上次绘制区域
            Rect rect = new Rect(DrawPosition, DrawSize);
            LastDrawRect = rect;
            return rect;
        }
    }
    public Vector2 printDemo => DrawRectOriginPos + Vector2.up * LastTotalHeight;
    //绘制行间间隙
    public void DrawHeightSpace(float spaceFactor)
    {
        GetNextSingleRect(spaceFactor);
    }
    /// <summary>
    /// 开始横向布局
    /// </summary>
    public void BeginHorizontalLayout(int count, float singleHeight = 1.25f)
    {
        //记录当前纵向的值
        isHorizontal = true;
        HorizontalCount = count;
        HorizontalIndex = 0;
        HorizontalSingleHeight = singleHeight * EditorGUIUtility.singleLineHeight;
    }
    /// <summary>
    /// 结束横向布局
    /// </summary>
    public void EndHorizontalLayout()
    {
        // 结束横向布局时清0
        isHorizontal = false;
        HorizontalIndex = 0;
        HorizontalCount = 0;
        //加一下总和
        LastTotalHeight += HorizontalSingleHeight;
        //结束横向清0
        HorizontalSingleHeight = 0;

    }
    /// <summary>
    /// 获取上一次绘制的区域
    /// </summary>
    /// <returns></returns>
    public Rect GetLastDrawRect() => LastDrawRect;
    /// <summary>
    /// 获取区域中间位置的Rect
    /// </summary>
    public Rect GetCenterRect(float width, float height)
    {
        float xPos = DrawRectOriginPos.x + (DrawRectSize.x - width) / 2;
        float yPos = DrawRectOriginPos.y + (DrawRectSize.y - height) / 2;
        return new Rect(xPos, yPos, width, height);
    }
}
