using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private const string APP_NAME = "Ultimate Predictor";
        private readonly string PREDICTION_CONFIG_PATH = $"{Environment.CurrentDirectory}\\predictionsConfig.json";
        private string[] _predictions;
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnPredict_Click(object sender, EventArgs e)
        {
            btnPredict.Enabled = false;
            //С ключевым словом await программа ждёт пока выполнится кусок кода, запущенный в другом потоке при помощи Task.Run.
            //При этом UI не блокируется.
            //После завершения исполнения этого кода продолжается выполнение метода.
            await Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    //Invoke нужен для того что бы обновлять элементы формы из другого потока
                    this.Invoke(new Action(() =>
                    {
                        progressBar1.Value = i;
                        this.Text = $"{i}%";
                    }));
                    Thread.Sleep(10);
                }
            });

            //Сообщение отобразится после только завершения метода с ключевым словом await.
            MessageBox.Show("Prediction");
            progressBar1.Value = 0;
            this.Text = APP_NAME;
            btnPredict.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = APP_NAME;

            try
            {
                var data = File.ReadAllText(PREDICTION_CONFIG_PATH);
                _predictions = JsonConvert.DeserializeObject<string[]>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                if (_predictions == null)
                    Close();
                else if (_predictions.Length == 0)
                {
                    MessageBox.Show("Предсказания закончились, кина не будет! =)");
                    Close();
                }
            }
        }
    }
}
