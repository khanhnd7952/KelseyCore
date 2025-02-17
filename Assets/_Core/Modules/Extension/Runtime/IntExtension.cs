namespace Kelsey.Extension
{
    public static class IntExtension
    {
        public static string To000Number(this int input)
        {
            if (input < 10)
            {
                return "00" + input;
            }
            else if (input < 100)
            {
                return "0" + input;
            }
            else
            {
                return input.ToString();
            }
        }
    }
}