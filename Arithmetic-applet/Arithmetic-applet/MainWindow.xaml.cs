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

namespace Arithmetic_applet
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    
    public class publicValue
    {
        public static String formula;//式子
        public static int questionAnswer;//正确答案
        public static int questionUserAnswer;//用户答案
        public static int sumDoCount = 0;//计数器，当前做了多少道题
        public static int rightQuestionCount = 0;//正确答案计数器
        public static int errorQuestionCount = 0;//错误答案计数器
        public static int score = 0;//得分
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void begin_click(object sender, RoutedEventArgs e)
        {
            answerBox.IsEnabled = true;
            beginButton.IsEnabled = false;
            confirmButton.IsEnabled = true;
            submitButton.IsEnabled = true;
            againButton.IsEnabled = true;
            answerList.IsEnabled = true;
            resultBox.IsEnabled = true;
        }

        private void set_question(object sender, RoutedEventArgs e)
        {
            int firstNumber, secondNumber;
            int symbols;

            Random random = new Random();

            while (true)
            {
                symbols = random.Next(0, 4);//0:+  1:-  2:*  3:/
                firstNumber = random.Next(0, 11);
                secondNumber = random.Next(0, 11);

                //减法情况下，前一个数不能小于后面一个数
                if (symbols == 1 && firstNumber < secondNumber)
                    continue;
                //除法情况下，不能出现小数，除数不能为0， 分子不能大于分母
                else if (symbols == 3 && ( firstNumber < secondNumber || secondNumber == 0 || firstNumber % secondNumber != 0))
                    continue;
                else
                {
                    switch (symbols)
                    {
                        case 0://+
                            publicValue.questionAnswer = firstNumber + secondNumber;
                            publicValue.formula = firstNumber.ToString() + "+" + secondNumber.ToString();
                            questionBox.Content = publicValue.formula;
                            break;
                        case 1://-
                            publicValue.questionAnswer = firstNumber - secondNumber;
                            publicValue.formula = firstNumber.ToString() + "-" + secondNumber.ToString();
                            questionBox.Content = publicValue.formula;
                            break;
                        case 2://*
                            publicValue.questionAnswer = firstNumber * secondNumber;
                            publicValue.formula = firstNumber.ToString() + "×" + secondNumber.ToString();
                            questionBox.Content = publicValue.formula;
                            break;
                        case 3:///
                            publicValue.questionAnswer = firstNumber / secondNumber;
                            publicValue.formula = firstNumber.ToString() + "÷" + secondNumber.ToString();
                            questionBox.Content = publicValue.formula;
                            break;
                        default:
                            break;
                    }
                    break;
                }
            }
        }

        private void answer_operation(object sender, RoutedEventArgs e)
        {
            String inListContent;//写入列表框的内容
            String content = answerBox.Text;//获取用户输入内容
            int userAnswer = Int32.Parse(content);

            if(publicValue.questionAnswer == userAnswer)
            {
                inListContent = publicValue.formula + "=" + userAnswer.ToString() + "正确";
                answerList.Items.Add(inListContent);
                answerBox.Text = "";
                answerBox.Focus();
            }
            else
            {
                inListContent = publicValue.formula + "=" + userAnswer.ToString() + "错误\t" + "正确答案：" + publicValue.questionAnswer.ToString();
                answerList.Items.Add(inListContent);
                answerBox.Text = "";
                answerBox.Focus();
            }
            set_question(sender, e);//重新设置题目
        }
    }
}
