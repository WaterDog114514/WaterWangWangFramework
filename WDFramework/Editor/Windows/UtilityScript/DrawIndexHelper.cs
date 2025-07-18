using UnityEditor;
using UnityEngine;
/// <summary>
/// �ڷ�Layout�ı༭�����Ʋ����У���ָ��Rect�ڲ����Զ����ֵ�����
/// </summary>
public class DrawIndexHelper
{
    //���ƿؼ��Ŀ��
    public Vector2 DrawRectOriginPos;
    public Vector2 DrawRectSize;
    public float defaultSingleHeight => EditorGUIUtility.singleLineHeight;
    //�ϴλ�������
    private Rect LastDrawRect;
    //�Ƿ������򲼾�
    public bool isHorizontal;
    //���򲼾�����
    public int HorizontalCount;
    //���򲼾�ʱ���ؼ����
    public float HorizontalSingleHeight;
    //���򲼾ּ�������
    public int HorizontalIndex;
    /// <summary>
    /// ���»��Ƴ�ʼλ�ã���Ҫ���ܻ����߼�ǰ����
    /// </summary>
    public void Update(Rect DrawRect)
    {
        //�ض�λ����
        DrawRectOriginPos = DrawRect.position;
        DrawRectSize = DrawRect.size;
        LastTotalHeight = 0;
        //���ú��򲼾�
        isHorizontal = false;
        HorizontalIndex = 0;
        HorizontalCount = 0;
        HorizontalSingleHeight = 0;
    }
    //������¼�ϴλ��Ƶĵ��и߶��ܺ�
    public float LastTotalHeight;
    /// <summary>
    /// ��ȡ�¸����л�������
    /// </summary>
    public Rect GetNextSingleRect(float SingleHeight = 1.25f)
    {
        if (isHorizontal)
        {
            //���������� �����ؼ��Ŀ��
            var singleHorizontalWidth = DrawRectSize.x / HorizontalCount;
            //�������λ�� ��ʼ����λ��+�������*�����ؼ����+���Ƹ߶�
            Vector2 DrawPosition = DrawRectOriginPos + Vector2.right * HorizontalIndex * singleHorizontalWidth + Vector2.up * LastTotalHeight;
            //������ƴ�С 
            Vector2 DrawSize = new Vector2(singleHorizontalWidth, HorizontalSingleHeight);
            //���Ӻ������
            HorizontalIndex++;
            //��¼�ϴλ�������
            Rect rect = new Rect(DrawPosition, DrawSize);
            LastDrawRect = rect;
            return rect;
        }
        else
        {
            //��ȡ���л��Ƹ߶�
            var DrawHeight = EditorGUIUtility.singleLineHeight * SingleHeight;
            //�������λ�� ��ʼ����λ��+���Ƹ߶�
            Vector2 DrawPosition = DrawRectOriginPos + Vector2.up * LastTotalHeight;
            //������ƴ�С
            Vector2 DrawSize = new Vector2(DrawRectSize.x, DrawHeight);
            //�ۼ��ϴλ��Ƹ߶�
            LastTotalHeight += DrawHeight;
            //��¼�ϴλ�������
            Rect rect = new Rect(DrawPosition, DrawSize);
            LastDrawRect = rect;
            return rect;
        }
    }
    public Vector2 printDemo => DrawRectOriginPos + Vector2.up * LastTotalHeight;
    //�����м��϶
    public void DrawHeightSpace(float spaceFactor)
    {
        GetNextSingleRect(spaceFactor);
    }
    /// <summary>
    /// ��ʼ���򲼾�
    /// </summary>
    public void BeginHorizontalLayout(int count, float singleHeight = 1.25f)
    {
        //��¼��ǰ�����ֵ
        isHorizontal = true;
        HorizontalCount = count;
        HorizontalIndex = 0;
        HorizontalSingleHeight = singleHeight * EditorGUIUtility.singleLineHeight;
    }
    /// <summary>
    /// �������򲼾�
    /// </summary>
    public void EndHorizontalLayout()
    {
        // �������򲼾�ʱ��0
        isHorizontal = false;
        HorizontalIndex = 0;
        HorizontalCount = 0;
        //��һ���ܺ�
        LastTotalHeight += HorizontalSingleHeight;
        //����������0
        HorizontalSingleHeight = 0;

    }
    /// <summary>
    /// ��ȡ��һ�λ��Ƶ�����
    /// </summary>
    /// <returns></returns>
    public Rect GetLastDrawRect() => LastDrawRect;
    /// <summary>
    /// ��ȡ�����м�λ�õ�Rect
    /// </summary>
    public Rect GetCenterRect(float width, float height)
    {
        float xPos = DrawRectOriginPos.x + (DrawRectSize.x - width) / 2;
        float yPos = DrawRectOriginPos.y + (DrawRectSize.y - height) / 2;
        return new Rect(xPos, yPos, width, height);
    }
}
