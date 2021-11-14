namespace CrossWordFiller
{
    public static class StringExt
    {
        public static bool IsMatch(this string word, string mask)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] != '0' && mask[i] != word[i])
                    return false;
            }

            return true;
        }
    }
}
