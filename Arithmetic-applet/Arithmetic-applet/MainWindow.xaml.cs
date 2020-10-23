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
    public partial class MainWindow : Window
    {
        Arithmetic arithmetic;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void begin_click(object sender, RoutedEventArgs e)
        {
            arithmetic = new Arithmetic();
            answerBox.IsEnabled = true;
            beginButton.IsEnabled = false;
            confirmButton.IsEnabled = true;
            submitButton.IsEnabled = true;
            answerList.IsEnabled = true;
            resultBox.IsEnabled = true;
            //清空答案框
            answerList.Items.Clear();
            resultBox.Text = "等待答题交卷";
            set_question(sender, e);
        }

        private void set_question(object sender, RoutedEventArgs e)
        {
            arithmetic.SetFormula();
            switch (arithmetic.formula[1])
            {
                case 0://+
                    arithmetic.strFormula = arithmetic.formula[0].ToString() + " + " + arithmetic.formula[2].ToString();
                    questionBox.Content = arithmetic.strFormula;
                    break;
                case 1://-
                    arithmetic.strFormula = arithmetic.formula[0].ToString() + " - " + arithmetic.formula[2].ToString();
                    questionBox.Content = arithmetic.strFormula;
                    break;
                case 2://*
                    arithmetic.strFormula = arithmetic.formula[0].ToString() + " × " + arithmetic.formula[2].ToString();
                    questionBox.Content = arithmetic.strFormula;
                    break;
                case 3:///
                    arithmetic.strFormula = arithmetic.formula[0].ToString() + " ÷ " + arithmetic.formula[2].ToString();
                    questionBox.Content = arithmetic.strFormula;
                    break;
                default:
                    break;
            }
        }

        private void answer_operation(object sender, RoutedEventArgs e)
        {
            String inListContent;//写入列表框的内容
            String content = answerBox.Text;//获取用户输入内容
            if (content.Length == 0)//检测用户输入是否为空
            {
                MessageBox.Show(mainWindow, "答案不能为空");
                answerBox.Focus();
            }
            else
            {
                int userAnswer = Int32.Parse(content);

                if (arithmetic.CheckAnswer(userAnswer))
                {
                    inListContent = arithmetic.strFormula + " = " + userAnswer.ToString() + " 正确";
                    answerList.Items.Insert(0, inListContent);
                    //总做题数和正确题数计数
                    arithmetic.rightCount++;
                    arithmetic.sumCount++;
                    answerBox.Text = "";
                    answerBox.Focus();
                }
                else
                {
                    inListContent = arithmetic.strFormula + " = " + userAnswer.ToString() + " 错误\t" + "正确答案：" + arithmetic.formula[3].ToString();
                    answerList.Items.Insert(0, inListContent);
                    //总做题数计数
                    arithmetic.sumCount++;
                    answerBox.Text = "";
                    answerBox.Focus();
                }
                set_question(sender, e);//重新设置题目
            }
        }

        private void submit_answer(object sender, RoutedEventArgs e)
        {
            float correctRate = arithmetic.GetCorrectRate();
            //浮点数去除多余的小数
            correctRate = (int)(correctRate * 10000);
            correctRate = (float)correctRate / 100;
            //计数器归零
            //旁边提示框展示数据统计
            resultBox.Text = "总题数：" + arithmetic.sumCount.ToString() + "\n"
                             + "正确数：" + arithmetic.rightCount.ToString() + "\n"
                             + "错误数：" + (arithmetic.sumCount - arithmetic.rightCount).ToString() + "\n"
                             + "正确率：" + correctRate.ToString() + "%\n"
                             ;

            answerBox.IsEnabled = false;
            beginButton.IsEnabled = true;
            confirmButton.IsEnabled = false;
            submitButton.IsEnabled = false;
        }
    }
}
public class Arithmetic
{
    public int rightCount = 0;//正确题数
    public int sumCount = 0;//总题数
    public int[] formula = new int[4];//保存式子的数组
    public String strFormula;
    public void SetFormula()//设置式子
    {
        int firstNumber, secondNumber;
        int symbols;

        Random random = new Random();

        while (true)
        {
            symbols = random.Next(0, 4);//0:+  1:-  2:*  3:/
            firstNumber = random.Next(0, 11);
            secondNumber = random.Next(0, 11);

            if (symbols == 1 && firstNumber < secondNumber)
                continue;//减法情况下，前一个数不能小于后面一个数
            else if (symbols == 3 && (firstNumber < secondNumber || secondNumber == 0 || firstNumber % secondNumber != 0))
                continue;//除法情况下，不能出现小数，除数不能为0， 分子不能大于分母
            else
            {
                formula[0] = firstNumber;
                formula[1] = symbols;
                formula[2] = secondNumber;
                switch (symbols)
                {
                    case 0://+
                        formula[3] = firstNumber + secondNumber;
                        break;
                    case 1://-
                        formula[3] = firstNumber - secondNumber;
                        break;
                    case 2://*
                        formula[3] = firstNumber * secondNumber;
                        break;
                    case 3:///
                        formula[3] = firstNumber / secondNumber;
                        break;
                    default:
                        break;
                }
                break;
            }
        }
    }
    public bool CheckAnswer(int inNumber)//进行答案的判断
    {
        if (inNumber == formula[3])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float GetCorrectRate()//返回正确率
    {
        return (float)rightCount / (float)sumCount;
    }
}
