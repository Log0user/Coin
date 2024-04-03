using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coin
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.KeyPreview = true; // Устанавливаем свойство KeyPreview формы в true
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Получаем путь к директории, в которой запущена программа
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формируем путь к файлу "api.txt"
            string filePath = Path.Combine(currentDirectory, "api.txt");

            try
            {
                // Создаем новый файл "api.txt" или перезаписываем существующий
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Записываем строку "0" в файл
                    writer.WriteLine("0");
                }

                MessageBox.Show("Файл 'api.txt' успешно создан и записан.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

                //Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем путь к директории, в которой запущена программа
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формируем путь к файлу "api.txt"
            string filePath = Path.Combine(currentDirectory, "api.txt");

            try
            {
                // Создаем новый файл "api.txt" или перезаписываем существующий
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    if (textBox1.Text.Length > 0)
                    {
                        // Записываем введенный api в файл
                        writer.WriteLine(textBox1.Text);

                    }
                    else
                    {
                        // Записываем строку "0" в файл
                        writer.WriteLine("0");
                    }

                }

                MessageBox.Show("Файл 'api.txt' успешно создан и записан.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                //Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }



        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Получаем путь к директории, в которой запущена программа
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формируем путь к файлу "api.txt"
            string filePath = Path.Combine(currentDirectory, "api.txt");

            // Проверяем существование файла
            if (File.Exists(filePath))
            {

                //Console.WriteLine("Файл 'api.txt' найден.");
            }
            else
            {

                DialogResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Подтверждение закрытия", MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true; // Отменяем закрытие формы
                }
                else
                {
                    Application.Exit(); // Закрываем всю программу
                }
                
            }


        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //MessageBox.Show("АААААААААААААААААААААААААААААААААААААААААААААААА", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.PerformClick(); // Вызываем нажатие кнопки
            }
        }
    }
}
