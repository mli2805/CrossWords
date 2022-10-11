using System;

namespace CrossWord
{
    public static class StringExt
    {
        public static int IndexOfNot(this string str, char ch, int startIndex)
        {
            for (int i = startIndex; i < str.Length; i++)
            {
                if (str[i] != ch)
                    return i;
            }

            return -1;
        }

        public static PlaceInLine? FindFirstPlaceForWord(this string str, int startIndex)
        {
            var open = -1;
            for (int i = startIndex; i < str.Length; i++)
            {
                if (open == -1 && str[i] != '1')
                    open = i;

                if (open != -1 && str[i] == '1')
                {
                    if (i - open < 2)
                    {
                        open = -1;
                        continue;
                    }

                    return new PlaceInLine()
                    {
                        StartIdx = open,
                        Length = i - open,
                    };
                }
            }

            return null;
        }

        public static bool IsMatch(this string word, string mask)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] != '0' && mask[i] != word[i])
                    return false;
            }

            return true;
        }

        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
