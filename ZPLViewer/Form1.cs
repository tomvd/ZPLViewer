using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

/**
**
** This is a tool to visualize ZPL labels
 * Thanks to Bart De Smet for the Code39 class
 * 
** <BR><BR>
** <PRE>
** Release   Date     By  Proj    Ref      Description
** --------- -------- --- ------  -------- --------------------------------------
** 1.0       05/05/10 TVD zplview none     initial version
** </PRE>
*/

namespace ZPLViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strIn = textBox1.Text;
            strIn = strIn.Replace("\r","");
            strIn = strIn.Replace("\n","");
            parseAndPaint(strIn);
        }

        private void parseAndPaint(string p_strIn)
        {
            string strCom;
            string strParams;
            char[] delimiterChars = {','};
            int x=0, y=0;
            int type=0,next;
            int w=100, h=100, t=1;
            Graphics g = pictureBox1.CreateGraphics();
            
            p_strIn = p_strIn.Remove(0, 1);
            strCom = p_strIn.Substring(0, p_strIn.IndexOf("^"));
            while (strCom.Length > 0)
            {

                if (strCom.Substring(0,2).Equals("FT"))
	            {
                    strParams = strCom.Remove(0, 2);
                    string[] coords = strParams.Split(delimiterChars);
                    x = int.Parse(coords[0])/2;
                    y = int.Parse(coords[1])/2;
                    type = 0;
	            }else
                if (strCom.Substring(0, 2).Equals("FO"))
                {
                    strParams = strCom.Remove(0, 2);
                    string[] coords = strParams.Split(delimiterChars);
                    x = int.Parse(coords[0]) / 2;
                    y = int.Parse(coords[1]) / 2;
                    type = 1;
                }
                else
                if (strCom.Substring(0,2).Equals("FT"))
	            {
                    strParams = strCom.Remove(0, 2);
                    string[] coords = strParams.Split(delimiterChars);
                    x = int.Parse(coords[0])/2;
                    y = int.Parse(coords[1])/2;
                    type = 0;
	            }else
                if (strCom.Substring(0,2).Equals("GB"))
	            {
                    strParams = strCom.Remove(0, 2);
                    string[] coords = strParams.Split(delimiterChars);
                    w = int.Parse(coords[0]) / 2;
                    h = int.Parse(coords[1]) / 2;
                    t = int.Parse(coords[2]) / 2;
                    if (type == 0)
                        y -= h;

                    g.DrawRectangle(new Pen(Color.Black,(float)t),x,y,w,h);
                }
                else
                if (strCom.Substring(0, 2).Equals("B3"))
                {
                    strParams = strCom.Remove(0, 2);
                    string[] coords = strParams.Split(delimiterChars);
                    h = int.Parse(coords[2]) / 2;
                    if (type == 0)
                        y -= h;
                    type = 2;
                }
                else
                if (strCom.Substring(0, 1).Equals("A"))
                {
                    strParams = strCom.Remove(0, 1);
                    string[] coords = strParams.Split(delimiterChars);
                    h = int.Parse(coords[1]) / 2;
                    w = int.Parse(coords[2]) / 2;
                    type = 3;
                }
                else
                    if (strCom.Substring(0, 2).Equals("BY"))
                    {
                        strParams = strCom.Remove(0, 2);
                        string[] coords = strParams.Split(delimiterChars);
                        w = int.Parse(coords[0]) /2;
                    }
                else
                    if (strCom.Substring(0, 2).Equals("FD"))
                    {
                        strParams = strCom.Remove(0, 2);
                        if (type == 2)
                        {
                            Code39Settings cs = new Code39Settings();
                            cs.BarCodeHeight = h;
                            cs.NarrowWidth = w;
                            cs.WideWidth = 4;
                            cs.LeftMargin = x;
                            cs.TopMargin = y;
                            Code39 code = new Code39(strParams, cs);
                            code.Paint(g);
                        }
                        if (type == 3)
                        {
                            Font font = new Font(FontFamily.GenericSansSerif,12);
                            g.DrawString(strParams, font, Brushes.Black, x, y);
                        }
                    }


                p_strIn = p_strIn.Remove(0, strCom.Length + 1);
                next = p_strIn.IndexOf("^");
                if (next > 0)
                    strCom = p_strIn.Substring(0, next);
                else
                    break;
            }
        }
    }

    class Code39Settings
    {
        private int height = 80;

        public int BarCodeHeight
        {
            get { return height; }
            set { height = value; }
        }

        private bool drawText = true;

        public bool DrawText
        {
            get { return drawText; }
            set { drawText = value; }
        }

        private int leftMargin = 10;

        public int LeftMargin
        {
            get { return leftMargin; }
            set { leftMargin = value; }
        }

        private int rightMargin = 10;

        public int RightMargin
        {
            get { return rightMargin; }
            set { rightMargin = value; }
        }

        private int topMargin = 10;

        public int TopMargin
        {
            get { return topMargin; }
            set { topMargin = value; }
        }

        private int bottomMargin = 10;

        public int BottomMargin
        {
            get { return bottomMargin; }
            set { bottomMargin = value; }
        }

        private int interCharacterGap = 2;

        public int InterCharacterGap
        {
            get { return interCharacterGap; }
            set { interCharacterGap = value; }
        }

        private int wideWidth = 6;

        public int WideWidth
        {
            get { return wideWidth; }
            set { wideWidth = value; }
        }

        private int narrowWidth = 2;

        public int NarrowWidth
        {
            get { return narrowWidth; }
            set { narrowWidth = value; }
        }

        private Font font = new Font(FontFamily.GenericSansSerif, 12);

        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        private int codeToTextGapHeight = 10;

        public int BarCodeToTextGapHeight
        {
            get { return codeToTextGapHeight; }
            set { codeToTextGapHeight = value; }
        }
    }

    class Code39
    {
        #region Static initialization

        static Dictionary<char, Pattern> codes;

        static Code39()
        {
            object[][] chars = new object[][] 
            {
                new object[] {'0', "n n n w w n w n n"},
                new object[] {'1', "w n n w n n n n w"},
                new object[] {'2', "n n w w n n n n w"},
                new object[] {'3', "w n w w n n n n n"},
                new object[] {'4', "n n n w w n n n w"},
                new object[] {'5', "w n n w w n n n n"},
                new object[] {'6', "n n w w w n n n n"},
                new object[] {'7', "n n n w n n w n w"},
                new object[] {'8', "w n n w n n w n n"},
                new object[] {'9', "n n w w n n w n n"},
                new object[] {'A', "w n n n n w n n w"},
                new object[] {'B', "n n w n n w n n w"},
                new object[] {'C', "w n w n n w n n n"},
                new object[] {'D', "n n n n w w n n w"},
                new object[] {'E', "w n n n w w n n n"},
                new object[] {'F', "n n w n w w n n n"},
                new object[] {'G', "n n n n n w w n w"},
                new object[] {'H', "w n n n n w w n n"},
                new object[] {'I', "n n w n n w w n n"},
                new object[] {'J', "n n n n w w w n n"},
                new object[] {'K', "w n n n n n n w w"},
                new object[] {'L', "n n w n n n n w w"},
                new object[] {'M', "w n w n n n n w n"},
                new object[] {'N', "n n n n w n n w w"},
                new object[] {'O', "w n n n w n n w n"},
                new object[] {'P', "n n w n w n n w n"},
                new object[] {'Q', "n n n n n n w w w"},
                new object[] {'R', "w n n n n n w w n"},
                new object[] {'S', "n n w n n n w w n"},
                new object[] {'T', "n n n n w n w w n"},
                new object[] {'U', "w w n n n n n n w"},
                new object[] {'V', "n w w n n n n n w"},
                new object[] {'W', "w w w n n n n n n"},
                new object[] {'X', "n w n n w n n n w"},
                new object[] {'Y', "w w n n w n n n n"},
                new object[] {'Z', "n w w n w n n n n"},
                new object[] {'-', "n w n n n n w n w"},
                new object[] {'.', "w w n n n n w n n"},
                new object[] {' ', "n w w n n n w n n"},
                new object[] {'*', "n w n n w n w n n"},
                new object[] {'$', "n w n w n w n n n"},
                new object[] {'/', "n w n w n n n w n"},
                new object[] {'+', "n w n n n w n w n"},
                new object[] {'%', "n n n w n w n w n"}
            };

            codes = new Dictionary<char, Pattern>();
            foreach (object[] c in chars)
                codes.Add((char)c[0], Pattern.Parse((string)c[1]));
        }

        #endregion

        private static Pen pen = new Pen(Color.Black);
        private static Brush brush = Brushes.Black;

        private string code;
        private Code39Settings settings;

        public Code39(string code)
            : this(code, new Code39Settings())
        {
        }

        public Code39(string code, Code39Settings settings)
        {
            foreach (char c in code)
                if (!codes.ContainsKey(c))
                    throw new ArgumentException("Invalid character encountered in specified code.");

            if (!code.StartsWith("*"))
                code = "*" + code;
            if (!code.EndsWith("*"))
                code = code + "*";

            this.code = code;
            this.settings = settings;
        }

        public void Paint(Graphics g)
        {
            //string code = this.code.Trim('*');
            string code = this.code;

            SizeF sizeCodeText = Graphics.FromImage(new Bitmap(1, 1)).MeasureString(code, settings.Font);

            int w = settings.LeftMargin + settings.RightMargin;
            foreach (char c in this.code)
                w += codes[c].GetWidth(settings) + settings.InterCharacterGap;
            w -= settings.InterCharacterGap;

            int h = settings.TopMargin + settings.BottomMargin + settings.BarCodeHeight;

            if (settings.DrawText)
                h += settings.BarCodeToTextGapHeight + (int)sizeCodeText.Height;

            //Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            //Graphics g = Graphics.FromImage(bmp);

            int left = settings.LeftMargin;

            foreach (char c in this.code)
                left += codes[c].Paint(settings, g, left) + settings.InterCharacterGap;

            if (settings.DrawText)
            {
                int tX = settings.LeftMargin + (w - settings.LeftMargin - settings.RightMargin - (int)sizeCodeText.Width) / 2;
                if (tX < 0)
                    tX = 0;
                int tY = settings.TopMargin + settings.BarCodeHeight + settings.BarCodeToTextGapHeight;
                g.DrawString(code, settings.Font, brush, tX, tY);
            }

            //return bmp;
        }

        private class Pattern
        {
            private bool[] nw = new bool[9];

            public static Pattern Parse(string s)
            {

                s = s.Replace(" ", "").ToLower();

                Pattern p = new Pattern();

                int i = 0;
                foreach (char c in s)
                    p.nw[i++] = c == 'w';

                return p;
            }

            public int GetWidth(Code39Settings settings)
            {
                int width = 0;

                for (int i = 0; i < 9; i++)
                    width += (nw[i] ? settings.WideWidth : settings.NarrowWidth);

                return width;
            }

            public int Paint(Code39Settings settings, Graphics g, int left)
            {
                int x = left;

                int w = 0;
                for (int i = 0; i < 9; i++)
                {
                    int width = (nw[i] ? settings.WideWidth : settings.NarrowWidth);

                    if (i % 2 == 0)
                    {
                        Rectangle r = new Rectangle(x, settings.TopMargin, width, settings.BarCodeHeight);
                        g.FillRectangle(brush, r);
                    }

                    x += width;
                    w += width;
                }

                return w;
            }
        }
    }
}
