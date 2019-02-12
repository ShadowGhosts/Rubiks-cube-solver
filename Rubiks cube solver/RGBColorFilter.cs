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
            ColorFiltering RedFilter = new ColorFiltering();
            // configure the filter to keep red object only
            RedFilter.Red = new IntRange(110, 255);
            RedFilter.Green = new IntRange(0, 60);
            RedFilter.Blue = new IntRange(0, 60);
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
            ColorFiltering GreenFilter = new ColorFiltering();
            // configure the filter to keep red object only
            GreenFilter.Red = new IntRange(0, 60);
            GreenFilter.Green = new IntRange(110, 255); ;
            GreenFilter.Blue = new IntRange(0, 60);
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
            filter.ApplyInPlace(unmanagedImage);
            // create color filter
            ColorFiltering YellowFilter = new ColorFiltering();
            // configure the filter to keep red object only
            YellowFilter.Red = new IntRange(100, 255);
            YellowFilter.Green = new IntRange(100, 255);
            YellowFilter.Blue = new IntRange(0, 75);
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
            ColorFiltering OrangeFilter = new ColorFiltering();
            // configure the filter to keep red object only
            OrangeFilter.Red = new IntRange(110, 255);
            OrangeFilter.Green = new IntRange(60, 150);
            OrangeFilter.Blue = new IntRange(0, 60);
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
            filter.ApplyInPlace(unmanagedImage);
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
            
            

            Point[] FR = new Point[] { new Point(283, 246), new Point(351, 204), new Point(416, 166), new Point(286, 319), new Point(356, 278), new Point(415, 238), new Point(290, 385), new Point(345, 338), new Point(413, 292) };
            Point[] FD = new Point[] { new Point(270, 70), new Point(322, 89), new Point(378, 115), new Point(210, 93), new Point(258, 119), new Point(319, 147), new Point(145, 124), new Point(195, 148), new Point(247, 185) };
            Point[] FB = new Point[] { new Point(115, 168), new Point(162, 200), new Point(217, 235), new Point(126, 238), new Point(175, 265), new Point(228, 308), new Point(140, 295), new Point(182, 325), new Point(232, 369) };
            Point[] FL = new Point[] { new Point(283, 246), new Point(351, 204), new Point(416, 166), new Point(286, 319), new Point(356, 278), new Point(415, 238), new Point(290, 385), new Point(345, 338), new Point(413, 292) };
            Point[] FU = new Point[] { new Point(270, 70), new Point(322, 89), new Point(378, 115), new Point(210, 93), new Point(258, 119), new Point(319, 147), new Point(145, 124), new Point(195, 148), new Point(247, 185) };
            Point[] FF = new Point[] { new Point(115, 168), new Point(162, 200), new Point(217, 235), new Point(126, 238), new Point(175, 265), new Point(228, 308), new Point(140, 295), new Point(182, 325), new Point(232, 369) };

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
