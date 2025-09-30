using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;

namespace TextRPG
{
    public class Intro
    {
        public void intromessage()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n 1. 상태보기 \n 2. 인벤토리");
            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요. >>");
                
                int input = int.Parse(Console.ReadLine());
                if (input == 1)
                {
                    Console.WriteLine("상태보기를 선택했습니다.");
                    break;
                }
                else if (input == 2)
                {
                    Console.WriteLine("인벤토리를 선택했습니다.");
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

            }
        }
        static void Main(string[] args)
        {
            Intro intro = new Intro();
            intro.intromessage();
        }
    }

 
}