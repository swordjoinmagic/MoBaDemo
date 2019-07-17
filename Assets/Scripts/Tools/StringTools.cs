using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class StringTools {
    public static int FindAnyCharCount(this string str,char ch) {
        int result = 0;
        foreach (char c in str) {
            if (c == ch)
                result += 1;
        }
        return result; 
    }
}

