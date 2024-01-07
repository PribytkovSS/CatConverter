namespace CatConverter
{
    public class DescriptionData
    {
        public DescriptionData() { }
        public DescriptionData(string[] dataLines)
        {
            // Number of description lines goes first in data file
            if (Int16.TryParse(dataLines[0], out var cnt))
            {
                Lines = new string[cnt];
                for (int i = 0; i < cnt; i++)
                    Lines[i] = dataLines[i+1];
            }
        }

        public string[] Lines { get; set; } = Array.Empty<string>();
    }
}