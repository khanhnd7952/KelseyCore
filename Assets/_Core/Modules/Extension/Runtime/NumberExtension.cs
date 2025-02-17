using UnityEngine;

namespace Kelsey.Extension
{
    public static class NumberExtension
    {
        private static string[] prefixes = new[] {"k", "M", "B", "T", "P", "E", "Z", "Y"};


        // public static string ConvertToSIPrefixes(this float number)
        // {
        //     string text = "";
        //     if (number <= 0)
        //     {
        //         text = "0";
        //     }
        //     else if (number <= 1)
        //     {
        //         text = "1";
        //     }
        //     else if (number < 1000)
        //     {
        //         text = Mathf.Floor(number).ToString();
        //     }
        //     else if (number < 1000000)
        //     {
        //         text = (number / 1000).ToString("0.0");
        //         text += "k";
        //     }
        //     else if (number < 1000000000)
        //     {
        //         text = (number / 1000000).ToString("0.0");
        //         text += "M";
        //     }
        //     else if (number < 1000000000000)
        //     {
        //         text = (number / 1000000000).ToString("0.0");
        //         text += "B";
        //     }
        //     else if (number < 1000000000000000)
        //     {
        //         text = (number / 1000000000000).ToString("0.0");
        //         text += "T";
        //     }
        //     else if (number < 1000000000000000000)
        //     {
        //         text = (number / 1000000000000000).ToString("0.0");
        //         text += "P";
        //     }
        //     else if (number < 1000000000000000000000f)
        //     {
        //         text = (number / 1000000000000000000f).ToString("0.0");
        //         text += "E";
        //     }
        //     else if (number < 1000000000000000000000000f)
        //     {
        //         text = (number / 1000000000000000000000f).ToString("0.0");
        //         text += "Z";
        //     }
        //     else if (number < 1000000000000000000000000000f)
        //     {
        //         text = (number / 1000000000000000000000000f).ToString("0.0");
        //         text += "Y";
        //     }
        //
        //     return text;
        // }

        public static string ConvertToSIPrefixes(this float number)
        {
            string text = "";
            if (number <= 0)
            {
                text = "0";
            }
            else if (number <= 1)
            {
                text = "1";
            }
            else if (number < 1000)
            {
                text = number.ToString("0");
            }
            else
            {
                float heSo = Mathf.Log(number, 1000);
                //Debug.Log("He so: " + heSo);
                int soNguyen = (int) heSo;
                float phanNguyen = Mathf.Pow(1000, soNguyen);

                string pre = "";
                while (soNguyen > prefixes.Length)
                {
                    pre += prefixes[prefixes.Length - 1];
                    soNguyen -= prefixes.Length;
                }

                float finalNumber = number / phanNguyen;
                string format = "";
                if (finalNumber < 10)
                {
                    format = "0.00";
                }
                else if (finalNumber < 100)
                {
                    format = "00.0";
                }
                else
                {
                    format = "000";
                }

                text = (number / phanNguyen).ToString(format) + prefixes[(soNguyen - 1) % prefixes.Length] + pre;
            }

            //Debug.Log("FInal text: " + text);
            return text;
        }

        public static string ConvertTo000Number(this int input)
        {
            if (input < 10) return "00" + input;
            if (input < 100) return "0" + input;
            return input.ToString();
        }
        
        public static string ConvertTo00Number(this int input)
        {
            if (input < 10) return "0" + input;
            return input.ToString();
        }
    }
}