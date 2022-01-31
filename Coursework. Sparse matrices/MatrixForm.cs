using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coursework.Sparse_matrices
{
    public partial class MatrixForm : Form
    {
        public MatrixForm()
        {
            InitializeComponent();
        }

        // Reads non-zero elements from DataGridView and represents it as sparse matrix
        private SparseMatrix ReadMatrixFromForm(DataGridView dgv)
        {

            List<((int, int), int)> data = new List<((int, int), int)>();
            try
            {
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    for (int j = 0; j < dgv.ColumnCount; j++)
                    {
                        int currentValue = Convert.ToInt32(dgv.Rows[i].Cells[j].Value);
                        if (currentValue != 0)
                        {
                            data.Add(((i, j), currentValue));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid data in matrix", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new SparseMatrix(dgv.RowCount, dgv.ColumnCount, data);
        }
        // Set on form matrix given in sparse form, other elements sets as zero
        private void SetMatrixOnForm(SparseMatrix M, DataGridView dgv)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.ColumnCount = M.Column;
            dgv.RowCount = M.Row;
            dgv.AutoResizeRows();
            dgv.AutoResizeColumns();
            var data = M.Data;
            int counter = 0;
            for (int i = 0; i < M.Row; i++)
            {
                for (int j = 0; j < M.Column; j++)
                {
                    if (data.Count() > counter)
                    {
                        if (data[counter].Item1.Item1 == i && data[counter].Item1.Item2 == j)
                            dgv.Rows[i].Cells[j].Value = data[counter++].Item2.ToString();
                        else
                            dgv.Rows[i].Cells[j].Value = "0";
                    }
                    else
                        dgv.Rows[i].Cells[j].Value = "0";
                }
                dgv.Rows[i].HeaderCell.Value = i.ToString();
            }
                
            for (int i = 0; i < M.Column; i++)
                dgv.Columns[i].HeaderText = i.ToString();
            
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }
        // Set on form matrix in sparse style
        private void SetMatrixOnSparseForm(SparseMatrix M, DataGridView dgv)
        {

            dgv.Rows.Clear();
            int i = 0;
            dgv.RowCount = M.Data.Count();
            foreach (var el in M.Data)
            {
                dgv.Rows[i].Cells[0].Value = el.Item1.Item1.ToString();
                dgv.Rows[i].Cells[1].Value = el.Item1.Item2;
                dgv.Rows[i++].Cells[2].Value = el.Item2;
            }
        }
        // Read matrix in sparse style
        private SparseMatrix ReadMatrixFromSparseForm(DataGridView dgv, int row, int column)
        {
            List<((int, int), int)> sparseMatrixData = new List<((int, int), int)>();
            SparseMatrix M = new SparseMatrix();
            try
            {
                for (int i = 0; i < dgv.RowCount - 1; i++)
                {
                    int iIndex = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                    int jIndex = Convert.ToInt32(dgv.Rows[i].Cells[1].Value);
                    int val = Convert.ToInt32(dgv.Rows[i].Cells[2].Value);
                    if (iIndex >= row || jIndex >= column)
                        throw new Exception("Index was out of range");
                    if (val != 0)
                        sparseMatrixData.Add(((iIndex, jIndex), val));
                }

                SparseMatrix matrix = new SparseMatrix(row, column, sparseMatrixData);
                return matrix;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Index was out of range")
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Invalid data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return M;
        }
        // Set DataGridView with empty matrix row*column
        private void SetEmptyGrid(DataGridView dgv, int rows, int columns)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.ColumnCount = columns;
            dgv.RowCount = rows;
            dgv.AutoResizeRows();
            dgv.AutoResizeColumns();
            for (int i = 0; i < rows; i++)
                dgv.Rows[i].HeaderCell.Value = i.ToString();
            for (int i = 0; i < columns; i++)
                dgv.Columns[i].HeaderText = i.ToString();
            dgv.AutoResizeRowHeadersWidth( DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

        }

        // Sum first and second matrix
        private void button2_Click_1(object sender, EventArgs e)
        {
            int rows1;
            int columns1;
            int rows2;
            int columns2;
            try
            {
                rows1 = Convert.ToInt32(textBox1.Text);
                columns1 = Convert.ToInt32(textBox2.Text);
                rows2 = Convert.ToInt32(textBox4.Text);
                columns2 = Convert.ToInt32(textBox3.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid rows or columns set", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                SparseMatrix M1 = new SparseMatrix();
                SparseMatrix M2 = new SparseMatrix();

                M1 = ReadMatrixFromSparseForm(dataGridView13, rows1, columns1);
                M2 = ReadMatrixFromSparseForm(dataGridView11, rows2, columns2);
                SparseMatrix M3 = new SparseMatrix();
                M3 = M1 + M2;
                SetMatrixOnSparseForm(M3, dataGridView12);

            }
            catch (CannotBeAdded ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        // Set matrix in part TO SPARSE
        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                SetEmptyGrid(dataGridView7, Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox9.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}
        // Convetring common matrix to sparse form
        private void button7_Click(object sender, EventArgs e)
        {
            SparseMatrix M = ReadMatrixFromForm(dataGridView7);
            dataGridView8.Rows.Clear();
            int i = 0;
            dataGridView8.RowCount = M.Data.Count();
            foreach (var el in M.Data)
            {
                dataGridView8.Rows[i].Cells[0].Value = el.Item1.Item1.ToString();
                dataGridView8.Rows[i].Cells[1].Value = el.Item1.Item2;
                dataGridView8.Rows[i++].Cells[2].Value = el.Item2;
            }
        }
        // Set matrix in part FROM SPARSE 
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                SetEmptyGrid(dataGridView10, Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Convetring sparse matrix to common form
        private void button9_Click(object sender, EventArgs e)
        {
            List<((int, int), int)> sparseMatrixData = new List<((int, int), int)>();
            try
            {
                int row = Convert.ToInt32(textBox12.Text);
                int column = Convert.ToInt32(textBox11.Text);
                for (int i = 0; i < dataGridView9.RowCount - 1; i++)
                {
                    int iIndex = Convert.ToInt32(dataGridView9.Rows[i].Cells[0].Value);
                    int jIndex = Convert.ToInt32(dataGridView9.Rows[i].Cells[1].Value);
                    int val = Convert.ToInt32(dataGridView9.Rows[i].Cells[2].Value);
                    if (iIndex >= row || jIndex >= column)
                        throw new Exception("Index was out of range");
                    if (val != 0)
                        sparseMatrixData.Add(((iIndex, jIndex), val));
                }

                SparseMatrix matrix = new SparseMatrix(row, column, sparseMatrixData);
                SetMatrixOnForm(matrix, dataGridView10);
            }
            catch (Exception ex)
            {
                if(ex.Message == "Index was out of range")
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Invalid data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // prohibition to insert no numbers into the textbox (inherited by all other textboxes)
        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        // Multiply first and second matrix
        private void button1_Click(object sender, EventArgs e)
        {
            int rows1;
            int columns1;
            int rows2;
            int columns2;
            try
            {
                rows1 = Convert.ToInt32(textBox8.Text);
                columns1 = Convert.ToInt32(textBox7.Text);
                rows2 = Convert.ToInt32(textBox5.Text);
                columns2 = Convert.ToInt32(textBox6.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid rows or columns set", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                SparseMatrix M1 = ReadMatrixFromSparseForm(dataGridView1, rows1, columns1);
                SparseMatrix M2 = ReadMatrixFromSparseForm(dataGridView3, rows2, columns2);
                SparseMatrix M3 = M1 * M2;
                SetMatrixOnSparseForm(M3, dataGridView2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
