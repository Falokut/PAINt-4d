using System.Drawing;

namespace Paint.Service
{
    internal class SaveService
    {
        static string currentFilename;

        static public bool Save(Bitmap bitmap)
        {
            if (currentFilename == "" || currentFilename == null) return false;
            bitmap.Save(currentFilename);
            return true;
        }

       static public void SaveAs(string filename, Bitmap bitmap)
        {
            currentFilename = filename;
            Save(bitmap);
        }

        static public Bitmap Load(string filename)
        {
            return new Bitmap(filename);
        }
    }
}
