using KR.HushchaPavlo702;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Updated UI
// Test commit

namespace KR_HushchaPavlo702
{
    public partial class Form1 : Form
    {
        // Підключення до MySQL (XAMPP)
        string connectionString = "server=localhost;database=sport_competition;uid=root;pwd=;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Завантаження таблиці participants у DataGridView
        private void LoadData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM participants";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);

                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridViewParticipants.DataSource = table;
            }
        }
   

        // ДОДАТИ
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO participants (surname, coach, result, distance) VALUES (@surname,@coach,@result,@distance)";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@surname", textBoxSurname.Text);
                cmd.Parameters.AddWithValue("@coach", textBoxCoach.Text);
                cmd.Parameters.AddWithValue("@result", textBoxResult.Text);
                cmd.Parameters.AddWithValue("@distance", comboBoxDistance.Text);

                cmd.ExecuteNonQuery();
            }

            LoadData();
        }

        // РЕДАГУВАТИ
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewParticipants.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridViewParticipants.CurrentRow.Cells["id"].Value);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE participants SET surname=@surname, coach=@coach, result=@result, distance=@distance WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@surname", textBoxSurname.Text);
                cmd.Parameters.AddWithValue("@coach", textBoxCoach.Text);
                cmd.Parameters.AddWithValue("@result", textBoxResult.Text);
                cmd.Parameters.AddWithValue("@distance", comboBoxDistance.Text);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            LoadData();
        }

        // ВИДАЛИТИ
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewParticipants.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridViewParticipants.CurrentRow.Cells["id"].Value);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM participants WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            LoadData();
        }

        // ОНОВИТИ
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // Клік по таблиці — заповнює поля редагування
        private void dataGridViewParticipants_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewParticipants.CurrentRow == null) return;

            textBoxSurname.Text = dataGridViewParticipants.CurrentRow.Cells["surname"].Value.ToString();
            textBoxCoach.Text = dataGridViewParticipants.CurrentRow.Cells["coach"].Value.ToString();
            textBoxResult.Text = dataGridViewParticipants.CurrentRow.Cells["result"].Value.ToString();
            comboBoxDistance.Text = dataGridViewParticipants.CurrentRow.Cells["distance"].Value.ToString();
        }

        // Меню: Довідка → Про автора
        private void menuAbout_Click(object sender, EventArgs e)
        {
            Form2 about = new Form2();
            about.ShowDialog();
        }


        private void menuProgram_Click(object sender, EventArgs e)
        {
            FormProgram program = new FormProgram();
            program.ShowDialog();
        }

        private void buttonAverage_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT AVG(result) FROM participants WHERE distance = 3";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                object result = cmd.ExecuteScalar();

                MessageBox.Show("Середній результат на дистанції 3 км: " + result);
            }
        }

        private void buttonWinners_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query3 = "SELECT surname FROM participants WHERE distance=3 ORDER BY result ASC LIMIT 1";
                string query5 = "SELECT surname FROM participants WHERE distance=5 ORDER BY result ASC LIMIT 1";

                MySqlCommand cmd3 = new MySqlCommand(query3, conn);
                MySqlCommand cmd5 = new MySqlCommand(query5, conn);

                string winner3 = cmd3.ExecuteScalar()?.ToString();
                string winner5 = cmd5.ExecuteScalar()?.ToString();

                MessageBox.Show("Переможець 3 км: " + winner3 +
                                "\nПереможець 5 км: " + winner5);
            }
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            if (comboBoxSort.SelectedIndex == -1)
            {
                MessageBox.Show("Оберіть поле для сортування");
                return;
            }

            {
                string column = comboBoxSort.Text;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = $"SELECT * FROM participants ORDER BY {column}";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridViewParticipants.DataSource = table;
                }
            }
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            if (comboBoxFilter.SelectedIndex == -1)
            {
                MessageBox.Show("Оберіть поле для фільтрації");
                return;
            }
            string column = comboBoxFilter.Text;
            string value = textBoxFilter.Text;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = $"SELECT * FROM participants WHERE {column} LIKE '%{value}%'";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);

                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridViewParticipants.DataSource = table;
            }
        }
    }
    }
    
    








          

            