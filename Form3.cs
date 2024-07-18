using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Compiler_Prroject
{
    public partial class Form3 : Form
    {
        string[] ids = new string[]
        {
                "int", "float", "string", "double", "bool", "char"
        };
        string[] reserved_word = new string[]
        {
                "for", "while", "if", "do", "return", "break", "continue", "end", "else", "switch","case", "default",":"
        };
        List<string> usedVars = new List<string>();
        Dictionary<string, char> reserved_words = new Dictionary<string, char>();
        public Form3()
        {
            InitializeComponent();
            this.Load += Form3_Load;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            reserved_words.Add("for",'(');
            reserved_words.Add("if", '(');
            reserved_words.Add("else", '{');
            reserved_words.Add("do", '{');
            reserved_words.Add("while", '(');
            reserved_words.Add("return", ';');
            reserved_words.Add("break", ';');
            reserved_words.Add("continue", ';');
            reserved_words.Add("end", ';');
            reserved_words.Add("switch", '(');

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string input = textBox1.Text;
            string[] words = input.Split(' ', '\n', '\r', '\t');
            string[] substr;
            List<string> usedVars = new List<string>();
            bool isUsed;
            string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int openParenthesesCount = 0;
            int closeParenthesesCount = 0;
            int openCurlyCount = 0;
            int closeCurlyCount = 0;
            bool flag;
            int caseCt = 0;
            int breakCt = 0;
            for (int i = 0; i < lines.Length; i++)//makes sure that every case has a break statement
            {
                substr = lines[i].Split(' ', '+', '-', '/', '%', '*', '(', ')', '{', '}', ',', ';', '<', '>', '=', '!', '&', '|', '[', ']', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ':');
                for(int k = 0; k < substr.Length; k++)
                {
                    if (substr[k] == "case")
                    {
                        caseCt++;
                    }
                    if (substr[k] == "break")
                    {
                        breakCt++;
                    }
                }
                bool flag2 = false;
                substr = lines[i].Split(' ', '/', '%', '*', '(', ')', '{', '}', ',', ';', '<', '>', '!', '&', '|', '[', ']', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ':');
                for (int k = 0; k < substr.Length; k++)
                {
                    for(int j = 0; j < substr[k].Length; j++)
                    {
                        if (substr[k][j] == '+')
                        {
                            flag2 = true;
                            if(j < substr[k].Length - 1 && (substr[k][j + 1] != '+' || substr[k][j + 1] != '='))
                            {
                                flag2 = false;
                                break;
                            }
                        }
                        if (substr[k][j] == '-')
                        {
                            flag2 = true;
                            if (j < substr[k].Length - 1 && (substr[k][j + 1] != '-' || substr[k][j + 1] != '='))
                            {
                                flag2 = false;
                                break;
                            }
                        }
                    }
                }
                if(flag2)
                {
                    listBox1.Items.Add("Missing sign");
                }
            }
            if(caseCt != breakCt)
            {
                listBox1.Items.Add("Missing a break statement.");
            }
            foreach (string line in lines)
            {
                flag = false;
                if (!string.IsNullOrWhiteSpace(line))
                {
                    substr = line.Split(' ','+', '-', '/', '%', '*', '(', ')', '{', '}', ',', ';', '<', '>', '=', '!', '&', '|', '[', ']', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
                    if (line[line.Length - 1] == ';' || line[line.Length - 1] == ')' || line[line.Length - 1] == '}' || line[line.Length - 1] == '{' || line[line.Length - 1] == ':')
                    {//checks if the line ends with a valid character or if it ends with : and has case in it
                        flag = true;
                        if(line[line.Length - 1] == ':')
                        {
                            flag = false;
                            for (int k = 0; k < substr.Length; k++)
                            {
                                if (substr[k] == "case")
                                {
                                    flag = true;
                                }
                            }
                        }
                    }
                    if(flag == false)
                    {
                        listBox1.Items.Add("Incorrect ending for the line.");
                    }
                    for (int k = 0; k < substr.Length; k++)//makes sure that every case ends with :
                    {
                        if (substr[k] == "case" && line[line.Length - 1] != ':')
                        {
                            listBox1.Items.Add("case should end with \":\"");
                        }
                    }
                    for (int p = 0; p < line.Length; p++)//makes sure that there is an expression between the brackets
                    {
                        if (line.Length - 1 > p &&line[p] == '(' && line[p + 1] == ')')
                        {
                            listBox1.Items.Add("Missing Expression between the brackets.");
                        }
                    }
                    foreach (char c in input)//checks for mismatched brackets
                    {
                        if (c == '(')
                        {
                            openParenthesesCount++;
                        }
                        else if (c == ')')
                        {
                            closeParenthesesCount++;
                        }
                        else if (c == '{')
                        {
                            openCurlyCount++;
                        }
                        else if (c == '}')
                        {
                            closeCurlyCount++;
                        }
                        
                    }
                    if(openCurlyCount != closeCurlyCount)
                    {
                        listBox1.Items.Add("Mismatched curly brackets");
                        break;
                    }
                    if (openParenthesesCount != closeParenthesesCount)
                    {
                        listBox1.Items.Add("Mismatched parentheses");
                        break;
                    }

                }
            }
            
            for (int i = 0; i < words.Length; i++)
            {
                isUsed = false;
                substr = words[i].Split('+', '-', '/', '%', '*', '(', ')', '{', '}', ',', ';', '<', '>', '=', '!', '&', '|', '[', ']', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0',':');
                for (int k = 0; k < substr.Length; k++)
                {
                    isUsed = false;
                    for (int j = 0; j < reserved_word.Length; j++)//checks if the word is a reserved word
                    {
                        if (substr[k] == reserved_word[j])
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    for (int j = 0; j < ids.Length; j++)
                    {
                        if (substr[k] == ids[j] )
                        {
                            if (words.Length > i + 1 && !isUsed && !string.IsNullOrWhiteSpace(words[i + 1]))
                            {
                                if ((words.Length > i + 2 && !isUsed && !string.IsNullOrWhiteSpace(words[i + 2]) && (words[i + 2] == ";" || words[i + 2] == "=")) || (words[i + 1][words[i + 1].Length - 1] == ';' || words[i + 1][words[i + 1].Length - 1] == '='))
                                {
                                    bool used2 = false;
                                    bool used3 = false;
                                    string[] juniorStr = words[i + 1].Split(';', '=');
                                    for (int l = 0;l < reserved_word.Length; l++)
                                    {
                                        if (juniorStr[0] == reserved_word[l])
                                        {
                                            used2 = true;
                                            break;
                                        }
                                    }
                                    for (int l = 0; l < ids.Length; l++)
                                    {
                                        if (juniorStr[0] == ids[l])
                                        {
                                            used2 = true;
                                            break;
                                        }
                                    }
                                    for (int l = 0; l < usedVars.Count; l++)
                                    {
                                        if (juniorStr[0] == usedVars[l])
                                        {
                                            listBox1.Items.Add(juniorStr[0]+ " is already defined.");
                                            used3 = true;
                                            break;
                                        }
                                    }
                                    if (!used2 && !used3)
                                    {
                                        usedVars.Add(juniorStr[0]);
                                    }
                                    else if(!used3)
                                    {
                                        listBox1.Items.Add(juniorStr[0] + " cannot be a variable.");
                                    }
                                    if (words[i + 1][words[i + 1].Length - 1] == '=')
                                    {
                                        listBox1.Items.Add("Line cannot end with '='.");
                                        isUsed = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    listBox1.Items.Add("Missing ; or invalid syntax");
                                }
                            }
                            isUsed = true;
                            
                        }
                    }
                    for (int j = 0; j < usedVars.Count; j++)
                    {
                        if (usedVars[j] == substr[k])
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    if (!isUsed && !string.IsNullOrWhiteSpace(substr[k]))
                    {
                        listBox1.Items.Add("Undeclared Variable \"" + substr[k] + "\"");
                    }
                }


                for (int j = 0; j < substr.Length; j++)
                {
                    if (reserved_words.ContainsKey(substr[j]) == true)
                    {
                        if ((words.Length > i + 1 && !string.IsNullOrWhiteSpace(words[i + 1]) && words[i + 1][0] == reserved_words[substr[j]]) || (words[i].Length > substr[j].Length && words[i][substr[j].Length] == reserved_words[substr[j]]))
                        {
                            //No Errors
                        }
                        else
                        {
                            listBox1.Items.Add("Missing " + reserved_words[substr[j]]);
                        }
                    }
                    
                }
                
            }
            
        }
    }
}
