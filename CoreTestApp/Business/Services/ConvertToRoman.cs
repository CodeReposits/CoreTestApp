using CoreTestApp.Business.IServices;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text;

namespace CoreTestApp.Business.Services
{
    public class ConvertToRoman : IConvertToRoman
    {
        private readonly IOptions<RomanSettings> settings;
        public ConvertToRoman(IOptions<RomanSettings> settings)
        {
            this.settings = settings;
        }

        // Validate Given Input number range
        public bool isValid(int numToConvert)
        {
            if(this.settings.Value.isMaxValue && numToConvert <= this.settings.Value.MaxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ToRoman(int num)
        {
           var _romanValues = this.settings.Value.RomanValues?.OrderByDescending(x => x.Value).ToList();

            StringBuilder result = new StringBuilder();

           if (_romanValues != null)
            {
                foreach (var rValue in _romanValues)
                {
                    if(num >= rValue.Value)
                    {
                        /// Brack the number into 1000's, 100's, 10's, 1's
                        int ConvertToRoundedNum = GetRoundedNumber(num);

                        var _letter = GetRomanLetter(ConvertToRoundedNum, rValue, _romanValues);

                        //4 consicutive same numbers are not allowed in Roman numerics, so repalce with Next Keys.
                        var count = _letter.ToCharArray().GroupBy(X => X).Where(Y => Y.Count() > 3).ToList().Count;
                        if(count > 0)
                        {
                            _letter = GetRomanLetter(rValue, _romanValues);
                        }

                        result.Append(_letter);

                        num = num - ConvertToRoundedNum;
                    }
                }
            }

            return result.ToString();
        }

        // Building Roman numeric using given number
        private string GetRomanLetter(int num, KeyValuePair<string,int> rValue, List<KeyValuePair<string, int>> _romanValues)
        {
            string romanLetter = "";            

            var Keys = _romanValues.Select(x => x.Key).ToList<string>();
            
            while (num > 0)
            {
                if (num < rValue.Value)
                {
                    rValue = _romanValues.FirstOrDefault(x => x.Key == Keys[Keys.IndexOf(rValue.Key) + 1]);
                }                

                romanLetter += rValue.Key;
                num -= rValue.Value;
            }
            
            return romanLetter;
        }

        private string GetRomanLetter(KeyValuePair<string, int> rValue, List<KeyValuePair<string, int>> _romanValues)
        {
            string romanLetter = "";

            var Keys = _romanValues.Select(x => x.Key).ToList<string>();
            rValue = _romanValues.FirstOrDefault(x => x.Key == Keys[Keys.IndexOf(rValue.Key) + 1]);
            romanLetter = rValue.Key + Keys[Keys.IndexOf(rValue.Key) - 2];

            return romanLetter;
        }


        /// Bracking the number into either 1000's, 100's, 10's or 1's
        private int GetRoundedNumber(int num)
        {
            int NumLength = num.ToString().Length - 1;

            int Convertnum;

            if (NumLength > 0)
            {
                Convertnum = num / (int)Math.Pow(10, NumLength)  * (int)Math.Pow(10, NumLength);
            }
            else
            {
                Convertnum = num;
            }

            return Convertnum;
        }
    }
}
