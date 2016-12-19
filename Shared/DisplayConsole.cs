using System;
using System.Collections.Generic;
using System.Text;

namespace Sean.Shared
{
 
//000     001 ☺   002 ☻   003 ♥   004 ♦   005 ♣   006 ♠   007
//008 .   009 .   010 .   011 ♂   012 ♀   013 .   014 ♫   015 ☼
//016 ►   017 ◄   018 ↕   019 ‼   020 ¶   021 §   022 ▬   023 ↨
//024 ↑   025 ↓   026 →   027 ←   028 ∟   029 ↔   030 ▲   031 ▼
//032     033 !   034 "   035 #   036 $   037 %   038 &   039 '
//040 (   041 )   042 *   043 +   044 ,   045 -   046 .   047 /
//048 0   049 1   050 2   051 3   052 4   053 5   054 6   055 7
//056 8   057 9   058 :   059 ;   060 <   061 =   062 >   063 ?
//064 @   065 A   066 B   067 C   068 D   069 E   070 F   071 G
//072 H   073 I   074 J   075 K   076 L   077 M   078 N   079 O
//080 P   081 Q   082 R   083 S   084 T   085 U   086 V   087 W
//088 X   089 Y   090 Z   091 [   092 \   093 ]   094 ^   095 _
//096 `   097 a   098 b   099 c   100 d   101 e   102 f   103 g
//104 h   105 i   106 j   107 k   108 l   109 m   110 n   111 o
//112 p   113 q   114 r   115 s   116 t   117 u   118 v   119 w
//120 x   121 y   122 z   123 {   124 |   125 }   126 ~   127 ⌂
//128 Ç   129 ü   130 é   131 â   132 ä   133 à   134 å   135 ç
//136 ê   137 ë   138 è   139 ï   140 î   141 ì   142 Ä   143 Å
//144 É   145 æ   146 Æ   147 ô   148 ö   149 ò   150 û   151 ù
//152 ÿ   153 Ö   154 Ü   155 ¢   156 £   157 ¥   158 P   159 ƒ
//160 á   161 í   162 ó   163 ú   164 ñ   165 Ñ   166 ª   167 º
//168 ¿   169 ¬   170 ¬   171 ½   172 ¼   173 ¡   174 «   175 »
//176 ░   177 ▒   178 ▓   179 │   180 ┤   181 ╣   182 ╣   183 ╗
//184 ╗   185 ╣   186 ║   187 ╗   188 ╝   189 ╝   190 ╝   191 ┐
//192 └   193 ┴   194 ┬   195 ├   196 ─   197 ┼   198 ╠   199 ╠
//200 ╚   201 ╔   202 ╩   203 ╦   204 ╠   205 ═   206 ╬   207 ╩
//208 ╩   209 ╦   210 ╦   211 ╚   212 ╚   213 ╔   214 ╔   215 ╬
//216 ╬   217 ┘   218 ┌   219 █   220 ▄   221 █   222 █   223 ▀
//224 a   225 ß   226 G   227 p   228 S   229 s   230 µ   231 t
//232 F   233 Ø   234 O   235 d   236 8   237 f   238 e   239 n
//240 =   241 ±   242 =   243 =   244 (   245 )   246 ÷   247 ~
//248 °   249    250 ·   251 V   252 n   253 ²   254 ■

    public class DisplayConsoleWindow
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        private int cursorX;
        private int cursorY;

        public DisplayConsoleWindow(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void WriteChar(int x,int y, char chr)
        {
            Console.SetCursorPosition(X+1+x, Y+1+y);
            Console.Write(chr);
        }
        public void Clear()
        {
            cursorX = 0;
            cursorY = 0;
            // TODO
        }
        public void Write(string str)
        {
            foreach(var chr in str)
            {
                if (cursorX >= Width)
                {
                    cursorX = 0; cursorY++;
                }
                WriteChar(cursorX, cursorY, chr);
                cursorX++;
            }
        }
        public void WriteNewLine()
        {
            cursorX = 0;
            cursorY++;
        }

        public void RenderBorder()
        {
            char topRi = Encoding.GetEncoding(437).GetChars(new byte[] { 183 })[0];
            char topLe = Encoding.GetEncoding(437).GetChars(new byte[] { 201 })[0];
            char botRi = Encoding.GetEncoding(437).GetChars(new byte[] { 188 })[0];
            char botLe = Encoding.GetEncoding(437).GetChars(new byte[] { 211 })[0];
            char vert = Encoding.GetEncoding(437).GetChars(new byte[] { 186 })[0];
            char hori = Encoding.GetEncoding(437).GetChars(new byte[] { 205 })[0];

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(X, Y);
            Console.Write($"{topLe}{new String(hori, Width-2)}{topRi}");
            for (int i = 1; i < Height-1; i++)
            {
                Console.SetCursorPosition(X, Y+i);
                Console.Write($"{vert}{new String(' ', Width - 2)}{vert}");
            }
            Console.SetCursorPosition(X, Y+Height-1);
            Console.Write($"{botLe}{new String(hori, Width-2)}{botRi}");
            Console.ResetColor();
        }
    }

    public class DisplayConsole
    {
        private List<DisplayConsoleWindow> windows;

        public DisplayConsole()
        {
            windows = new List<DisplayConsoleWindow>();

            Console.Clear();

            // Scrolling - Get rid of the scroll bars by making the buffer the same size as the window
            Console.SetWindowSize(65, 33);
           
            //// Windows
            //Console.BufferWidth = 65;
            //Console.BufferHeight = 33;

            //// Make the window much smaller than the buffer and scroll around
            //Console.SetWindowSize(20, 5);
            //    for (int left = 5; left< 25; left++)
            //    {
            //         Console.WindowLeft = left;
            //         System.Threading.Thread.Sleep(100);
            //     }
            //    for (int top = 5; top< 20; top++)
            //    {
            //         Console.WindowTop = top;
            //         System.Threading.Thread.Sleep(100);
            //     }
            //    Console.SetWindowSize(65, 33);
        }

        public void Render()
        {
            foreach (var win in windows)
            {
                win.RenderBorder();
            }
        }

        public DisplayConsoleWindow AddWindow(string name, int x,int y,int width,int height)
        {
            var win = new DisplayConsoleWindow(x,y,width,height);
            windows.Add(win);
            win.RenderBorder();
            return win;
        }
    }
}
