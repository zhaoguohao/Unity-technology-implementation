using UnityEngine;
using System.Collections;
using System;

public class FingerEvent : MonoBehaviour
{

    public static FingerEvent Instance;

    public enum FingerDir
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// 滑动委托
    /// </summary>
    public Action<FingerDir> OnFingerDrag;

    public enum ZoomType
    {
        In,
        Out
    }
    /// <summary>
    /// 缩放的委托
    /// </summary>
    public Action<ZoomType> OnZoom;
    private Vector2 m_TempFinger1Pos;
    private Vector2 m_TempFinger2Pos;

    private Vector2 m_OldFinger1Pos;
    private Vector2 m_OldFinger2Pos;

    /// <summary>
    /// 玩家点击地面的委托
    /// </summary>
    public Action OnPlayerClickGround;
    /// <summary>
    /// 拖动的旧位置
    /// </summary>
    private Vector2 m_OldDragPos;
    /// <summary>
    /// 拖动的方向
    /// </summary>
    private Vector2 m_DragDir;
    /// <summary>
    /// 交互参数
    /// </summary>
    private int m_PreFinger = -1;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        //启动时调用，这里开始注册手势操作的事件。

        //按下事件： OnFingerDown就是按下事件监听的方法，这个名子可以由你来自定义。方法只能在本类中监听。下面所有的事件都一样！！！
        FingerGestures.OnFingerDown += OnFingerDown;
        //开始拖动事件
        FingerGestures.OnFingerDragBegin += OnFingerDragBegin;
        //拖动中事件...
        FingerGestures.OnFingerDragMove += OnFingerDragMove;
        //拖动结束事件
        FingerGestures.OnFingerDragEnd += OnFingerDragEnd;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (OnZoom != null)
            {
                OnZoom(ZoomType.Out);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (OnZoom != null)
            {
                OnZoom(ZoomType.In);
            }
        }
    }

    void OnDisable()
    {
        //关闭时调用，这里销毁手势操作的事件
        //和上面一样
        FingerGestures.OnFingerDown -= OnFingerDown;
        FingerGestures.OnFingerDragBegin -= OnFingerDragBegin;
        FingerGestures.OnFingerDragMove -= OnFingerDragMove;
        FingerGestures.OnFingerDragEnd -= OnFingerDragEnd;
    }

    //按下时调用
    void OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        m_PreFinger = 1;
    }

    //开始滑动
    void OnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
    {
        m_PreFinger = 2;
        m_OldDragPos = fingerPos;
    }
    //滑动结束
    void OnFingerDragEnd(int fingerIndex, Vector2 fingerPos)
    {
        m_PreFinger = 4;
    }
    //滑动中
    void OnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
    {
        m_PreFinger = 3;
        m_DragDir = fingerPos - m_OldDragPos;
        if (m_DragDir.y < m_DragDir.x && m_DragDir.y > -m_DragDir.x)
        {
            //向右
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Right);
            }
        }
        else if (m_DragDir.y > m_DragDir.x && m_DragDir.y < -m_DragDir.x)
        {
            //向左
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Left);
            }
        }
        else if (m_DragDir.y > m_DragDir.x && m_DragDir.y > -m_DragDir.x)
        {
            //向上
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Up);
            }
        }
        else
        {
            //向下
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Down);
            }
        }
    }
}
