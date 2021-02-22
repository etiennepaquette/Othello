using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private const int Dimension = 8;

    private Cell[,] Board = new Cell[Dimension, Dimension];
    private bool blackTurn;

    public GameObject prefDisc;

    void Start()
    {
        // Init
        for (int y = 0; y < Dimension; y++)
        {
            for (int x = 0; x < Dimension; x++)
            {
                Board[x, y] = new Cell();
            }
        }

        blackTurn = true;

        // Starter pieces
        PlacePiece(3, 3, "Black");
        PlacePiece(4, 4, "Black");
        PlacePiece(4, 3, "White");
        PlacePiece(3, 4, "White");
    }

    private void PlacePiece(int x, int y, string color)
    {
        GameObject go = Instantiate(prefDisc, new Vector3(x + 0.5f, 0, y + 0.5f), Quaternion.identity, this.transform);
        if (color == "Black")
            go.transform.RotateAround(go.transform.position, go.transform.forward, 180f);

        Board[x, y].color = color;
        Board[x, y].transform = go.transform;

        blackTurn = !blackTurn;
    }   

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 10; // To be able to get the correct screen position
            int x = (int)Camera.main.ScreenToWorldPoint(pos).x;
            int y = (int)Camera.main.ScreenToWorldPoint(pos).z;

            if (ValidPlacement(x,y)) {
                PlacePiece(x, y, blackTurn? "Black" : "White");
            }
        }
    }

    private bool ValidPlacement(int posX, int posY)
    {
        bool valid = false;
        string color = blackTurn ? "Black" : "White";

        // Out of bound verification
        if (posX < 0 || posX >= Dimension || posY < 0 || posY >= Dimension)
            return false;

        // If there's already a piece where we clicked
        if (Board[posX, posY].transform != null)
            return false;

        //Vertical Check
        if ((posY + 2) < Dimension && Board[posX, posY + 1].transform != null && Board[posX, posY + 1].color != color) 
        {
            for (int y = posY + 2; y < Dimension; y++)
            {
                if (Board[posX, y].transform == null)
                    break;

                if (Board[posX, y].transform != null && Board[posX, y].color == color) 
                {
                    valid = true;
                    for (int yy = posY + 1; yy < y; yy++)
                    {
                        Flip(Board[posX, yy]);
                    }
                    break;
                }

            }
        }
        if ((posY - 2) > 0 && Board[posX, posY - 1].transform != null && Board[posX, posY - 1].color != color)
        {
            for (int y = posY - 2; y > 0; y--)
            {
                if (Board[posX, y].transform == null)
                    break;

                if (Board[posX, y].transform != null && Board[posX, y].color == color)
                {
                    valid = true;
                    for (int yy = posY - 1; yy > y; yy--)
                    {
                        Flip(Board[posX, yy]);
                    }
                    break;
                }

            }
        }

        //Horizontal Check
        if ((posX + 2) < Dimension && Board[posX + 1, posY].transform != null && Board[posX + 1, posY].color != color)
        {
            for (int x = posX + 2; x < Dimension; x++)
            {
                if (Board[x, posY].transform == null)
                    break;

                if (Board[x, posY].transform != null && Board[x, posY].color == color)
                {
                    valid = true;
                    for (int xx = posX + 1; xx < x; xx++)
                    {
                        Flip(Board[xx, posY]);
                    }
                    break;
                }

            }
        }
        if ((posX - 2) > 0 && Board[posX - 1, posY].transform != null && Board[posX - 1, posY].color != color)
        {
            for (int x = posX - 2; x > 0; x--)
            {
                if (Board[x, posY].transform == null)
                    break;

                if (Board[x, posY].transform != null && Board[x, posY].color == color)
                {
                    valid = true;
                    for (int xx = posX - 1; xx > x; xx--)
                    {
                        Flip(Board[xx, posY]);
                    }
                    break;
                }

            }
        }

        //Diagonal / Check
        if ((posX + 2) < Dimension && (posY + 2) < Dimension && 
            Board[posX + 1, posY + 1].transform != null && Board[posX + 1, posY + 1].color != color)
        {
            int y = posY + 2;
            for (int x = posX + 2; x < Dimension; x++, y++)
            {
                if (Board[x, y].transform == null)
                    break;

                if (Board[x, y].transform != null && Board[x, y].color == color)
                {
                    valid = true;
                    int yy = posY + 1;
                    for (int xx = posX + 1; xx < x; xx++, yy++)
                    {
                        Flip(Board[xx, yy]);
                    }
                    break;
                }

            }
        }
        if ((posX - 2) > 0 && (posY - 2) > 0 &&
            Board[posX - 1, posY - 1].transform != null && Board[posX - 1, posY - 1].color != color)
        {
            int y = posY - 2;
            for (int x = posX - 2; x > 0; x--, y--)
            {
                if (Board[x, y].transform == null)
                    break;

                if (Board[x, y].transform != null && Board[x, y].color == color)
                {
                    valid = true;
                    int yy = posY - 1;
                    for (int xx = posX - 1; xx > x; xx--, y--)
                    {
                        Flip(Board[xx, yy]);
                    }
                    break;
                }

            }
        }

        //Diagonal \ Check
        if ((posX + 2) < Dimension && (posY - 2) > 0 &&
            Board[posX + 1, posY - 1].transform != null && Board[posX + 1, posY - 1].color != color)
        {
            int y = posY - 2;
            for (int x = posX + 2; x < Dimension; x++, y--)
            {
                if (Board[x, y].transform == null)
                    break;

                if (Board[x, y].transform != null && Board[x, y].color == color)
                {
                    valid = true;
                    int yy = posY - 1;
                    for (int xx = posX + 1; xx < x; xx++, yy--)
                    {
                        Flip(Board[xx, yy]);
                    }
                    break;
                }

            }
        }
        if ((posX - 2) > 0 && (posY + 2) < Dimension &&
            Board[posX - 1, posY + 1].transform != null && Board[posX - 1, posY + 1].color != color)
        {
            int y = posY + 2;
            for (int x = posX - 2; x > 0; x--, y++)
            {
                if (Board[x, y].transform == null)
                    break;

                if (Board[x, y].transform != null && Board[x, y].color == color)
                {
                    valid = true;
                    int yy = posY + 1;
                    for (int xx = posX - 1; xx > x; xx--, yy++)
                    {
                        Flip(Board[xx, yy]);
                    }
                    break;
                }

            }
        }

        return valid;
    }

    private void Flip(Cell cell)
    {
        cell.color = cell.color == "Black" ? "White" : "Black";
        cell.transform.RotateAround(cell.transform.position, cell.transform.forward, 180f);
    }
}
