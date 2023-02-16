namespace CoreTestApp.Business.IServices
{
    public interface IConvertToRoman
    {
        bool isValid(int numToConvert);

        string ToRoman(int num);
    }
}
