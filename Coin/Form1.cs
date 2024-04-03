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
        // ����������� ������� �� user32.dll
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // ��������� ��� ������������� ������ (Alt, Ctrl, Shift, Win)
        private const uint MOD_NONE = 0x0000; // ��� �������������
        private const uint MOD_ALT = 0x0001; // Alt
        private const uint MOD_CONTROL = 0x0002; // Ctrl
        private const uint MOD_SHIFT = 0x0004; // Shift
        private const uint MOD_WIN = 0x0008; // Win

        // ��������� ��� ������� F8
        private const uint VK_F8 = 0x77; // ��� ������� F8

        // ID ������� �������
        private const int HOTKEY_ID = 1;




        private NotifyIcon notifyIcon;
        public Form1()
        {
            // ������� ������ NotifyIcon
            InitializeComponent();

            // ����������� ������� ������� F8 ��� ������������� �����
            RegisterHotKey(this.Handle, HOTKEY_ID, MOD_NONE, VK_F8);


            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.Text = "Coin";

            // ������������� �� ������� Move
            this.Move += Form1_Move;

            
            // ���������� ���������� ������� MouseClick
            notifyIcon.MouseClick += notifyIcon_MouseClick;

            this.KeyPreview = true; // ��������� �������� KeyPreview � true ��� ��������� f8
        }


        // ����� ��� ��������� ��������� Windows
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // ��������� ��������� � ������� ������� �������
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == HOTKEY_ID)
            {
                // ��������� ������� ������� ������� F8
                //MessageBox.Show("������� ������� F8 ������!");                 //--------------------F8

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

            // �������� ���� � ����������, � ������� �������� ���������
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // ��������� ���� � ����� "api.txt"
            string filePath = Path.Combine(currentDirectory, "api.txt");

            // ��������� ������������� �����
            if (File.Exists(filePath))
            {
                
                //Console.WriteLine("���� 'api.txt' ������.");
            }
            else
            {
                Form2 form = new Form2();
                form.ShowDialog();
               // Console.WriteLine("���� 'api.txt' �� ������.");
            }
                        
            await Gen();
        }

        private async Task Gen()
        {
            string apiKey = ""; //api ���� ������� ����

            // �������� ���� � ����������, � ������� �������� ���������
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // ��������� ���� � ����� "api.txt"
            string filePath = Path.Combine(currentDirectory, "api.txt");

            try
            {
                // ������ ���������� ����� api.txt
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line)) // ���������, ��� ������ �� ������
                    {
                        apiKey = line.Trim(); // ������� ������ ������� �� �����
                        break; // ���������� ����� ����� ������ �������� ������
                    }
                }

                if (checkBox1.Checked == false) //���� �������� �������
                {
                    MessageBox.Show($"API ���� ������� ��������: " + apiKey, "OK"); // �c���
                }
            }
            catch (Exception ex)
            {
                if (checkBox1.Checked == false) //���� �������� �������
                {
                    MessageBox.Show($"������ ��� ������ ����� api.txt: {ex.Message}", "Error"); // ���������
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

                        // ������ JSON-������
                        using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                        {
                            JsonElement root = document.RootElement;

                            // ���������� �������� ���������� �����
                            int randomNumber = root.GetProperty("result").GetProperty("random").GetProperty("data")[0].GetInt32();

                            // ��������� ������ ������, ��������� �������� ���������� �����                            

                            label1.Text = "����������� RandomOrg";
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
                        // ��������� ���������� ����� ��������
                        Random random = new Random();
                        int randomNumber = random.Next(0, 2); // ��������� ���������� ����� �� 0 �� 1

                        // ��������� ������ ������, ��������� �������� �������� ���������������� ���������� �����

                        label1.Text = "��������� ���������.";
                        label2.Text = "����� �������� ����������";
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

                        if (checkBox1.Checked == false) //���� �������� �������
                        {
                            MessageBox.Show($"������ ��� ���������� �������: {response.StatusCode}", "Error");
                        }

                    }
                }
                catch (Exception ex)
                {
                    // ��������� ���������� ����� �������� � ������ ����������
                    Random random = new Random();
                    int randomNumber = random.Next(0, 2); // ��������� ���������� ����� �� 0 �� 1

                    // ��������� ������ ������, ��������� �������� �������� ���������������� ���������� �����

                    label1.Text = "��������� ���������.";
                    label2.Text = "����� �������� ����������";
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
                    if (checkBox1.Checked == false)  //���� �������� �������
                    {
                        MessageBox.Show($"������: {ex.Message}", "Exception");
                    }
                }
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e) //�������� �� ������� �����
        {
            Show();
            this.Location = new Point(182, 182);
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide(); //������ �����               
            }
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            int x = this.Location.X;
            int y = this.Location.Y;
            //label3.Text = $"����� ��������� �� ����������� ({x}, {y})";
            if (x > 1500 && y > 1000)
            {
                Hide();
            }
        }

        // ���������� ������� MouseClick ��� notifyIcon -���
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripMenuItem exitItem = new ToolStripMenuItem("�����");
                exitItem.Click += ExitItem_Click;
                menu.Items.Add(exitItem);
                notifyIcon.ContextMenuStrip = menu;
                menu.Show(Control.MousePosition);
            }
        }


        // ���������� ������� ��� ����� "�����" � ����������� ����
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
