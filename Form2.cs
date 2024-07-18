using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler_Prroject
{
    public partial class Form2 : Form
    {
        string[] ids = new string[]
        {
            "int", "float", "string", "double", "bool", "char"
        };
        string[] reserved_words = new string[]
        {
            "for", "while", "if", "do", "return", "break", "continue", "end"
        };
        char[] symbolsCH = new char[]
        {
            '+', '-', '/', '%', '*', '(', ')', '{', '}', ',', ';', '&', '|', '<', '>', '=', '!', '[', ']'
        };
        char[] nums = new char[]
        {
            '1','2','3','4','5','6','7','8','9','0'
        };
        List<string> usedVars = new List<string>();
        int flagnum;
        bool isUsed;
        public Form2()
        {
            InitializeComponent();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            usedVars.Clear();
            string[] words = textBox1.Text.Split(' ','\n');
            string[] substr;
            string numTemp;
            flagnum = 0;
            for (int i = 0; i < words.Length; i++)
            {
                for (int l = 1; l < words[i].Length-1; l++)
                {
                    if (words[i][l] == '[')
                    {
                        string[] listName = words[i].Split('[');
                        if (words[i][l+1] == '\'')
                        {
                            listBox6.Items.Add(listName[0] + " is a Char Array");
                        }
                        else if (words[i][l + 1] == '"')
                        {
                            listBox6.Items.Add(listName[0] + " is a String Array");
                        }
                        else
                        {
                            listBox6.Items.Add(listName[0] + " is a Number Array");
                        }
                        string[] listName2 = words[i].Split('[',']', ',');

                        listBox6.Items.Add("With " + (listName2.Length - 2) + " Items");
                        for (int p = 1; p < listName2.Length; p++)
                        {
                            if (!string.IsNullOrWhiteSpace(listName2[p]))
                            {
                                listBox6.Items.Add(listName2[p]);
                            }
                        }
                    }
                }
                numTemp = "";
                for (int l = 0; l < words[i].Length; l++)
                {
                    for(int j = 0; j < nums.Length; j++)
                    {
                        if (words[i][l] == nums[j])
                        {
                            numTemp += words[i][l];
                            flagnum = 1;
                            
                        }
                    }
                }
                if(flagnum == 1)
                {

                    listBox5.Items.Add(numTemp);
                }
                for (int l = 0; l < words[i].Length; l++)
                {
                    for (int j = 0; j < symbolsCH.Length; j++)
                    {
                        if (words[i][l] == symbolsCH[j])
                        {
                            if (words[i][l] == '+' || words[i][l] == '-' || words[i][l] == '=')
                            {
                                if(l < words[i].Length - 1 && words[i][l] == symbolsCH[j] && words[i][l + 1] == symbolsCH[j])
                                {
                                    listBox3.Items.Add(symbolsCH[j].ToString() + symbolsCH[j].ToString());
                                    l++;
                                    break;
                                }
                            }
                            if(l < words[i].Length - 1 && (words[i][l] == '+' || words[i][l] == '-' || words[i][l] == '/' || words[i][l] == '*' || words[i][l] == '%') && words[i][l + 1] == '=')
                            {
                                listBox3.Items.Add(words[i][l].ToString() + "=");
                                l++;
                                break;
                            }
                            else
                            {
                                listBox3.Items.Add(symbolsCH[j]);
                                break;
                            }
                        }
                    }
                }
                substr = words[i].Split('+', '-', '/', '%', '*', '(', ')', '{', '}', ',', ';', '<', '>', '=', '!', '&', '|', '[', ']', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
                for (int k = 0; k < substr.Length; k++)
                {
                    isUsed = false;
                    for (int j = 0; j < ids.Length; j++)
                    {
                        if (substr[k] == ids[j])
                        {
                            listBox1.Items.Add(ids[j]);
                            isUsed = true;
                        }
                    }
                    for (int j = 0; j < reserved_words.Length; j++)
                    {
                        if (substr[k] == reserved_words[j])
                        {
                            listBox2.Items.Add(reserved_words[j]);
                            isUsed = true;
                        }
                    }
                    for (int m = 0; m < usedVars.Count; m++)
                    {
                        if (substr[k] == usedVars[m])
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    if (!isUsed && !string.IsNullOrWhiteSpace(substr[k]))
                    {
                        
                        listBox4.Items.Add(substr[k]);
                        usedVars.Add(substr[k]);
                    }
                }
            }
        }
    }
}
