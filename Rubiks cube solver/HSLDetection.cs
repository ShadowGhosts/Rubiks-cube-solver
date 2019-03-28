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
    class HSLDetection
    {
        public Color ColorFilter(Bitmap bitmap, string Face)
        {



            Point[] FR = new Point[] { new Point(124, 158), new Point(194, 92), new Point(266, 34), new Point(122, 217), new Point(413, 292), new Point(253, 114), new Point(105, 299), new Point(165, 260), new Point(237, 199) };
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
        public string ColorFilterString(Bitmap bitmap, string Face)
        {



            Point[] FR = new Point[] { new Point(124, 158), new Point(194, 92), new Point(266, 34), new Point(122, 217), new Point(413, 292), new Point(253, 114), new Point(105, 299), new Point(165, 260), new Point(237, 199) };
            Point[] FD = new Point[] { new Point(323, 442), new Point(225, 409), new Point(144, 370), new Point(375, 405), new Point(258, 119), new Point(202, 326), new Point(453, 374), new Point(367, 332), new Point(284, 275) };
            Point[] FB = new Point[] { new Point(349, 36), new Point(412, 96), new Point(489, 166), new Point(337, 108), new Point(175, 265), new Point(484, 218), new Point(324, 197), new Point(410, 257), new Point(491, 294) };
            Point[] FL = new Point[] { new Point(103, 177), new Point(172, 221), new Point(245, 278), new Point(121, 252), new Point(356, 278), new Point(248, 361), new Point(128, 300), new Point(187, 363), new Point(243, 419) };
            Point[] FU = new Point[] { new Point(143, 110), new Point(221, 68), new Point(268, 32), new Point(213, 154), new Point(258, 119), new Point(369, 64), new Point(287, 208), new Point(377, 151), new Point(450, 107) };
            Point[] FF = new Point[] { new Point(337, 288), new Point(421, 224), new Point(486, 173), new Point(331, 360), new Point(175, 265), new Point(472, 253), new Point(323, 426), new Point(399, 374), new Point(476, 310) };

            Color color;
            switch (Face)
            {
                //Face Right
                case "fR1":
                    color = AreaPixelSample(FR[0], bitmap);
                    return color.ToString();

                case "fR2":
                    color = AreaPixelSample(FR[1], bitmap);
                    return color.ToString();

                case "fR3":
                    color = AreaPixelSample(FR[2], bitmap);
                    return color.ToString();

                case "fR4":
                    color = AreaPixelSample(FR[3], bitmap);
                    return color.ToString();

                case "fR5":
                    color = AreaPixelSample(FR[4], bitmap);
                    return color.ToString();

                case "fR6":
                    color = AreaPixelSample(FR[5], bitmap);
                    return color.ToString();

                case "fR7":
                    color = AreaPixelSample(FR[6], bitmap);
                    return color.ToString();

                case "fR8":
                    color = AreaPixelSample(FR[7], bitmap);
                    return color.ToString();

                case "fR9":
                    color = AreaPixelSample(FR[8], bitmap);
                    return color.ToString();

                //Face Down
                case "fD1":
                    color = AreaPixelSample(FD[0], bitmap);
                    return color.ToString();

                case "fD2":
                    color = AreaPixelSample(FD[1], bitmap);
                    return color.ToString();

                case "fD3":
                    color = AreaPixelSample(FD[2], bitmap);
                    return color.ToString();

                case "fD4":
                    color = AreaPixelSample(FD[3], bitmap);
                    return color.ToString();

                case "fD5":
                    color = AreaPixelSample(FD[4], bitmap);
                    return color.ToString();

                case "fD6":
                    color = AreaPixelSample(FD[5], bitmap);
                    return color.ToString();

                case "fD7":
                    color = AreaPixelSample(FD[6], bitmap);
                    return color.ToString();

                case "fD8":
                    color = AreaPixelSample(FD[7], bitmap);
                    return color.ToString();

                case "fD9":
                    color = AreaPixelSample(FD[8], bitmap);
                    return color.ToString();

                //Face Back
                case "fB1":
                    color = AreaPixelSample(FB[0], bitmap);
                    return color.ToString();

                case "fB2":
                    color = AreaPixelSample(FB[1], bitmap);
                    return color.ToString();

                case "fB3":
                    color = AreaPixelSample(FB[2], bitmap);
                    return color.ToString();

                case "fB4":
                    color = AreaPixelSample(FB[3], bitmap);
                    return color.ToString();

                case "fB5":
                    color = AreaPixelSample(FB[4], bitmap);
                    return color.ToString();

                case "fB6":
                    color = AreaPixelSample(FB[5], bitmap);
                    return color.ToString();

                case "fB7":
                    color = AreaPixelSample(FB[6], bitmap);
                    return color.ToString();

                case "fB8":
                    color = AreaPixelSample(FB[7], bitmap);
                    return color.ToString();

                case "fB9":
                    color = AreaPixelSample(FB[8], bitmap);
                    return color.ToString();

                //Face Left
                case "fL1":
                    color = AreaPixelSample(FL[0], bitmap);
                    return color.ToString();

                case "fL2":
                    color = AreaPixelSample(FL[1], bitmap);
                    return color.ToString();

                case "fL3":
                    color = AreaPixelSample(FL[2], bitmap);
                    return color.ToString();

                case "fL4":
                    color = AreaPixelSample(FL[3], bitmap);
                    return color.ToString();

                case "fL5":
                    color = AreaPixelSample(FL[4], bitmap);
                    return color.ToString();

                case "fL6":
                    color = AreaPixelSample(FL[5], bitmap);
                    return color.ToString();

                case "fL7":
                    color = AreaPixelSample(FL[6], bitmap);
                    return color.ToString();

                case "fL8":
                    color = AreaPixelSample(FL[7], bitmap);
                    return color.ToString();

                case "fL9":
                    color = AreaPixelSample(FL[8], bitmap);
                    return color.ToString();

                //Face Back
                case "fU1":
                    color = AreaPixelSample(FU[0], bitmap);
                    return color.ToString();

                case "fU2":
                    color = AreaPixelSample(FU[1], bitmap);
                    return color.ToString();

                case "fU3":
                    color = AreaPixelSample(FU[2], bitmap);
                    return color.ToString();

                case "fU4":
                    color = AreaPixelSample(FU[3], bitmap);
                    return color.ToString();

                case "fU5":
                    color = AreaPixelSample(FU[4], bitmap);
                    return color.ToString();

                case "fU6":
                    color = AreaPixelSample(FU[5], bitmap);
                    return color.ToString();

                case "fU7":
                    color = AreaPixelSample(FU[6], bitmap);
                    return color.ToString();

                case "fU8":
                    color = AreaPixelSample(FU[7], bitmap);
                    return color.ToString();

                case "fU9":
                    color = AreaPixelSample(FU[8], bitmap);
                    return color.ToString();

                //Face Back
                case "fF1":
                    color = AreaPixelSample(FF[0], bitmap);
                    return color.ToString();

                case "fF2":
                    color = AreaPixelSample(FF[1], bitmap);
                    return color.ToString();

                case "fF3":
                    color = AreaPixelSample(FF[2], bitmap);
                    return color.ToString();

                case "fF4":
                    color = AreaPixelSample(FF[3], bitmap);
                    return color.ToString();

                case "fF5":
                    color = AreaPixelSample(FF[4], bitmap);
                    return color.ToString();

                case "fF6":
                    color = AreaPixelSample(FF[5], bitmap);
                    return color.ToString();

                case "fF7":
                    color = AreaPixelSample(FF[6], bitmap);
                    return color.ToString();

                case "fF8":
                    color = AreaPixelSample(FF[7], bitmap);
                    return color.ToString();

                case "fF9":
                    color = AreaPixelSample(FF[8], bitmap);
                    return color.ToString();
            }
            color = Color.Gray;
            return color.ToString();
        }


        private Color ColorCheck(Point SampleCenter, Bitmap bitmap)
        {
            Color Blue = Color.Blue;
            Color Orange = Color.Orange;
            Color Red = Color.Red;
            Color White = Color.White;
            Color Green = Color.Green;
            Color Yellow = Color.Yellow;

            Color Sample = AreaPixelSample(SampleCenter, bitmap);
            


            if (Sample.GetHue() > 200 && Sample.GetHue() < 274 && Sample.GetSaturation() > 10 && Sample.GetSaturation() < 80)
            {
                return Blue;
            }
            else if (Sample.GetHue() > 330 && Sample.GetHue() < 10 && Sample.GetSaturation() > 10 && Sample.GetSaturation() < 80)
            {
                return Red;
            }
            else if (Sample.GetHue() > 70 && Sample.GetHue() < 140 && Sample.GetSaturation() > 10 && Sample.GetSaturation() < 80)
            {
                return Green;
            }
            else if (Sample.GetHue() > 40 && Sample.GetHue() < 70 && Sample.GetSaturation() > 10 && Sample.GetSaturation() < 80)
            {
                return Yellow;
            }
            else if (Sample.GetHue() > 10 && Sample.GetHue() < 40 && Sample.GetSaturation() > 10 && Sample.GetSaturation() < 80)
            {
                return Orange;
            }
            else if (Sample.GetSaturation() > 80 )
            {
                return White;
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
