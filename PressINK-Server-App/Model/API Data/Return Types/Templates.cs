using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json.Serialization;

namespace PressINK_Server_App.Model.API_Data.Return_Types
{

    public interface IPrinter
    {
        string HOST { get; set; }
        public string STATUS { get; set; }

        int Convert(string str);

    }

    public class MSP : IPrinter
    {
        public string HOST { get; set; }
        public string STATUS { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public int INK { get; set; }
        public int MAINT { get; set; }
        

        public int IMG_U { get; set; }
        public string WARNING { get; set; }
        public string MESSAGE { get; set; }

        public int Convert(string str)
        {
            try
            {
                bool success = int.TryParse(str, out int number);
                if (success)
                {
                    return number;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception ex)
            {
                return 0;
            }
            
        }
        public int Convert(int num)
        {
            try
            {
                return num;
            }
            catch (Exception ex)
            {
                return num;
            }
        }
    }


    public class STA: IPrinter
    {
        public string HOST { get; set; }
        public string STATUS { get; set; }

        public int Convert(string str)
        {
            return 0;
        }
    }

    public class Error_Dis : IPrinter //This is not a System template to output errors for null stat values. 
    {
        public string HOST { get; set; }
        public string STATUS { get; set; }
        public string ERROR { get; set; }

        public int Convert(string str)
        {
            return 0;
        }
    }
}
