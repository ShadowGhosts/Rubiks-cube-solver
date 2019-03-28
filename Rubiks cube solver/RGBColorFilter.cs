using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace Rubiks_cube_solver_app
{
    class RGBColorFilter
    {
        public Bitmap RedFilter(Bitmap bitmap)
        {
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);
            
            // create color filter
            HSLFiltering RedFilter = new HSLFiltering();
            // configure the filter to keep red object only
            RedFilter.Hue = new IntRange(336, 15);
            RedFilter.Saturation = new Range(0.6f, 1);
            RedFilter.Luminance = new Range(0.1f, 1);
            // filter image
            RedFilter.ApplyInPlace(unmanagedImage);
            Bitmap managedImage = unmanagedImage.ToManagedImage();
            return managedImage;
        }

        //wrong filter value
        public Bitmap BlueFilter(Bitmap bitmap)
        {
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);
            //GammaCorrection filter = new GammaCorrection(0.29999);
            //filter.ApplyInPlace(unmanagedImage);
            // create color filter

            ColorFiltering BlueFilter = new ColorFiltering();
            // configure the filter to keep red object only
            BlueFilter.Red = new IntRange(0, 60);
            BlueFilter.Green = new IntRange(0, 60);
            BlueFilter.Blue = new IntRange(110, 255);

            /*HSLFiltering BlueFilter = new HSLFiltering();
            // configure the filter to keep red object only
            BlueFilter.Hue = new IntRange(185, 275);
            BlueFilter.Saturation = new Range(0.6f, 1);
            BlueFilter.Luminance = new Range(0.1f, 1);*/
            
            // filter image
            BlueFilter.ApplyInPlace(unmanagedImage);
            Bitmap managedImage = unmanagedImage.ToManagedImage();
            return managedImage;
        }

        //wrong filter value
        public Bitmap GreenFilter(Bitmap bitmap)
        {
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);
            // create color filter
            HSLFiltering GreenFilter = new HSLFiltering();
            // configure the filter to keep red object only
            GreenFilter.Hue = new IntRange(65, 155);
            GreenFilter.Saturation = new Range(0.6f, 1); ;
            GreenFilter.Luminance = new Range(0.1f, 1);
            // filter image
            GreenFilter.ApplyInPlace(unmanagedImage);

            Bitmap managedImage = unmanagedImage.ToManagedImage();
            return managedImage;
        }

        //wrong filter value
        public Bitmap YellowFilter(Bitmap bitmap)
        {
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);
            GammaCorrection filter = new GammaCorrection(0.29999);
            //filter.ApplyInPlace(unmanagedImage);
            // create color filter
            HSLFiltering YellowFilter = new HSLFiltering();
            // configure the filter to keep red object only
            YellowFilter.Hue = new IntRange(40, 65);
            YellowFilter.Saturation = new Range(0.6f, 1);
            YellowFilter.Luminance = new Range(0.1f, 1);
            // filter image
            YellowFilter.ApplyInPlace(unmanagedImage);
            //YellowFilter.ApplyInPlace(bitmap);

            Bitmap managedImage = unmanagedImage.ToManagedImage();
            return managedImage;
        }

        //wrong filter value
        public Bitmap OrangeFilter(Bitmap bitmap)
        {
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);
            GammaCorrection filter = new GammaCorrection(0.39999);
            //filter.ApplyInPlace(unmanagedImage);
            // create color filter
            HSLFiltering OrangeFilter = new HSLFiltering();
            // configure the filter to keep red object only
            OrangeFilter.Hue = new IntRange(15, 40);
            OrangeFilter.Saturation = new Range(0.6f, 1);
            OrangeFilter.Luminance = new Range(0.1f, 1);
            // filter image
            OrangeFilter.ApplyInPlace(unmanagedImage);

            Bitmap managedImage = unmanagedImage.ToManagedImage();
            return managedImage;
        }

        //wrong filter value
        public Bitmap WhiteFilter(Bitmap bitmap)
        {
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);
            GammaCorrection filter = new GammaCorrection(0.19999);
            //filter.ApplyInPlace(unmanagedImage);
            // create color filter
            ColorFiltering WhiteFilter = new ColorFiltering();
            // configure the filter to keep red object only
            WhiteFilter.Red = new IntRange(200, 255);
            WhiteFilter.Green = new IntRange(200, 255);
            WhiteFilter.Blue = new IntRange(200, 255);
            // filter image
            WhiteFilter.ApplyInPlace(unmanagedImage);

            Bitmap managedImage = unmanagedImage.ToManagedImage();
            return managedImage;
        }

        public Color ColorFilter(Bitmap bitmap, string Face)
        {



            Point[] FR = new Point[] { new Point(107, 157), new Point(177, 87), new Point(246, 32), new Point(107, 214), new Point(413, 292), new Point(246, 106), new Point(89, 301), new Point(156, 254), new Point(235, 194) };
            Point[] FD = new Point[] { new Point(323, 442), new Point(225, 409), new Point(144, 370), new Point(375, 405), new Point(258, 119), new Point(202, 326), new Point(453, 374), new Point(367, 332), new Point(284, 275) };
            Point[] FB = new Point[] { new Point(349, 36), new Point(412, 96), new Point(489, 166), new Point(337, 108), new Point(175, 265), new Point(484, 218), new Point(324, 197), new Point(410, 257), new Point(491, 294) };
            Point[] FL = new Point[] { new Point(103, 177), new Point(172, 221), new Point(245, 278), new Point(121, 252), new Point(356, 278), new Point(248, 361), new Point(128, 300), new Point(187, 363), new Point(243, 419) };
            Point[] FU = new Point[] { new Point(143, 110), new Point(221, 68), new Point(268, 32), new Point(213, 154), new Point(258, 119), new Point(369, 64), new Point(287, 208), new Point(377, 151), new Point(450, 107) };
            Point[] FF = new Point[] { new Point(337, 288), new Point(421, 224), new Point(486, 173), new Point(331, 360), new Point(175, 265), new Point(472, 253), new Point(323, 426), new Point(399, 374), new Point(476, 310) };

            switch (Face)
            {
                //Face Right
                case "fR1":
                    return ColorCheck(FR[0], bitmap);
               
                case "fR2":
                    return ColorCheck(FR[1], bitmap);

                case "fR3":
                    return ColorCheck(FR[2], bitmap);

                case "fR4":
                    return ColorCheck(FR[3], bitmap);

                case "fR5":
                    return ColorCheck(FR[4], bitmap);

                case "fR6":
                    return ColorCheck(FR[5], bitmap);

                case "fR7":
                    return ColorCheck(FR[6], bitmap);

                case "fR8":
                    return ColorCheck(FR[7], bitmap);

                case "fR9":
                    return ColorCheck(FR[8], bitmap);

                //Face Down
                case "fD1":
                    return ColorCheck(FD[0], bitmap);

                case "fD2":
                    return ColorCheck(FD[1], bitmap);

                case "fD3":
                    return ColorCheck(FD[2], bitmap);

                case "fD4":
                    return ColorCheck(FD[3], bitmap);

                case "fD5":
                    return ColorCheck(FD[4], bitmap);

                case "fD6":
                    return ColorCheck(FD[5], bitmap);

                case "fD7":
                    return ColorCheck(FD[6], bitmap);

                case "fD8":
                    return ColorCheck(FD[7], bitmap);

                case "fD9":
                    return ColorCheck(FD[8], bitmap);

                //Face Back
                case "fB1":
                    return ColorCheck(FB[0], bitmap);

                case "fB2":
                    return ColorCheck(FB[1], bitmap);

                case "fB3":
                    return ColorCheck(FB[2], bitmap);

                case "fB4":
                    return ColorCheck(FB[3], bitmap);

                case "fB5":
                    return ColorCheck(FB[4], bitmap);

                case "fB6":
                    return ColorCheck(FB[5], bitmap);

                case "fB7":
                    return ColorCheck(FB[6], bitmap);

                case "fB8":
                    return ColorCheck(FB[7], bitmap);

                case "fB9":
                    return ColorCheck(FB[8], bitmap);

                //Face Left
                case "fL1":
                    return ColorCheck(FL[0], bitmap);

                case "fL2":
                    return ColorCheck(FL[1], bitmap);

                case "fL3":
                    return ColorCheck(FL[2], bitmap);

                case "fL4":
                    return ColorCheck(FL[3], bitmap);

                case "fL5":
                    return ColorCheck(FL[4], bitmap);

                case "fL6":
                    return ColorCheck(FL[5], bitmap);

                case "fL7":
                    return ColorCheck(FL[6], bitmap);

                case "fL8":
                    return ColorCheck(FL[7], bitmap);

                case "fL9":
                    return ColorCheck(FL[8], bitmap);

                //Face Back
                case "fU1":
                    return ColorCheck(FU[0], bitmap);

                case "fU2":
                    return ColorCheck(FU[1], bitmap);

                case "fU3":
                    return ColorCheck(FU[2], bitmap);

                case "fU4":
                    return ColorCheck(FU[3], bitmap);

                case "fU5":
                    return ColorCheck(FU[4], bitmap);

                case "fU6":
                    return ColorCheck(FU[5], bitmap);

                case "fU7":
                    return ColorCheck(FU[6], bitmap);

                case "fU8":
                    return ColorCheck(FU[7], bitmap);

                case "fU9":
                    return ColorCheck(FU[8], bitmap);

                //Face Back
                case "fF1":
                    return ColorCheck(FF[0], bitmap);

                case "fF2":
                    return ColorCheck(FF[1], bitmap);

                case "fF3":
                    return ColorCheck(FF[2], bitmap);

                case "fF4":
                    return ColorCheck(FF[3], bitmap);

                case "fF5":
                    return ColorCheck(FF[4], bitmap);

                case "fF6":
                    return ColorCheck(FF[5], bitmap);

                case "fF7":
                    return ColorCheck(FF[6], bitmap);

                case "fF8":
                    return ColorCheck(FF[7], bitmap);

                case "fF9":
                    return ColorCheck(FF[8], bitmap);
            }
            return Color.Gray;
        }

        

        private Color ColorCheck(Point SampleCenter,  Bitmap bitmap)
        {
            Color Blue = Color.Blue;
            Color Orange = Color.Orange;
            Color Red = Color.Red;
            Color White = Color.White;
            Color Green = Color.Green;
            Color Yellow = Color.Yellow;


            Bitmap filteredblue = BlueFilter(bitmap);
            Bitmap filteredred = RedFilter(bitmap);
            Bitmap filteredgreen = GreenFilter(bitmap);
            Bitmap filteredyellow = YellowFilter(bitmap);
            Bitmap filteredorange = OrangeFilter(bitmap);
            Bitmap filteredwhite = WhiteFilter(bitmap);

            for (int i = 0; i <= 5; i++)
            {
                if (i == 0)
                {
                    Color Sample = AreaPixelSample(SampleCenter, filteredblue);
                    if (Sample.R > 20 && Sample.G > 20 && Sample.B > 20)
                    {
                        return Blue;
                    }
                }
                else if (i == 1)
                {
                    Color Sample = AreaPixelSample(SampleCenter, filteredred);
                    if (Sample.R > 20 && Sample.G > 20 && Sample.B > 20)
                    {
                        return Red;
                    }
                }
                else if (i == 2)
                {
                    Color Sample = AreaPixelSample(SampleCenter, filteredgreen);
                    if (Sample.R > 20 && Sample.G > 20 && Sample.B > 20)
                    {
                        return Green;
                    }
                }
                else if (i == 3)
                {
                    Color Sample = AreaPixelSample(SampleCenter, filteredyellow);
                    if (Sample.R > 20 && Sample.G > 20 && Sample.B > 20)
                    {
                        return Yellow;
                    }
                }
                else if (i == 4)
                {
                    Color Sample = AreaPixelSample(SampleCenter, filteredorange);
                    if (Sample.R > 20 && Sample.G > 20 && Sample.B > 20)
                    {
                        return Orange;
                    }
                }
                else if (i == 5)
                {
                    Color Sample = AreaPixelSample(SampleCenter, filteredwhite);
                    if (Sample.R > 20 && Sample.G > 20 && Sample.B > 20)
                    {
                        return White;
                    }
                }
            }
            return Color.Gray;
        }

        private Color AreaPixelSample(System.Drawing.Point CenterOfSample, Bitmap test)
        {
            CenterOfSample.X = CenterOfSample.X - 5;
            CenterOfSample.Y = CenterOfSample.Y - 5;
            int resultB = 0;
            int resultR = 0;
            int resultG = 0;
            int total = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Color area = test.GetPixel(CenterOfSample.X, CenterOfSample.Y);
                    resultB = resultB + area.B;
                    resultG = resultG + area.G;
                    resultR = resultR + area.R;
                    CenterOfSample.X++;
                    total++;
                }
                CenterOfSample.X = CenterOfSample.X - 10;
                CenterOfSample.Y++;
            }

            resultB = resultB / total;
            resultG = resultG / total;
            resultR = resultR / total;


            return Color.FromArgb(resultR, resultG, resultB);
        }

       


    }
}
