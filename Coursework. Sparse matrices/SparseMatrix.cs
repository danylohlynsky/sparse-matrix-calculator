using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework.Sparse_matrices
{
    // Abstract class fields that are common for any type of matrix
    abstract class Matrix
    {
        public int Row { get; protected set; }
        public int Column { get; protected set; }
    }
    // Main class
    class SparseMatrix : Matrix
    {
        private int ElementsAmount { get; set; }

        public List<((int, int), int)> Data = new List<((int, int), int)>();
        public SparseMatrix()
        {
            ElementsAmount = 0;
            Row = 0;
            Column = 0;
        }
        public SparseMatrix(int rows, int column)
        {
            Row = rows;
            Column = column;
            ElementsAmount = 0;
        }
        public SparseMatrix(int rows, int column, List<((int, int), int)> data)
        {
            Row = rows;
            Column = column;
            OrderData(data);
            Data = data;
            ElementsAmount = data.Count();
        }
        // Inserting some value to a given position
        private void Insert(int row, int column, int value)
        {
            if (Row < row && Column < column)
            {
                // exp
            }
            Data.Add(((row, column), value));
        }
        // Function for addind two matrices
        public static SparseMatrix Add(SparseMatrix firstMatrix, SparseMatrix secondMatrix)
        {
            if ((firstMatrix.Row != secondMatrix.Row) || (firstMatrix.Column != secondMatrix.Column))
            {
                throw new CannotBeAdded("Matrices cannot be added: the number of rows or columns is different");
            }
            SparseMatrix result = new SparseMatrix(firstMatrix.Row, firstMatrix.Column);
            int fPos = 0;
            int sPos = 0;
            while (fPos < firstMatrix.ElementsAmount && sPos < secondMatrix.ElementsAmount)
            {
                if (firstMatrix.Data[fPos].Item1.Item1 < secondMatrix.Data[sPos].Item1.Item1 ||
                    firstMatrix.Data[fPos].Item1.Item1 == secondMatrix.Data[sPos].Item1.Item1 &&
                     firstMatrix.Data[fPos].Item1.Item2 < secondMatrix.Data[sPos].Item1.Item2)
                {
                    result.Insert(firstMatrix.Data[fPos].Item1.Item1,
                        firstMatrix.Data[fPos].Item1.Item2,
                        firstMatrix.Data[fPos].Item2);
                    fPos++;
                }
                else if (firstMatrix.Data[fPos].Item1.Item1 > secondMatrix.Data[sPos].Item1.Item1 ||
                    firstMatrix.Data[fPos].Item1.Item1 == secondMatrix.Data[sPos].Item1.Item1 &&
                     firstMatrix.Data[fPos].Item1.Item2 > secondMatrix.Data[sPos].Item1.Item2)
                {
                    result.Insert(secondMatrix.Data[sPos].Item1.Item1,
                        secondMatrix.Data[sPos].Item1.Item2,
                        secondMatrix.Data[sPos].Item2);
                    sPos++;
                }
                else
                {
                    int addedValue = firstMatrix.Data[fPos].Item2 + secondMatrix.Data[sPos].Item2;
                    if (addedValue != 0)
                        result.Insert(secondMatrix.Data[sPos].Item1.Item1,
                            secondMatrix.Data[sPos].Item1.Item2,
                            addedValue);
                    sPos++;
                    fPos++;
                }
            }
            while (fPos < firstMatrix.ElementsAmount)
            {
                result.Insert(firstMatrix.Data[fPos].Item1.Item1,
                        firstMatrix.Data[fPos].Item1.Item2,
                        firstMatrix.Data[fPos].Item2);
                fPos++;
            }
            while (sPos < secondMatrix.ElementsAmount)
            {
                result.Insert(secondMatrix.Data[sPos].Item1.Item1,
                        secondMatrix.Data[sPos].Item1.Item2,
                        secondMatrix.Data[sPos].Item2);
                sPos++;
            }
            OrderData(result.Data);
            return result;
        }
        private static void Transpose(SparseMatrix matrix)
        {
            for (int i = 0; i < matrix.ElementsAmount; i++)
            {
                matrix.Data[i] = ((matrix.Data[i].Item1.Item2, matrix.Data[i].Item1.Item1), matrix.Data[i].Item2);
            }
            int tmp = matrix.Row;
            matrix.Row = matrix.Column;
            matrix.Column = tmp;
            OrderData(matrix.Data);
        }
        // Function for multiplying two matrices 
        public static SparseMatrix Multiply(SparseMatrix firstMatrix, SparseMatrix secondMatrix)
        {
            if (firstMatrix.Column != secondMatrix.Row)
            {
                throw new CannotBeMultiplied("Matrices cannot be multiplied: " +
                    "the number of colums in first matrix must be equal to number of rows in second");
            }
            Transpose(secondMatrix);

            SparseMatrix result = new SparseMatrix(firstMatrix.Row, secondMatrix.Row);
            for (int fPos = 0; fPos < firstMatrix.ElementsAmount;)
            {
                int currentRow = firstMatrix.Data[fPos].Item1.Item1;
                for (int sPos = 0; sPos < secondMatrix.ElementsAmount;)
                {
                    int currentColumn = secondMatrix.Data[sPos].Item1.Item1;
                    int fTemp = fPos;
                    int sTemp = sPos;
                    int sum = 0;

                    while (fTemp < firstMatrix.ElementsAmount &&
                        firstMatrix.Data[fTemp].Item1.Item1 == currentRow &&
                        sTemp < secondMatrix.ElementsAmount &&
                        secondMatrix.Data[sTemp].Item1.Item1 == currentColumn)
                    {
                        if (firstMatrix.Data[fTemp].Item1.Item2 < secondMatrix.Data[sTemp].Item1.Item2)
                            fTemp++;
                        else if (firstMatrix.Data[fTemp].Item1.Item2 > secondMatrix.Data[sTemp].Item1.Item2)
                            sTemp++;
                        else
                            sum += firstMatrix.Data[fTemp++].Item2 * secondMatrix.Data[sTemp].Item2;
                    }
                    if (sum != 0)
                    {
                        result.Insert(currentRow, currentColumn, sum);
                    }
                    while (sPos < secondMatrix.ElementsAmount && secondMatrix.Data[sPos].Item1.Item1 == currentColumn)
                        sPos++;

                }
                while (fPos < firstMatrix.ElementsAmount && firstMatrix.Data[fPos].Item1.Item1 == currentRow)
                    fPos++;
            }
            return result;
        }
        // Ordering sparse matrix data
        private static void OrderData(List<((int, int), int)> data)
        {
            data.Sort((y, x) =>
            {
                int result = y.Item1.Item1.CompareTo(x.Item1.Item1);
                return result == 0 ? y.Item1.Item2.CompareTo(x.Item1.Item2) : result;
            });
        }

        public static SparseMatrix operator +(SparseMatrix firstMatrix, SparseMatrix secondMatrixx)
            => Add(firstMatrix, secondMatrixx);
        public static SparseMatrix operator *(SparseMatrix firstMatrix, SparseMatrix secondMatrixx)
            => Multiply(firstMatrix, secondMatrixx);
    }
}
