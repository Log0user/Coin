using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.IO;

namespace Coin
{
    public partial class Form1 : Form
    {
        // Определения функций из user32.dll
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Константы для модификаторов клавиш (Alt, Ctrl, Shift, Win)
        private const uint MOD_NONE = 0x0000; // Без модификаторов
        private const uint MOD_ALT = 0x0001; // Alt
        private const uint MOD_CONTROL = 0x0002; // Ctrl
        private const uint MOD_SHIFT = 0x0004; // Shift
        private const uint MOD_WIN = 0x0008; // Win

        // Константа для клавиши F8
        private const uint VK_F8 = 0x77; // Код клавиши F8

        // ID горячей клавиши
        private const int HOTKEY_ID = 1;




        private NotifyIcon notifyIcon;
        public Form1()
        {
            // Создаем объект NotifyIcon
            InitializeComponent();

            // Регистрация горячей клавиши F8 при инициализации формы
            RegisterHotKey(this.Handle, HOTKEY_ID, MOD_NONE, VK_F8);


            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.Text = "Coin";

            // подписываемся на событие Move
            this.Move += Form1_Move;

            
            // Подключаем обработчик события MouseClick
            notifyIcon.MouseClick += notifyIcon_MouseClick;

            this.KeyPreview = true; // Установка свойства KeyPreview в true для перехвата f8
        }


        // Метод для обработки сообщений Windows
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Проверяем сообщения о нажатии горячей клавиши
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == HOTKEY_ID)
            {
                // Обработка нажатия горячей клавиши F8
                //MessageBox.Show("Горячая клавиша F8 нажата!");                 //--------------------F8

                Show();
                this.Location = new Point(182, 182);
                WindowState = FormWindowState.Normal;
                button1_Click(null,null);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "OwO";
            button1.BackColor = Color.Orange;
            await Gen();
        }

        private async void Form1_Load(object sender, EventArgs e)
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
                Form2 form = new Form2();
                form.ShowDialog();
               // Console.WriteLine("Файл 'api.txt' не найден.");
            }
                        
            await Gen();
        }

        private async Task Gen()
        {
            string apiKey = ""; //api ключ вводить сюда

            // Получаем путь к директории, в которой запущена программа
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формируем путь к файлу "api.txt"
            string filePath = Path.Combine(currentDirectory, "api.txt");

            try
            {
                // Читаем содержимое файла api.txt
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line)) // Проверяем, что строка не пустая
                    {
                        apiKey = line.Trim(); // Удаляем лишние пробелы из ключа
                        break; // Прекращаем поиск после первой непустой строки
                    }
                }

                if (checkBox1.Checked == false) //Если включена отладка
                {
                    MessageBox.Show($"API ключ успешно загружен: " + apiKey, "OK"); // Уcпех
                }
            }
            catch (Exception ex)
            {
                if (checkBox1.Checked == false) //Если включена отладка
                {
                    MessageBox.Show($"Ошибка при чтении файла api.txt: {ex.Message}", "Error"); // Антиуспех
                }
            }            

            string apiUrl = $"https://api.random.org/json-rpc/2/invoke";
            string jsonRequest = $"{{\"jsonrpc\": \"2.0\", \"method\": \"generateIntegers\", \"params\": {{\"apiKey\": \"{apiKey}\", \"n\": 1, \"min\": 0, \"max\": 1, \"replacement\": true, \"base\": 10}}, \"id\": 1}}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        // Разбор JSON-ответа
                        using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                        {
                            JsonElement root = document.RootElement;

                            // Извлечение значения случайного числа
                            int randomNumber = root.GetProperty("result").GetProperty("random").GetProperty("data")[0].GetInt32();

                            // Установка текста кнопки, сообразно значению случайного числа                            

                            label1.Text = "Использован RandomOrg";
                            if (randomNumber == 0)
                            {
                                button1.Text = "No";
                                button1.BackColor = Color.Green;
                            }
                            else if (randomNumber == 1)
                            {
                                button1.Text = "Yes";
                                button1.BackColor = Color.Blue;
                            }
                        }
                    }
                    else
                    {
                        // Генерация случайного числа локально
                        Random random = new Random();
                        int randomNumber = random.Next(0, 2); // Генерация случайного числа от 0 до 1

                        // Установка текста кнопки, сообразно значению локально сгенерированного случайного числа

                        label1.Text = "Локальная генерация.";
                        label2.Text = "Малая энтропия случаности";
                        if (randomNumber == 0)
                        {
                            button1.Text = "No";
                            button1.BackColor = Color.Green;
                        }
                        else if (randomNumber == 1)
                        {
                            button1.Text = "Yes";
                            button1.BackColor = Color.Blue;
                        }

                        if (checkBox1.Checked == false) //Если включена отладка
                        {
                            MessageBox.Show($"Ошибка при выполнении запроса: {response.StatusCode}", "Error");
                        }

                    }
                }
                catch (Exception ex)
                {
                    // Генерация случайного числа локально в случае исключения
                    Random random = new Random();
                    int randomNumber = random.Next(0, 2); // Генерация случайного числа от 0 до 1

                    // Установка текста кнопки, сообразно значению локально сгенерированного случайного числа

                    label1.Text = "Локальная генерация.";
                    label2.Text = "Малая энтропия случаности";
                    if (randomNumber == 0)
                    {
                        button1.Text = "No";
                        button1.BackColor = Color.Green;
                    }
                    else if (randomNumber == 1)
                    {
                        button1.Text = "Yes";
                        button1.BackColor = Color.Blue;
                    }
                    if (checkBox1.Checked == false)  //Если включена отладка
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Exception");
                    }
                }
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e) //Даблклик по скрытой форме
        {
            Show();
            this.Location = new Point(182, 182);
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide(); //Скрыть форму               
            }
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            int x = this.Location.X;
            int y = this.Location.Y;
            //label3.Text = $"Форма находится на координатах ({x}, {y})";
            if (x > 1500 && y > 1000)
            {
                Hide();
            }
        }

        // Обработчик события MouseClick для notifyIcon -ПКМ
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripMenuItem exitItem = new ToolStripMenuItem("Выход");
                exitItem.Click += ExitItem_Click;
                menu.Items.Add(exitItem);
                notifyIcon.ContextMenuStrip = menu;
                menu.Show(Control.MousePosition);
            }
        }


        // Обработчик события для опции "Выход" в контекстном меню
        private void ExitItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
               
    }
}
