using CommunityToolkit.Maui.Storage;
using System.Collections.ObjectModel;
using System.IO;

namespace CalculatorApp
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<string> _calculationHistory = new ObservableCollection<string>();
        private string _currentCalculation = "";

        public MainPage()
        {
            InitializeComponent();

            // Загрузка истории из локального хранилища
            LoadHistory();

            // Инициализация списка истории
            HistoryListView.ItemsSource = _calculationHistory;
        }

        private async void LoadHistory()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "calculator_history.txt");

                if (File.Exists(filePath))
                {
                    var historyLines = await File.ReadAllLinesAsync(filePath);
                    foreach (var line in historyLines)
                    {
                        _calculationHistory.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке истории: " + ex.Message);
            }
        }

        private async void SaveHistory()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "calculator_history.txt");
                await File.WriteAllLinesAsync(filePath, _calculationHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении истории: " + ex.Message);
            }
        }

        private void NumberButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            _currentCalculation += button.Text;
            DisplayResult.Text = _currentCalculation;
        }

        private void OperatorButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            _currentCalculation += button.Text;
            DisplayResult.Text = _currentCalculation;
        }

        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = CalculateExpression(_currentCalculation);
                _calculationHistory.Add($"{_currentCalculation} = {result}");
                _currentCalculation = result.ToString();
                DisplayResult.Text = _currentCalculation;
                SaveHistory();
            }
            catch (Exception ex)
            {
                DisplayResult.Text = "Ошибка";
                Console.WriteLine("Ошибка при вычислении: " + ex.Message);
            }
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            _currentCalculation = "";
            DisplayResult.Text = "0";
        }

        private double CalculateExpression(string expression)
        {
            // Простая реализация вычисления выражения
            if (expression.Contains("+"))
            {
                var parts = expression.Split('+');
                return double.Parse(parts[0]) + double.Parse(parts[1]);
            }
            else if (expression.Contains("-"))
            {
                var parts = expression.Split('-');
                return double.Parse(parts[0]) - double.Parse(parts[1]);
            }
            else if (expression.Contains("*"))
            {
                var parts = expression.Split('*');
                return double.Parse(parts[0]) * double.Parse(parts[1]);
            }
            else if (expression.Contains("/"))
            {
                var parts = expression.Split('/');
                return double.Parse(parts[0]) / double.Parse(parts[1]);
            }
            else
            {
                return double.Parse(expression);
            }
        }
    }
}
