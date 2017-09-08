using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;

namespace SSG.Services.RFQ
{
    public static class ReportCommon
    {
        public static BaseColor LIGHT_BLUE = new BaseColor(26, 117, 255);
        public static BaseColor DARK_BLUE = new BaseColor(0, 51, 204);
        public static BaseColor DARK_BLUE_2 = new BaseColor(0, 77, 153);
        public static BaseColor YELLOW_GREEN = new BaseColor(0, 153, 51);
        public static BaseColor LIGHTEST_RED = new BaseColor(255, 179, 179);
        public static BaseColor LIGHT_RED = new BaseColor(255, 153, 153);
        public static BaseColor DARK_RED = new BaseColor(153, 0, 0);


        public static Font _standardFont = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
        public static Font _standardFontUnderline = new Font(Font.FontFamily.HELVETICA, 10, Font.UNDERLINE, BaseColor.BLACK);
        public static Font _standardFontBold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK);
        public static Font _standardFontWhiteBold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.WHITE);
        public static Font _standardFontRedBold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, DARK_RED);
        public static Font _standardFontDarkBlueBold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, DARK_BLUE_2);


        public static Font _standardTableFont = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);
        public static Font _standardTableFontBlue = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLUE);
        public static Font _standardTableFontUnderline = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);
        public static Font _standardRedTableFont = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.RED);
        public static Font _standardRedTableFontBold = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.RED);
        public static Font _standardTableFontBold = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK);
        public static Font _standardTableFontBoldGray = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.GRAY);
        public static Font _standardTableFontBoldWhite = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.WHITE);
        public static Font _standardGreenTableFontBold = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.GREEN);
        public static Font _standardTableFontBoldRed = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.RED);

        public static Font _largeTableFontWhiteBold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.WHITE);
        public static Font _largeTableFontGrayBold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.GRAY);
        public static Font _headerFontLightBlue = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, LIGHT_BLUE);

        public static Font _headerFontBold = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK);
        public static Font _headerFontRedBold = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.RED);
        public static Font _headerFontBlueBold = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, DARK_BLUE);
        public static Font _headerFontLightBlueBold = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, LIGHT_BLUE);
        public static Font _headerFontGreyBold = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.LIGHT_GRAY);
        public static Font _headerFontWhiteBold = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.WHITE);


        public static Font _LargeFontGrayBold = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.GRAY);
        public static Font _largeFontBold = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.BLACK);

        public static Font _veryLargeFontBold = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.BLACK);
    }
}
