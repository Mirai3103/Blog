using System.Text.RegularExpressions;

namespace BlogApp.Utils;

public class Helper
{
    public static string Slugify(string text)
    {
        for (var i = 32; i < 48; i++)
        {

            text = text.Replace(((char)i).ToString(), " ");

        }
        text = text.Replace(".", "-");

        text = text.Replace(" ", "-");

        text = text.Replace(",", "-");

        text = text.Replace(";", "-");

        text = text.Replace(":", "-");

        Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

        string strFormD = text.Normalize(System.Text.NormalizationForm.FormD );

        return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

    }

}