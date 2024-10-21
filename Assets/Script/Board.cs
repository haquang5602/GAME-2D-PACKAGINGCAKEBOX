using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Board : MonoBehaviour
{   
    public List<List<Transform>> boards=new();

    public enum Object
    {
        NONE,
        CAKE,
        BOX,
        CANDY,
        COUNT,
    }

    private bool IsWin;
    public List<List<Object>> objs=new();
    public Transform box;
    public Transform cake;
    
    public pos posOfCake;

    public pos posOfBox;

    // Start is called before the first frame update
    public static Board Instance;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        IsWin = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform r = transform.GetChild(i);
            List<Transform> row = new List<Transform>();
            List<Object> rowObj = new List<Object>();
            for (int j = 0; j < r.childCount; j++)
            {
                Transform cell = r.GetChild(j);
                row.Add(cell);
                if (cell.childCount > 0)
                {
                    Transform obj = cell.GetChild(0);
                    if (obj.name == "CAKE")
                    {
                        rowObj.Add(Object.CAKE);
                        posOfCake = new pos(i, j);
                    }
                    else if (obj.name == "BOX")
                    {
                        rowObj.Add(Object.BOX);
                        posOfBox = new pos(i, j);
                    }
                    else if (obj.name == "CANDY")
                    {
                        rowObj.Add(Object.CANDY);
                    }
                }
                else
                {
                    rowObj.Add(Object.NONE);
                }
            }

            objs.Add(rowObj);
            boards.Add(row);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Xử lý khi ngón tay bắt đầu chạm vào màn hình
                    break;

                case TouchPhase.Moved:
                    // Xử lý khi ngón tay di chuyển trên màn hình
                    Vector2 deltaPosition = touch.deltaPosition;

                    // Xác định hướng di chuyển
                    if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
                    {
                        // Ngón tay di chuyển theo chiều ngang (phải hoặc trái)
                        if (deltaPosition.x > 0)
                        {
                            // Ngón tay di chuyển về phải
                            Move(Vector2.right);
                        }
                        else
                        {
                            // Ngón tay di chuyển về trái
                            Move(Vector2.left);
                        }
                    }
                    else
                    {
                        // Ngón tay di chuyển theo chiều dọc (trên hoặc dưới)
                        if (deltaPosition.y > 0)
                        {
                            // Ngón tay di chuyển lên trên
                            Move(Vector2.up);
                        }
                        else
                        {
                            // Ngón tay di chuyển xuống dưới
                            Move(Vector2.down);
                        }
                    }

                    break;

                case TouchPhase.Ended:
                    // Xử lý khi ngón tay kết thúc chạm vào màn hình
                    break;
            }
        }
    }

    public void Move(Vector2 dir)
    {
        if (posOfBox.X * dir.x + posOfBox.Y * dir.y < posOfCake.X * dir.x + posOfCake.Y * dir.y)
        {
            MoveBox(dir);
            MoveCake(dir);
        }
        else
        {
            MoveCake(dir);
            MoveBox(dir);
        }

        if ((dir == Vector2.down||dir == Vector2.up)  && (posOfCake.X == (posOfBox.X -1)) && posOfCake.Y == posOfBox.Y)
        {   cake.gameObject.SetActive(false);
            Debug.Log("Victory");
            IsWin = true;
            Debug.Log(IsWin);
        }
        
    }

    public void MoveBox(Vector2 dir)
    {
        if (dir == Vector2.up)
        {
            while (posOfBox.X > 0)
            {
                int x = posOfBox.X - 1;
                if (objs[x][posOfBox.Y] == Object.NONE)
                {
                    objs[posOfBox.X][posOfBox.Y] = Object.NONE;
                    objs[x][posOfBox.Y] = Object.BOX;
                    posOfBox.X = x;
                    box.position = boards[posOfBox.X][posOfBox.Y].position;
                }
                else
                {   Debug.Log(objs[posOfBox.X][posOfBox.Y]);
                    break;
                }

            }
        }
        else if (dir == Vector2.down)
        {
            while (posOfBox.X < objs.Count - 1)
            {
                int x = posOfBox.X + 1;
                if (objs[x][posOfBox.Y] == Object.NONE)
                {    objs[posOfBox.X][posOfBox.Y] = Object.NONE;
                    objs[x][posOfBox.Y] = Object.BOX;
                    posOfBox.X = x;
                    box.position = boards[posOfBox.X][posOfBox.Y].position;
                }
                else {   Debug.Log(objs[posOfBox.X][posOfBox.Y]);
                    break;
                }

            }

        }
        else if (dir == Vector2.left)
        {
            while (posOfBox.Y > 0)
            {
                int y = posOfBox.Y - 1;
                if (objs[posOfBox.X][y] == Object.NONE)
                {
                    objs[posOfBox.X][y] = Object.BOX;
                    objs[posOfBox.X][posOfBox.Y] = Object.NONE;
                    posOfBox.Y = y;
                    box.position = boards[posOfBox.X][posOfBox.Y].position;
                }
                else {   Debug.Log(objs[posOfBox.X][posOfBox.Y]);
                    break;
                }
            }
        }

        else if (dir == Vector2.right)
        {
            while (posOfBox.Y < objs.Count - 1)
            {
                int y = posOfBox.Y + 1;
                if (objs[posOfBox.X][y] == Object.NONE)
                {   objs[posOfBox.X][y] = Object.BOX;
                    objs[posOfBox.X][posOfBox.Y] = Object.NONE;
                    posOfBox.Y = y;
                    box.position = boards[posOfBox.X][posOfBox.Y].position;
                }
                else {   Debug.Log(objs[posOfBox.X][posOfBox.Y]);
                    break;
                }

            }

        }
    }

    public bool GetIsWin()
    {
        return IsWin;
    }

    public void MoveCake(Vector2 dir)
    {
        if (dir == Vector2.up)
        {
            while (posOfCake.X > 0)
            {
                int x = posOfCake.X - 1;
                if (objs[x][posOfCake.Y] == Object.NONE)
                {
                    objs[x][posOfCake.Y] = Object.CAKE;
                    objs[posOfCake.X][posOfCake.Y] =Object.NONE;
                    posOfCake.X = x;
                    cake.position = boards[posOfCake.X][posOfCake.Y].position;
                }
                else {   Debug.Log(objs[posOfCake.X][posOfCake.Y]);
                    break;
                }

            }
        }
        else if (dir == Vector2.down)
        {
            while (posOfCake.X < objs.Count - 1)
            {
                int x = posOfCake.X + 1;
                if (objs[x][posOfCake.Y] == Object.NONE)
                {   objs[x][posOfCake.Y] = Object.CAKE;
                    objs[posOfCake.X][posOfCake.Y] =Object.NONE;
                    posOfCake.X = x;
                    cake.position = boards[posOfCake.X][posOfCake.Y].position;
                }
                else  {   Debug.Log(objs[posOfCake.X][posOfCake.Y]);
                    break;
                }

            }

        }
        else if (dir == Vector2.left)
        {
            while (posOfCake.Y > 0)
            {
                int y = posOfCake.Y - 1;
                if (objs[posOfCake.X][y] == Object.NONE)
                {
                    objs[posOfCake.X][y] = Object.CAKE;
                    objs[posOfCake.X][posOfCake.Y] = Object.NONE;
                    posOfCake.Y = y;
                    cake.position = boards[posOfCake.X][posOfCake.Y].position;
                }
                else  {   Debug.Log(objs[posOfCake.X][posOfCake.Y]);
                    break;
                }

            }
        }

        else if (dir == Vector2.right)
        {
            while (posOfCake.Y < objs.Count - 1)
            {
                int y = posOfCake.Y + 1;
                if (objs[posOfCake.X][y] == Object.NONE)
                {   objs[posOfCake.X][y] = Object.CAKE;
                    objs[posOfCake.X][posOfCake.Y] = Object.NONE;
                    posOfCake.Y = y;
                    cake.position = boards[posOfCake.X][posOfCake.Y].position;
                }
                else  {   Debug.Log(objs[posOfCake.X][posOfCake.Y]);
                    break;
                }

            }

        }
    }
}




[System.Serializable]
public struct pos
        {
            public int X;
            public int Y;

            public pos(int x, int y)
            {
                X = x;
                Y = y;
            }

            public bool Compare(pos other)
            {
                return X == other.X && Y == (other.Y - 1 );
            }
        } 
