using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorV2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow() // Какая-то системная херня в которой мне пока в падлу разбираться, пусть пока остатется тут
        {
            InitializeComponent();
        }

        /// <summary>
        /// сначала я написал разные впомогательные функции
        /// </summary>

        public void type() // возможно не лучше название для функции, но оно говорящее. Функция выводит введенное пользователем математическое выражение
        {
            ResultTextBox.Text = sentence + value;
        }
        public void isFirst() // проверяет впервые ли пользователь символ
        {
            if (FirstIteration) FirstIteration = false;
            else calculate();
        }
        private void isNumberTyping()
        {
            if (NumberTyping) // если последние введенное заначение это число, то оно записыватеся в выражение и value очищается
            {
                sentence += value;
                NumberTyping = false;
            }
        }
        private bool IsNotNull(double number)
        {
            if (number == 0)
            {
                error(1);

                ClearAll(); 
                return false;
            }
            else return true;
        }
        private void ClearAll() // функция очищающая последнее веденное значение пользвателя
        {
            value = "";
            sentence = "";
            ResultTextBox.Text = "";
            CalculationProcessText.Text = "";
        }
        private void Clear(object sender, RoutedEventArgs e) // Обработчик кнопки, вызывающий функцию очистки
        {
            ClearAll();
        }
        private void Info(object sender, RoutedEventArgs e) // показывает сводку с информацией
        {

        }
        private void error(int code) // вывод диалогового окна с ошибкой, произошедшей при использовании калькулятора
        {
            MessageBox.Show(Convert.ToString((ErrorCodes)code), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            ClearAll();
        }

        /// <summary>
        /// затем идет перечисление основных переменных для работы калькулятора
        /// </summary>

        string sentence = ""; // тут хранится математическое выражение введеное пользователем
        string value = ""; // тут хранится текущее значение введеное пользователем, будь-то цифра, число или знак

        bool FirstIteration = true; // позволит проверить в в первые ли пользователь что-то вводит
        bool NumberTyping = false; // отслеживает печатает ли поьзователь число или вводит математический знак

        public enum ErrorCodes // список ошибок которые возможны при использовании калькулятора
        {
            ImposibleToDevideByZero = 1,
            ValueNotAllowedToBeRaisetToAPower = 3,
            MissingValue = 5,
            UnnabledToFindFactorial = 6,
            UnableToPerformCalculations = 10
        }

        /// <summary>
        /// ниже прописаны основные оброботчики кнопок
        /// </summary>

        private void Number_Click(object sender, RoutedEventArgs e) // функция передающая выобранную цифру
        {
            if (!NumberTyping) // если последние введенное заначение это символ, то он записыватеся в выражение и value очищается
            {
                sentence += value;
                value = "";
                NumberTyping = true;
            }

            string number = (string)((Button)e.OriginalSource).Content;
            value += number;

            type();
        }
        private void Cymbol_Click(object sender, RoutedEventArgs e) // функция передающая выбранное действие
        {
            isFirst();

            isNumberTyping();

            string symbol = (string)((Button)e.OriginalSource).Content;
            value = " " + symbol + " ";

            type();
        }
        private void Degree(object sender, RoutedEventArgs e) // функция возводящая ранее введеное число в степеь
        {
            if (value == "") error(5);
            else
            {
                try
                {
                    isFirst();

                    isNumberTyping();
                    value = " ^ ";

                    type();
                }
                catch
                {
                    error(3);
                }

            }
        }
        private void Notation(object sender, RoutedEventArgs e) // цуенкия переводящая число пользователя в иную систему счисления
        {
            if (value == "") error(5);
            else
            {
                try
                {
                    isFirst();
                    isNumberTyping();

                    value = " to ";

                    type();
                }
                catch 
                {
                    error(10); 
                }
            }
        }
        private void Factorial(object sender, RoutedEventArgs e) // функция находящая факториал из числа пользователя
        {
            if (value == "") error(5);
            else
            {
                try
                {
                isFirst();
                isNumberTyping();

                double nDone = 1;

                for (int i = 1; i < Convert.ToDouble(value) + 1; i++)
                {
                    nDone *= i;
                }

                value = Convert.ToString(nDone);
                CalculationProcessText.Text = sentence + "!";
                sentence = "";
                NumberTyping = true;

                type();
                }
                catch
                {
                    error(6);
                }
            }
        }

        /// <summary>
        /// ниже пописаны функции для вычисления математической формулы, введеной пользователем
        /// </summary>
 
        private void Equal(object sender, RoutedEventArgs e)
        {
            calculate();
        }
        public void calculate() // фукция для вычисления введеной математической формулы и ее отображения
        {
            try
            {
            if (NumberTyping) sentence += value; // записываев в выражение последнее введенное число
            value = "";

            string[] calculation = sentence.Split(' ');
            double nDone = 0;
            double nWork = 0;

            for (int i = 0; i < calculation.Length; i+=2)
            {
                if (i == 0) nDone = Convert.ToDouble(calculation[i]);
                else
                {
                    nWork = Convert.ToDouble(calculation[i]);

                    switch (calculation[i - 1])
                    {
                        case "-": nDone -= nWork; break;
                        case "+": nDone += nWork; break;
                        case "*": nDone *= nWork; break;
                        case "^": nDone = Math.Pow(nDone, nWork); break;
                        case "/": if(IsNotNull(nWork)) nDone /= nWork; else ClearAll(); break;
                        case "to": // не знаю почему, но мне не удалось реализовать это через отдельную функцию, по этому пришлось все писать тут

                            int n = (int)Convert.ToDouble(nDone);
                            int s = (int)Convert.ToDouble(nWork);

                            List<int> result = new List<int>();

                            while (n > 0)
                            {
                                result.Add(n % s);
                                n /= s;
                            }
                            result.Reverse();
                            nDone = Convert.ToDouble(string.Join("", result));
                            break;
                    }
                }
            }

            value = Convert.ToString(nDone);
            CalculationProcessText.Text = sentence;
            sentence = "";
            NumberTyping = true;

            type();
            }
            catch
            {
                error(10);
            }
        }
    }
}
