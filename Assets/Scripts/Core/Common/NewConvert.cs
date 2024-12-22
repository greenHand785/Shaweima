using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NewConvert
{

    public static string To16Str(int value)
    {
        string testString = Convert.ToString(value, 16);
        string result = "0x";
        for(int i = 0; i < 8 - testString.Length; i++)
        {
            result += 0;
        }
        return result + testString;
    }

}

