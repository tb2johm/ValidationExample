using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationExample.Models
{
    public class Version
    {
        public int Major { get; set; }
        public int Minor { get; set; }

        public Version() { }

        public Version(int major, int minor)
        {
            this.Major = major;
            this.Minor = minor;
        }

        public override string ToString()
        {
            return $"{this.Major}.{this.Minor}";
        }

        public static Version Copy(Version src)
        {
            var version = new Version(src.Major, src.Minor);

            return version;
        }

        public static bool TryParse(string text, out Version version)
        {
            var texts = text.Split('.');
            if (texts.Length == 2)
            {
                if (int.TryParse(texts[0], out int major) && int.TryParse(texts[1], out int minor))
                {
                    version = new Version(major, minor);
                    return true;
                }
            }

            version = null;
            return false;
        }

        public static Version TryParse(string text)
        {
            TryParse(text, out Version version);
            return version;
        }
    }
}
