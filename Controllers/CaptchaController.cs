using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;

namespace FMDSS.Controllers
{
    public class CaptchaController : Controller
    {
        //
        // GET: /Captcha/

        public ActionResult Generate(string p)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question 

            char[] characters = RandomString(6);
            var captcha = string.Format("{0}", new string(characters));

            //store answer 
            Session["Captcha" + p] = captcha;

            //image stream 
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(200, 50))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                WriteLine(gfx, rand);

                gfx.DrawString("", new Font("Arial", rand.Next(22, 28)), PickRandomBrush(rand), 25, 5);
                for (int i = 0; i < characters.Length; i++)
                {
                    gfx.DrawString(characters[i].ToString(), new Font("Arial", rand.Next(22, 28)), PickRandomBrush(rand), (i + 1) * 25, 5);
                }


                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }
        private Brush PickRandomBrush(Random rnd)
        {
            Brush[] b = { Brushes.DarkBlue, Brushes.DarkGreen, Brushes.DarkMagenta, Brushes.Black, Brushes.DarkMagenta, Brushes.DarkOrchid, Brushes.Red, Brushes.DeepPink };
            return b[rnd.Next(0, b.Length - 1)];
        }
        private void WriteLine(Graphics gfx, Random rand)
        {
            int n = rand.Next(1, 3);
            switch (n)
            {
                case 1:
                    for (int i = 1; i < 50; i++)
                    {
                        gfx.DrawLine(new Pen(Brushes.LightGray), i * 4, 0, i * 4, 55);
                    }
                    break;
                case 2:
                    for (int i = 1; i < 15; i++)
                    {
                        gfx.DrawLine(new Pen(Brushes.LightGray), 0, i * 4, 200, i * 4);
                    }
                    break;
                default:
                    for (int i = 1; i < 50; i++)
                    {
                        gfx.DrawLine(new Pen(Brushes.LightGray), i * 4, 0, i * 4, 55);
                    }
                    for (int i = 1; i < 15; i++)
                    {
                        gfx.DrawLine(new Pen(Brushes.LightGray), 0, i * 4, 200, i * 4);
                    }
                    break;
            }            
        }

        private Random random = new Random();
        private char[] RandomString(int length)
        {
            var chars = "0123456789";
            return Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray();
        }
    }
}
