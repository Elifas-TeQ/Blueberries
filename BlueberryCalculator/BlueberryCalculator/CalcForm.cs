using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueberryCalculator
{
    public partial class CalcForm : Form
    {
        Stopwatch clickTimeSpan = new Stopwatch();
        public CalcForm()
        {
            InitializeComponent();

            var buttons = new Control[17] {
                buttonZero, buttonOne, buttonTwo, buttonThree, buttonFour, buttonFive,
                buttonSix, buttonSeven, buttonEight, buttonNine, buttonOpenParenthesis,
                buttonCloseParenthesis, buttonDivide, buttonMultiply, buttonMinus,
                buttonPlus, buttonMod
            };


            foreach (Control c in buttons)
            {
                c.Click += InputClick;

            }
        }

        private void InputClick(object sender, EventArgs e)
        {
            if (textBoxExpression != null)
            {
                textBoxExpression.Text += ((Control)sender).Text;
            }
        }

        protected void deleteLastSymbol()
        {
            textBoxExpression.Text = textBoxExpression.Text.Remove(textBoxExpression.Text.Length - 1, 1);
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            if (textBoxExpression.TextLength != 0) deleteLastSymbol();
        }


        private void buttonClear_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls.OfType<TextBox>())
            {
                c.Text = "";
            }
        }

        private void buttonNegate_Click(object sender, EventArgs e)
        {
            if (clickTimeSpan.IsRunning)
            {
                clickTimeSpan.Stop();

                if (clickTimeSpan.ElapsedMilliseconds <= 3000)
                {
                    if (textBoxExpression.TextLength != 0 )
                    {
                        switch(textBoxExpression.Text[textBoxExpression.Text.Length - 1])
                        {
                            case 'm':
                                textBoxExpression.Text = textBoxExpression.Text.Substring(0, textBoxExpression.Text.Length - 1) + "p";
                                break;
                            case 'p':
                                textBoxExpression.Text = textBoxExpression.Text.Substring(0, textBoxExpression.Text.Length - 1) + "m";
                                break;
                            default:
                                textBoxExpression.Text += "m";
                                break;

                        }   
                        /*    
                            textBoxExpression.Text = textBoxExpression.Text.EndsWith("m") ?
                                textBoxExpression.Text.Substring(0, textBoxExpression.Text.Length - 1) + "p"
                                : textBoxExpression.Text.Substring(0, textBoxExpression.Text.Length - 1) + "m";
                        */                
                    }
                    //clickTimeSpan.Start();
                    clickTimeSpan = Stopwatch.StartNew();
                }
                else
                {
                    textBoxExpression.Text += "m";
                    clickTimeSpan = Stopwatch.StartNew();
                }
            }
            else
            {
                textBoxExpression.Text += "m";
                clickTimeSpan.Start();
            }
        }
    }
}
