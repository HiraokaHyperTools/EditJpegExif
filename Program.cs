using EditJpegExif.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditJpegExif {
    class Program {
        static void Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("EditJpegExif inout.jpg /ascii 37510 \"{url: 'http://...'}\" ");
                Environment.ExitCode = 1;
                return;
            }
            var fpJpeg = args[0];
            using (Bitmap pic = new Bitmap(new MemoryStream(File.ReadAllBytes(fpJpeg)))) {
                for (int x = 1; x < args.Length; x++) {
                    if (args[x] == "/ascii") {
                        x++;
                        if (x < args.Length) {
                            int propId = int.Parse(args[x]);
                            x++;
                            if (x < args.Length) {
                                SetExifProp(pic, propId, Encoding.UTF8.GetBytes(args[x] + "\0"));
                            }
                        }
                    }
                    else if (args[x] == "/ascii-file") {
                        x++;
                        if (x < args.Length) {
                            int propId = int.Parse(args[x]);
                            x++;
                            if (x < args.Length) {
                                SetExifProp(pic, propId, File.ReadAllBytes(args[x]));
                            }
                        }
                    }
                }
                pic.Save(fpJpeg, ImageFormat.Jpeg);
            }
        }

        private static void SetExifProp(Bitmap pic, int propId, byte[] data) {
            PropertyItem item;
            try {
                item = pic.GetPropertyItem(propId);
            }
            catch (ArgumentException) {
                item = Resources.A.PropertyItems[0];
                item.Id = propId;
            }
            item.Type = 2;
            item.Value = data;
            item.Len = data.Length;
            pic.SetPropertyItem(item);
        }
    }
}
