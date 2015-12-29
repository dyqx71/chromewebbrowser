using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Collections;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "http://image.zzd.sm.cn/14134331185176514123.gif?id=0";

            string filename =str.Substring(23, str.IndexOf('?')-23);

            var strat = DateTime.Now;
            Console.WriteLine("123");

            Console.WriteLine(Class_mysql_conn.Run_SQL("select * from tb_article", Class_mysql_conn.ConnStr).ToString());

            var end = DateTime.Now;

            int timeCost = (int)((strat - end).TotalMilliseconds);

            Console.WriteLine(timeCost.ToString());


            Console.Read();
        }

        /// <summary>
        /// 异步接受数据
        /// </summary>
        /// <param name="asyncResult"></param>
        public void AsyncDownLoad(IAsyncResult asyncResult)
        {
            WebRequest request = (WebRequest)asyncResult.AsyncState;
            string url = request.RequestUri.ToString();


            WebResponse response = request.EndGetResponse(asyncResult);


            using (Stream stream = response.GetResponseStream())
            {
                Image img = Image.FromStream(stream);
                string[] tmpUrl = url.Split('.');
                img.Save(string.Concat("", "/", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ".", tmpUrl[tmpUrl.Length - 1]));
                img.Dispose();
                stream.Close();
            }
        }



        /**/
        /// <summary>
        ///  加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="iTheImage">Image对象（以此图片为水印）</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private void addWatermarkImage(Graphics picture, Image iTheImage,
            string _watermarkPosition, int _width, int _height)
        {
            Image watermark = new Bitmap(iTheImage);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            float[][] colorMatrixElements = {
                                                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                                                new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                                                new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                                                new float[] {0.0f, 0.0f, 0.0f, 0.3f, 0.0f},
                                                new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                                            };
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            int xpos = 0;
            int ypos = 0;
            int WatermarkWidth = 0;
            int WatermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((_width > watermark.Width * 4) && (_height > watermark.Height * 4))
            {
                bl = 1;
            }
            else if ((_width > watermark.Width * 4) && (_height < watermark.Height * 4))
            {
                bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

            }
            else if ((_width < watermark.Width * 4) && (_height > watermark.Height * 4))
            {
                bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);
            }
            else
            {
                if ((_width * watermark.Height) > (_height * watermark.Width))
                {
                    bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

                }
                else
                {
                    bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);

                }

            }
            WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height * bl);
            switch (_watermarkPosition)
            {
                case "WM_TOP_LEFT":
                    xpos = 10;
                    ypos = 10;
                    break;
                case "WM_TOP_RIGHT":
                    xpos = _width - WatermarkWidth - 10;
                    ypos = 10;
                    break;
                case "WM_BOTTOM_RIGHT":
                    xpos = _width - WatermarkWidth - 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
                case "WM_BOTTOM_LEFT":
                    xpos = 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
            }
            picture.DrawImage(
                watermark,
                new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight),
                0,
                0,
                watermark.Width,
                watermark.Height,
                GraphicsUnit.Pixel,
                imageAttributes);
            watermark.Dispose();
            imageAttributes.Dispose();
        }


        public static void AddWaterText(string oldpath, string savepath, string watertext,string color, int alpha)
        {
            Image image = Image.FromFile(oldpath);
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            Font font = new Font("arial", 18);
            SizeF ziSizeF = new SizeF();
            ziSizeF = graphics.MeasureString(watertext, font);
            float x = 0f;
            float y = 0f;
            //switch (position)
            //{
            //    case WaterPositionMode.LeftTop:
            //        x = ziSizeF.Width / 2f;
            //        y = 8f;
            //        break;
            //    case WaterPositionMode.LeftBottom:
            //        x = ziSizeF.Width / 2f;
            //        y = image.Height - ziSizeF.Height;
            //        break;
            //    case WaterPositionMode.RightTop:
            //        x = image.Width * 1f - ziSizeF.Width / 2f;
            //        y = 8f;
            //        break;
            //    case WaterPositionMode.RightBottom:
            //        x = image.Width - ziSizeF.Width;
            //        y = image.Height - ziSizeF.Height;
            //        break;
            //    case WaterPositionMode.Center:
            //        x = image.Width / 2;
            //        y = image.Height / 2 - ziSizeF.Height / 2;
            //        break;
            //}

            x = image.Width - ziSizeF.Width;
            y = image.Height - ziSizeF.Height;
            try
            {
                StringFormat stringFormat = new StringFormat { Alignment = StringAlignment.Center };
                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));
                //graphics.DrawString(watertext, font, solidBrush, x + 1f, y + 1f, stringFormat);


                //ArrayList loca = GetLocation("", null, null);
                //graphics.DrawImage(null, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Height));
                SolidBrush brush = new SolidBrush(Color.FromArgb(alpha, ColorTranslator.FromHtml(color)));
                graphics.DrawString(watertext, font, brush, x, y, stringFormat);
                solidBrush.Dispose();
                brush.Dispose();
                bitmap.Save(savepath, ImageFormat.Jpeg);
            }
            catch (Exception e)
            {


            }
            finally
            {
                bitmap.Dispose();
                image.Dispose();
            }

        }



        /**/
        /// <summary>
        /// 图片水印位置处理方法
        /// </summary>
        /// <param name="location">水印位置</param>
        /// <param name="img">需要添加水印的图片</param>
        /// <param name="waterimg">水印图片</param>

        private  ArrayList GetLocation(string location, Image img, Image waterimg)
        {
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;
            if (location == "LT")
            {
                x = 10;
                y = 10;
            }
            else if (location == "T")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else if (location == "RT")
            {
                x = img.Width - waterimg.Width;
                y = 10;
            }
            else if (location == "LC")
            {
                x = 10;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - waterimg.Width;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "LB")
            {
                x = 10;
                y = img.Height - waterimg.Height;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else
            {
                x = img.Width - waterimg.Width;
                y = img.Height - waterimg.Height;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;
        } 



        /// <summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param   name= "originalImagePath ">源图路径（物理路径） </param> 
        /// <param   name= "thumbnailPath "> 缩略图路径（物理路径） </param> 
        /// <param   name= "width "> 缩略图宽度 </param> 
        /// <param   name= "height "> 缩略图高度 </param> 
        /// <param   name= "mode "> 生成缩略图的方式 </param>         
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW "://指定高宽缩放（可能变形）                                 
                    break;
                case "W "://指定宽，高按比例                                         
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H "://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut "://指定高宽裁减（不变形）                                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                    new System.Drawing.Rectangle(x, y, ow, oh),
                    System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 检测图片类型
        /// </summary>
        /// <param name="_fileExt"></param>
        /// <returns>正确返回True</returns>
        private bool CheckFileExt(string _fileExt)
        {
            string[] allowExt = new string[] { ".gif", ".jpg", ".jpeg" };
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i] == _fileExt) { return true; }
            }
            return false;
        }
    }
}
