using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace howtogetasks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        HttpClient httpClient = new HttpClient();

        public event PropertyChangedEventHandler? PropertyChanged;

        public Answer answer {  get; set; } =  new Answer();
        public StartClass StartClass { get; set; }
        public ResultClass ResultClass { get; set; } = new ResultClass();
        public Student Student { get; set; } =  new Student();
        public LastResult Result { get; set; } = new LastResult();
        public LastAnswer LastAnswer { get; set; } = new LastAnswer();

        public MainWindow()
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri("http://192.168.4.100:5000/api/");
            DataContext = this;
            //GetTask();
            //TryMe();
            //LetsGo();
            //Приём();
            //End();
        }
        public async void GetTask()
        {
            var responce = await httpClient.GetAsync($"Main");
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                return;
            }
            else
            {
                var content = await responce.Content.ReadAsStringAsync();
                MessageBox.Show(content);
            }
        }

        public async void SendAnswer(object sender, RoutedEventArgs e)
        {
            var myAnswer = JsonSerializer.Serialize(answer);
            var responce = await httpClient.PostAsync($"Main/Answer", new StringContent(myAnswer, Encoding.UTF8, "application/json"));
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                MessageBox.Show("error " + result);
                return;
            }
            else
            {
                var result = await responce.Content.ReadFromJsonAsync<Result>();
                MessageBox.Show( result.Result1);
            }
        }

        public async void TryMe()
        {
            StartClass = new StartClass() { Start = true };
            var arg = JsonSerializer.Serialize(StartClass);
            var responce = await httpClient.PostAsync($"Ask2/TryMe", new StringContent(arg, Encoding.UTF8, "application/json"));
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                MessageBox.Show("error " + result);
                return;
            }
            else
            {
                var result = await responce.Content.ReadFromJsonAsync<ResultClass>();
                MessageBox.Show($"{result.text}, {result.x},{result.y}");
            }
        }

        public async void GuessComment(object sender, RoutedEventArgs e)
        {
            var myAnswer = JsonSerializer.Serialize(ResultClass);
            var responce = await httpClient.PutAsync($"Ask2/Comment", new StringContent(myAnswer, Encoding.UTF8, "application/json"));
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                MessageBox.Show("error " + result);
                return;
            }
            else
            {
                var result = await responce.Content.ReadFromJsonAsync<Result>();
                MessageBox.Show(result.Result2);
            }
        }

        public async void LetsGo()
        {
            var responce = await httpClient.GetAsync($"LetsGo");
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                MessageBox.Show("error " + result);
                return;
            }
            else
            {
                var result = await responce.Content.ReadFromJsonAsync<Object>();
                MessageBox.Show($"{result.ObjectTask}{result.ObjectType}{result.ObjectJson}");
                Student = JsonSerializer.Deserialize<Student>(result.ObjectJson);
                End();
                //MessageBox.Show($"{Student.Id} {Student.Name} {Student.Birthday} {Student.Group.Title} {Student.Group.Special.Title}");
            }
        }

        public async void Приём()
        {
            var responce = await httpClient.GetAsync($"LetsGo/Приём?x=10");
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                MessageBox.Show("error " + result);
                return;
            }
            else
            {
                var content = await responce.Content.ReadAsStringAsync();
                MessageBox.Show(content);
            }
        }

        public async void End()
        {
            var arg = JsonSerializer.Serialize(Student);
            var responce = await httpClient.PostAsync($"LetsGo/End", new StringContent(arg, Encoding.UTF8, "application/json"));
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                MessageBox.Show("error " + result);
                return;
            }
            else
            {
                var content = await responce.Content.ReadAsStringAsync();
                MessageBox.Show(content);
            }
        }
        public async void ThisEndsNow(object sender, RoutedEventArgs e)
        {
            var arg = JsonSerializer.Serialize(Result);
            var responce = await httpClient.PostAsync($"TheLastOne/ThisEndsNow", new StringContent(arg, Encoding.UTF8, "application/json"));
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                MessageBox.Show("error " + result);
                return;
            }
            else
            {
                var content1 = await responce.Content.ReadAsStringAsync();
                var content = await responce.Content.ReadFromJsonAsync<LastAnswer>();
                LastAnswer = content;// JsonSerializer.Deserialize<LastAnswer>(content.Image);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastAnswer)));
                MessageBox.Show($"{content.Text}, {content.Title}");
            }
        }
    }
}